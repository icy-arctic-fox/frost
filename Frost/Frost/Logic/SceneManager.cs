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
		private readonly Stack<SceneStackEntry> _sceneStack = new Stack<SceneStackEntry>();
		private readonly IDisplay _display;

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

			var entry = new SceneStackEntry(scene, _display);
			_sceneStack.Push(entry);
		}

		/// <summary>
		/// Leaves the current scene and goes back to the previous one
		/// </summary>
		/// <remarks>The current executing scene will finish its <see cref="Update"/> or <see cref="Draw"/>.</remarks>
		public void ExitScene ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates the active scene
		/// </summary>
		public void Update ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the active scene
		/// </summary>
		public void Draw ()
		{
			throw new NotImplementedException();
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
			/// <param name="display">Display to draw the scene onto</param>
			public SceneStackEntry (Scene scene, IDisplay display)
			{
				if(scene == null)
					throw new ArgumentNullException("scene", "The scene to execute can't be null.");
				if(display == null)
					throw new ArgumentNullException("display", "The display to render to can't be null.");

				Scene   = scene;
				Manager = new StateManager(display, scene, scene);
			}
		}
	}
}
