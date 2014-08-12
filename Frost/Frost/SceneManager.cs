using System;
using System.Collections.Generic;
using Frost.Display;
using Frost.Utility;

namespace Frost
{
	/// <summary>
	/// Controls the flow between different segments of the game
	/// </summary>
	/// <remarks>The scene manager operates using a stack.
	/// The scene on top of the stack is executed every frame.
	/// When it is finished, the top scene is popped off the stack.</remarks>
	public class SceneManager
	{
		private readonly IDisplay _display;

		private Scene _curScene;

		/// <summary>
		/// Scene stack arranged with new scenes at the end of the list
		/// </summary>
		/// <remarks>A linked list is used here because the stack needs to be traversed.</remarks>
		private readonly LinkedList<Scene> _sceneStack = new LinkedList<Scene>();

		/// <summary>
		/// Collection of scenes that are updated and rendered each frame.
		/// This is a subset of the full scene stack.
		/// The scenes are ordered as bottom (<see cref="LinkedList{T}.First"/>) to top (<see cref="LinkedList{T}.Last"/>).
		/// </summary>
		private readonly LinkedList<Scene> _scenesToProcess = new LinkedList<Scene>();

		private readonly object _updateLocker = new object(), _renderLocker = new object();

		/// <summary>
		/// Checks if there are any scenes being processed
		/// </summary>
		public bool ScenesRemaining
		{
			get
			{
				lock(_sceneStack)
					return _curScene != null;
			}
		}

		/// <summary>
		/// Scene currently being processed.
		/// If there isn't a scene, then null is returned.
		/// </summary>
		public Scene CurrentScene
		{
			get
			{
				lock(_sceneStack)
					return _curScene;
			}
		}

		/// <summary>
		/// Creates a new scene manager
		/// </summary>
		/// <param name="initialScene">Initial scene</param>
		/// <param name="display">Display to render scenes to</param>
		/// <exception cref="ArgumentNullException">The initial scene (<paramref name="initialScene"/>) and <paramref name="display"/> to render on can't be null.</exception>
		public SceneManager (Scene initialScene, IDisplay display)
		{
			if(initialScene == null)
				throw new ArgumentNullException("initialScene");
			if(display == null)
				throw new ArgumentNullException("display");

			RenderDuplicateFrames = true;
			_display = display;
			EnterScene(initialScene);
		}

		#region Scene management
		#region EnterScene

		/// <summary>
		/// Triggered when a new scene is entered
		/// </summary>
		public event EventHandler<SceneEventArgs> SceneEntered;

		/// <summary>
		/// Called when a scene has been added to the stack
		/// </summary>
		/// <param name="args">Scene arguments</param>
		/// <remarks>This method triggers the <see cref="SceneEntered"/> event.</remarks>
		protected virtual void OnEnterScene (SceneEventArgs args)
		{
			SceneEntered.NotifyThreadedSubscribers(this, args);
		}

		/// <summary>
		/// Enters a new scene
		/// </summary>
		/// <param name="scene">Scene to enter</param>
		/// <exception cref="ArgumentNullException">A null scene can't be entered.</exception>
		public void EnterScene (Scene scene)
		{
			if(scene == null)
				throw new ArgumentNullException("scene");

			lock(_sceneStack)
				pushScene(scene);

			OnEnterScene(new SceneEventArgs(scene));
		}

		/// <summary>
		/// Pushes a scene onto the stack
		/// </summary>
		/// <param name="scene">Scene to push onto the top of the stack</param>
		private void pushScene (Scene scene)
		{
			// Assign the scene's manager
			scene.SetOwner(this);

			// Add the scene to the stack
			_sceneStack.AddLast(scene);
			_curScene = scene;

			// Update the list of scenes to process
			lock(_updateLocker)
				lock(_renderLocker)
				{
					if(!scene.AllowFallthrough) // Don't allow any other scenes to be processed
						_scenesToProcess.Clear();
					_scenesToProcess.AddLast(scene);
				}
		}
		#endregion

		#region ExitScene

		/// <summary>
		/// Triggered when a scene is left
		/// </summary>
		public event EventHandler<SceneEventArgs> SceneExited;

		/// <summary>
		/// Called when a scene has been removed from the stack
		/// </summary>
		/// <param name="args">Scene arguments</param>
		/// <remarks>This method triggers the <see cref="SceneExited"/> event.</remarks>
		protected virtual void OnExitScene (SceneEventArgs args)
		{
			SceneExited.NotifyThreadedSubscribers(this, args);
		}

		/// <summary>
		/// Leaves the current scene and goes back to the previous one
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if there are no more scenes to process</exception>
		/// <remarks>The current executing scene will finish its <see cref="Update"/> or <see cref="Render"/>.</remarks>
		public void ExitScene ()
		{
			Scene prevScene;
			lock(_sceneStack)
			{
				if(_sceneStack.Count <= 0)
					throw new InvalidOperationException("There are no more scenes left to exit from.");
				prevScene = popScene();
			}

			OnExitScene(new SceneEventArgs(prevScene));
		}

