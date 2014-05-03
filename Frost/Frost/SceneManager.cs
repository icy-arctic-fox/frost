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
		private SceneStackEntry _curScene;
		private readonly Stack<SceneStackEntry> _sceneStack = new Stack<SceneStackEntry>();
		private readonly IDisplay _display;

		/// <summary>
		/// Checks if there are any scenes being processed
		/// </summary>
		public bool ScenesRemaining
		{
			get
			{
				lock(_locker)
					return _sceneStack.Count > 0;
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
					return (_sceneStack.Count <= 0) ? null : _curScene.Scene;
			}
		}

		/// <summary>
		/// State manager for the current scene.
		/// If there isn't a scene, then null is returned.
		/// </summary>
		internal StateManager StateManager
		{
			get
			{
				lock(_locker)
					return (_sceneStack.Count <= 0) ? null : _curScene.StateManager;
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
			var entry = new SceneStackEntry(scene);
#if DEBUG
			entry.StateManager.UpdateThreadId = _updateThreadId;
			entry.StateManager.RenderThreadId = _renderThreadId;
#endif

			lock(_locker)
			{
				_sceneStack.Push(entry);
				_curScene = entry;
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

				var prevEntry = _sceneStack.Pop();
				prevScene = prevEntry.Scene;
				prevScene.SetParentManager(null);
				_curScene = (_sceneStack.Count > 0) ? _sceneStack.Peek() : default(SceneStackEntry);
			}

			OnExitScene(new SceneEventArgs(prevScene));
		}
		#endregion

		#region Update and render

#if DEBUG
		private volatile int _updateThreadId;

		/// <summary>
		/// ID of the thread that is allowed to update
		/// </summary>
		internal int UpdateThreadId
		{
			set
			{
				foreach(var entry in _sceneStack)
					entry.StateManager.UpdateThreadId = value;
				_updateThreadId = value;
			}
		}
#endif

		/// <summary>
		/// Prepares for an update
		/// </summary>
		/// <param name="stepArgs">Step information</param>
		/// <remarks><paramref name="stepArgs"/> is populated with state index information.</remarks>
		internal void PreUpdate (FrameStepEventArgs stepArgs)
		{
			// Get the previous state and next state to update
			int prevStateIndex;
			var nextStateIndex = StateManager.AcquireNextUpdateState(out prevStateIndex);

			stepArgs.PreviousStateIndex = prevStateIndex;
			stepArgs.NextStateIndex     = nextStateIndex;
		}

		/// <summary>
		/// Updates the active scene
		/// </summary>
		/// <returns>True if there is still a scene to process</returns>
		/// <remarks>A return value of false indicates that the last scene has exited and the game should terminate.</remarks>
		internal bool Update (FrameStepEventArgs stepArgs)
		{
			_display.Update();
			CurrentScene.Step(stepArgs.PreviousStateIndex, stepArgs.NextStateIndex); // TODO: Pass stepArgs
			return ScenesRemaining;
		}

		/// <summary>
		/// Cleans up after an update
		/// </summary>
		/// <param name="stepArgs">Step information</param>
		internal void PostUpdate (FrameStepEventArgs stepArgs)
		{
			// Release the state
			StateManager.ReleaseUpdateState();
		}

#if DEBUG
		private volatile int _renderThreadId;

		/// <summary>
		/// ID of the thread that is allowed to render
		/// </summary>
		internal int RenderThreadId
		{
			set
			{
				foreach(var entry in _sceneStack)
					entry.StateManager.RenderThreadId = value;
				_renderThreadId = value;
			}
		}
#endif

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
			// Retrieve the next state to render
			int prevStateIndex;
			var nextStateIndex = StateManager.AcquireNextRenderState(out prevStateIndex);

			drawArgs.PreviousStateIndex = prevStateIndex;
			drawArgs.StateIndex         = nextStateIndex;

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
					CurrentScene.Draw(_display, drawArgs.StateIndex, drawArgs.Interpolation); // TODO: Pass drawArgs
					if(drawArgs.Duplicate)
						++RenderedDuplicateFrames;
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

			// Release the state
			StateManager.ReleaseRenderState();
		}
		#endregion

		/// <summary>
		/// An entry in the stack
		/// </summary>
		private struct SceneStackEntry
		{
			/// <summary>
			/// Scene to update and render
			/// </summary>
			public readonly Scene Scene;

			/// <summary>
			/// Manager that tracks the state to update and render
			/// </summary>
			public readonly StateManager StateManager;

			/// <summary>
			/// Creates a new stack entry
			/// </summary>
			/// <param name="scene">Scene to update and render</param>
			/// <exception cref="ArgumentNullException">The stored scene can't be null.</exception>
			public SceneStackEntry (Scene scene)
			{
				if(scene == null)
					throw new ArgumentNullException("scene");

				Scene        = scene;
				StateManager = new StateManager();
			}
		}
	}
}
