using System;
using System.Collections.Generic;
using Frost.Display;

namespace Frost.Logic
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
		/// Scene currently being processed
		/// </summary>
		public Scene CurrentScene
		{
			get
			{
				lock(_locker)
					return _curScene.Scene;
			}
		}

		/// <summary>
		/// State manager for the current scene
		/// </summary>
		internal StateManager StateManager
		{
			get
			{
				lock(_locker)
					return _curScene.Manager;
			}
		}

		/// <summary>
		/// Creates a new scene manager
		/// </summary>
		/// <param name="initialScene">Initial scene</param>
		/// <param name="display">Display to render scenes to</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="initialScene"/> or <paramref name="display"/> are null.</exception>
		public SceneManager (Scene initialScene, IDisplay display)
		{
			if(initialScene == null)
				throw new ArgumentNullException("initialScene", "The initial scene can't be null.");
			if(display == null)
				throw new ArgumentNullException("display", "The display to render to can't be null.");

			_display = display;
			EnterScene(initialScene);
		}

		/// <summary>
		/// Enters a new scene
		/// </summary>
		/// <param name="scene">Scene to enter</param>
		public void EnterScene (Scene scene)
		{
			if(scene == null)
				throw new ArgumentNullException("scene", "The scene to enter can't be null.");

			var entry = new SceneStackEntry(scene);
#if DEBUG
			entry.Manager.UpdateThreadId = UpdateThreadId;
			entry.Manager.RenderThreadId = RenderThreadId;
#endif

			lock(_locker)
			{
				_sceneStack.Push(entry);
				_curScene = entry;
			}
		}

		/// <summary>
		/// Leaves the current scene and goes back to the previous one
		/// </summary>
		/// <remarks>The current executing scene will finish its <see cref="Update"/> or <see cref="Render"/>.</remarks>
		public void ExitScene ()
		{
			throw new NotImplementedException();
		}

#if DEBUG
		/// <summary>
		/// ID of the thread that is allowed to update
		/// </summary>
		internal int UpdateThreadId { private get; set; }
#endif

		/// <summary>
		/// Updates the active scene
		/// </summary>
		public void Update ()
		{
			// Get the previous state and next state to update
			int prevStateIndex;
			var nextStateIndex = StateManager.AcquireNextUpdateState(out prevStateIndex);

			// Perform the update
			_display.Update();
			CurrentScene.Step(prevStateIndex, nextStateIndex);
			((Window)_display).Title = String.Join(" - ", ToString(), StateManager); // TODO: Remove this

			// Release the state
			StateManager.ReleaseUpdateState();
		}

#if DEBUG
		/// <summary>
		/// ID of the thread that is allowed to render
		/// </summary>
		internal int RenderThreadId { private get; set; }
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
		/// Renders the active scene
		/// </summary>
		public void Render ()
		{
			// Retrieve the next state to render
			int prevStateIndex;
			var nextStateIndex = StateManager.AcquireNextRenderState(out prevStateIndex);

			if(RenderDuplicateFrames || prevStateIndex != nextStateIndex)
			{// Render the frame
				_display.EnterFrame();
				CurrentScene.Draw(_display, nextStateIndex);
				_display.ExitFrame();

				if(prevStateIndex == nextStateIndex)
					++RenderedDuplicateFrames;
			}

			// Release the state
			StateManager.ReleaseRenderState();
		}

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
			public readonly StateManager Manager;

			/// <summary>
			/// Creates a new stack entry
			/// </summary>
			/// <param name="scene">Scene to update and render</param>
			public SceneStackEntry (Scene scene)
			{
				if(scene == null)
					throw new ArgumentNullException("scene", "The scene to execute can't be null.");

				Scene   = scene;
				Manager = new StateManager();
			}
		}
	}
}