		/// <summary>
		/// Pops the top-most scene off from the stack
		/// </summary>
		/// <returns>Top-most scene that was just popped off</returns>
		private Scene popScene ()
		{
			// Remove the scene
			var popped = _sceneStack.Last.Value;
			_sceneStack.RemoveLast();

			// Remove the scene from being processed
			lock(_updateLocker)
				lock(_renderLocker)
				{
					_scenesToProcess.RemoveLast();

					if(_scenesToProcess.Count <= 0)
					{// All scenes to process removed, get the next group of scenes to process (if any)
						var curNode = _sceneStack.Last;
						while(curNode != null)
						{// Continue adding to the process list in reverse order until fall-through isn't allowed
							var scene = curNode.Value;
							_scenesToProcess.AddFirst(scene);

							if(!scene.AllowFallthrough)
								break;
							
							curNode = curNode.Previous;
						}
					}
				}

			// Tell the scene it has no manager anymore
			popped.SetOwner(null);

			// Update the current scene
			_curScene = (_sceneStack.Count > 0) ? _sceneStack.Last.Value : null;

			return popped;
		}
		#endregion
		#endregion

		#region Update

		/// <summary>
		/// Prepares for an update
		/// </summary>
		/// <param name="stepArgs">Step information</param>
		/// <remarks><paramref name="stepArgs"/> is populated with state index information.</remarks>
		internal void PreUpdate (FrameStepEventArgs stepArgs)
		{
			// ...
		}

		/// <summary>
		/// Updates the active scene
		/// </summary>
		/// <param name="stepArgs">Update information</param>
		/// <returns>True if there is still a scene to process</returns>
		/// <remarks>A return value of false indicates that the last scene has exited and the game should terminate.</remarks>
		internal bool Update (FrameStepEventArgs stepArgs)
		{
			updateSceneStack(stepArgs);
			return ScenesRemaining;
		}

		/// <summary>
		/// Steps through all reachable scenes.
		/// The stack is processed top-down so that higher scenes can intercept events before lower scenes.
		/// </summary>
		/// <param name="args">Update information</param>
		private void updateSceneStack (FrameStepEventArgs args)
		{
			lock(_updateLocker)
			{
				var curNode = _scenesToProcess.Last; // Start at the top scene
				while(curNode != null)
				{// Step through each scene that is part of the render
					var scene = curNode.Value;
					scene.Update(args);
					curNode = curNode.Previous; // Advance from top to bottom of the stack
				}
			}
		}

		/// <summary>
		/// Cleans up after an update
		/// </summary>
		/// <param name="stepArgs">Step information</param>
		internal void PostUpdate (FrameStepEventArgs stepArgs)
		{
			// ...
		}
		#endregion

		#region Render

		/// <summary>
		/// Total number of frames that were drawn multiple times.
		/// This represents the frames that were actually rendered more than once.
		/// </summary>
		public long RenderedDuplicateFrames { get; private set; }

		/// <summary>
		/// Indicates whether duplicate frames should be rendered (when game updates are behind display updates)
		/// </summary>
		public bool RenderDuplicateFrames { get; set; }

		/// <summary>
		/// Prepares for rendering
		/// </summary>
		/// <param name="drawArgs">Render information</param>
		/// <remarks><paramref name="drawArgs"/> is populated with state index information.</remarks>
		internal void PreRender (FrameDrawEventArgs drawArgs)
		{
			_display.EnterFrame();
		}

		/// <summary>
		/// Renders the active scene
		/// </summary>
		/// <param name="drawArgs">Render information</param>
		public void Render (FrameDrawEventArgs drawArgs)
		{
			if(ScenesRemaining)
			{// Only render if there's a scene
				if(RenderDuplicateFrames || !drawArgs.Duplicate)
				{// Render the frame
					renderSceneStack(drawArgs);
					if(drawArgs.Duplicate)
						++RenderedDuplicateFrames;
				}
			}
		}

		/// <summary>
		/// Draws all reachable scenes.
		/// The stack is processed bottom-up so that higher scenes are overlaid on top of lower scenes.
		/// </summary>
		/// <param name="args">Render information</param>
		private void renderSceneStack (FrameDrawEventArgs args)
		{
			lock(_renderLocker)
			{
				var curNode = _scenesToProcess.First; // Start at the bottom scene
				while(curNode != null)
				{// Step through each scene that is part of the render
					var scene = curNode.Value;
					scene.Draw(args);
					curNode = curNode.Next; // Advance from bottom to top of the stack
				}
			}
		}

		/// <summary>
		/// Cleans up after rendering
		/// </summary>
		/// <param name="drawArgs">Render information</param>
		internal void PostRender (FrameDrawEventArgs drawArgs)
		{
			_display.ExitFrame();
		}
		#endregion
	}
}
