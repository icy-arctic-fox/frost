using System;
using System.Threading;
using Frost.Display;

namespace Frost.Modules
{
	/// <summary>
	/// Tracks the frames to be updated and rendered
	/// </summary>
	public class StateManager
	{
		/// <summary>
		/// Thread that runs the render loop
		/// </summary>
		private readonly Thread _renderThread;

		private readonly IDisplay _display;
		private readonly IStepableNode _updateRoot;
		private readonly IDrawableNode _renderRoot;

		/// <summary>
		/// Creates a new state manager with the update and render root nodes
		/// </summary>
		/// <param name="display">Display that shows frames on the screen</param>
		/// <param name="updateRoot">Root node for updating the game state</param>
		/// <param name="renderRoot">Root node for rendering the game state</param>
		/// <remarks>Generally, <paramref name="updateRoot"/> and <paramref name="renderRoot"/> are the same object.
		/// They can be different for more complex situations.</remarks>
		public StateManager (IDisplay display, IStepableNode updateRoot, IDrawableNode renderRoot)
		{
#if DEBUG
			if(display == null)
				throw new ArgumentNullException("display", "The display that renders frames can't be null.");
			if(updateRoot == null)
				throw new ArgumentNullException("updateRoot", "The game update root node can't be null.");
			if(renderRoot == null)
				throw new ArgumentNullException("renderRoot", "The game render root node can't be null.");
#endif
			_renderThread = new Thread(doRenderThread);

			_display = display;
			_updateRoot = updateRoot;
			_renderRoot = renderRoot;
		}

		/// <summary>
		/// Starts the state manager.
		/// This call blocks until told to exit by the <see cref="Stop"/> method.
		/// </summary>
		public void Run ()
		{
			_renderThread.Start();
			throw new NotImplementedException();
		}

		/// <summary>
		/// Stops the state manager.
		/// This will cause the <see cref="Run"/> method to return.
		/// </summary>
		public void Stop ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Threaded method that runs the render loop
		/// </summary>
		private void doRenderThread ()
		{
			// TODO: Loop
			_display.EnterFrame();
			_renderRoot.DrawState(0); // TODO: Add state number
			_display.ExitFrame();
			throw new NotImplementedException();
		}
	}
}
