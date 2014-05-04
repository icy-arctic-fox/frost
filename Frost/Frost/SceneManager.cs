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
		private readonly object _locker = new object();
		private readonly IDisplay _display;

		private Scene _curScene;

		/// <summary>
		/// Scene stack arranged as new scenes at the end of the list
		/// </summary>
		private readonly LinkedList<Scene> _sceneStack = new LinkedList<Scene>();

		/// <summary>
		/// Checks if there are any scenes being processed
		/// </summary>
		public bool ScenesRemaining
		{
			get
			{
				lock(_locker)
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
				lock(_locker)
					return _curScene;
			}
		}

		/// <summary>
		/// Creates a new scene manager
		/// </summary>
		/// <param name="initialScene">Initial scene</param>
		/// <param name="display">Display to render scenes to</param>
		/// <exception cref="ArgumentNullException">The initial scene (<paramref name="initialScene"/>) and <paramref name="display"/> display to render can't be null.</exception>
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

			scene.SetParentManager(this);
			lock(_locker)
			{
				_sceneStack.AddLast(scene);
				_curScene = scene;
			}

			OnEnterScene(new SceneEventArgs(scene));
		}

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
			lock(_locker)
			{
				if(_sceneStack.Count <= 0)
					throw new InvalidOperationException("There are no more scenes left to exit from.");

				prevScene = _sceneStack.Last.Value;
				_sceneStack.RemoveLast();
				prevScene.SetParentManager(null);
				_curScene = (_sceneStack.Count > 0) ? _sceneStack.Last.Value : null;
			}

			OnExitScene(new SceneEventArgs(prevScene));
		}
		#endregion

		#region Update and render

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
			_display.Update();
			updateSceneStack(stepArgs);
			return ScenesRemaining;
		}

		/// <summary>
		/// Processes all scenes in the stack from top to bottom until <see cref="Scene.AllowFallthrough"/> is false or the bottom of the stack is reached.
		/// The stack is processed top-down so that higher scenes can intercept events before lower scenes.
		/// </summary>
		/// <param name="args">Update information</param>
		private void updateSceneStack (FrameStepEventArgs args)
		{
			var curNode = _sceneStack.Last;
			while(curNode != null)
			{
				var scene = curNode.Value;
				scene.Step(args);

				if(scene.AllowFallthrough) // Fall through to the next scene
					curNode = curNode.Previous;
				else // Don't fall through, stop processing scenes
					break;
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
		/// Processes all scenes in the stack from top to bottom until <see cref="Scene.AllowFallthrough"/> is false or the bottom of the stack is reached.
		/// The stack is processed bottom-up so that higher scenes are overlaid on top of lower scenes.
		/// </summary>
		/// <param name="args">Render information</param>
		private void renderSceneStack (FrameDrawEventArgs args)
		{
			// Find the bottom node first
			var bottomNode = _sceneStack.Last;
			while(bottomNode != null)
			{
				var scene = bottomNode.Value;
				if(scene.AllowFallthrough) // Scenes below this can be rendered
					bottomNode = bottomNode.Previous;
				else // Can't render anything below this
					break;
			}

			// Render each scene from bottom to top
			var curNode = bottomNode;
			while(curNode != null)
			{
				var scene = curNode.Value;
				scene.Draw(args);
				curNode = curNode.Next;
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
