using System;
using System.Diagnostics;
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
/*			if(updateRoot == null)
				throw new ArgumentNullException("updateRoot", "The game update root node can't be null.");
			if(renderRoot == null)
				throw new ArgumentNullException("renderRoot", "The game render root node can't be null.");*/
#endif
			_renderThread = new Thread(doRenderLoop);

			_display = display;
			_updateRoot = updateRoot;
			_renderRoot = renderRoot;
		}

		/// <summary>
		/// Indicates if the state manager is running.
		/// When false and executing, the update and render loops should exit.
		/// </summary>
		private bool _running;

		/// <summary>
		/// Starts the state manager.
		/// This call blocks until told to exit by the <see cref="Stop"/> method.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if the state manager has already stopped with <see cref="Stop"/>.
		/// The state manager cannot be restarted after it has been stopped.</exception>
		public void Run ()
		{
			if(!_renderThread.IsAlive)
			{// Thread is not running
				try
				{// ... but has it already exited?
					_running = true;
					_renderThread.Start();
				}
				catch(ThreadStateException e)
				{// Thread has already started and finished
					_running = false;
					throw new InvalidOperationException("The state manager has already exited - cannot restart.", e);
				}
				doUpdateLoop();
			}
		}

		/// <summary>
		/// Stops the state manager.
		/// This will cause the <see cref="Run"/> method to return.
		/// </summary>
		public void Stop ()
		{
			_running = false;
		}

		#region Update and render loops/logic

		#region Update

		/// <summary>
		/// Default length of time to target for each update frame
		/// </summary>
		public const double DefaultTargetUpdatePeriod = 1f / 60f;

		/// <summary>
		/// Target length of time (in seconds) for updates per frame
		/// </summary>
		private double _targetUpdatePeriod = DefaultTargetUpdatePeriod;

		/// <summary>
		/// Actual length of time (in seconds) that the game logic updates took during the last frame
		/// </summary>
		private double _prevUpdatePeriod;

		/// <summary>
		/// Length of time (in seconds) that passed during the last frame update
		/// </summary>
		private double _updatePeriod;
		
		/// <summary>
		/// Tracks the amount of time taken to complete game logic updates for a frame
		/// </summary>
		private readonly Stopwatch _updateTimer = new Stopwatch();

		/// <summary>
		/// Actual number of logic updates per second
		/// </summary>
		public double UpdateRate
		{
			get { return 1d / _updatePeriod; }
		}

		/// <summary>
		/// Target number of logic updates per second.
		/// A value of 0 represents no limit of updates per second.
		/// </summary>
		public double TargetUpdateRate
		{
			get { return (Math.Abs(_targetUpdatePeriod) < Double.Epsilon) ? 0d : 1d / _targetUpdatePeriod; }
			set { _targetUpdatePeriod = (Math.Abs(value) < Double.Epsilon) ? 0d : 1d / value; }
		}

		/// <summary>
		/// Length of time (in seconds) that the logic update took last frame
		/// </summary>
		public double UpdateTime
		{
			get { return _prevUpdatePeriod; }
		}

		/// <summary>
		/// Performs updates to the game state
		/// </summary>
		private void doUpdateLoop ()
		{
			_updateTimer.Start();
			while(_running)
			{// Continue performing game logic updates until told to stop
				// Update the game logic
				_display.Update();
//				_updateRoot.StepState(0, 1); // TODO: Use correct state indices
				((Window)_display).Title = UpdateRate + " u/s";

				// Measure how long it took to update
				var elapsed = _updateTimer.Elapsed;
				_prevUpdatePeriod = elapsed.TotalSeconds;

				// Calculate how long to sleep for the remaining time (if there's any) in the frame
				var sleepTime = (int)((_targetUpdatePeriod - _prevUpdatePeriod) * 1000);
				if(sleepTime < 0) // Don't sleep for a negative time
					sleepTime = 0;

				// The sleep time can be zero, which will yield the update thread.
				// This is important for single-core processors and allowing the render thread to work.
				Thread.Sleep(sleepTime); // TODO: Reduce how much time is spent sleeping to avoid over sleeping

				// Save how long it took for reporting and restart the timer
				_updatePeriod = _updateTimer.Elapsed.TotalSeconds;
				_updateTimer.Restart();
			}
		}
		#endregion

		/// <summary>
		/// Threaded method that runs the render loop
		/// </summary>
		private void doRenderLoop ()
		{
			while(_running)
			{
				_display.EnterFrame();
//				_renderRoot.DrawState(0); // TODO: Use correct index
				_display.ExitFrame();
				Thread.Sleep(1000 / 60);
			}
			throw new NotImplementedException();
		}
		#endregion
	}
}
