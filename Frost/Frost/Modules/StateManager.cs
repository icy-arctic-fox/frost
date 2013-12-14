using System;
using System.Diagnostics;
using System.Threading;
using Frost.Display;

namespace Frost.Modules
{
	/// <summary>
	/// Tracks the frames to be updated and rendered.
	/// The state manager uses multiple states in memory so that updates and rendering can take place on separate threads concurrently.
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
		private volatile bool _running;

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

		/// <summary>
		/// Current frame number - this is the update frame number
		/// </summary>
		private ulong _frame = 0;

		/// <summary>
		/// Number of frames rendered
		/// </summary>
		private ulong _framesRendered = 0;

		/// <summary>
		/// Number of times a frame has been rendered more than once
		/// </summary>
		private ulong _duplicateFrames = 0;

		/// <summary>
		/// Current frame number - this is the update frame number
		/// </summary>
		public ulong Frame
		{
			get { return _frame; }
		}

		/// <summary>
		/// Number of frames rendered
		/// </summary>
		public ulong FramesRendered
		{
			get { return _framesRendered; }
		}

		/// <summary>
		/// Number of times a frame has been rendered more than once
		/// </summary>
		public ulong DuplicateFrames
		{
			get { return _duplicateFrames; }
		}

		private readonly object _locker = new object();

		/// <summary>
		/// Index of the previous state updated
		/// </summary>
		private int _updateIndex = -1;

		/// <summary>
		/// Index of the previous state rendered
		/// </summary>
		private int _renderIndex = -1;

		/// <summary>
		/// Last frame number that was updated
		/// </summary>
		private ulong _updateFrame = 0;

		/// <summary>
		/// Last frame number that was rendered
		/// </summary>
		private ulong _renderFrame = 0;

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
		/// Current number of logic updates per second
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
				// Find out which frame state object that should be updating and referencing
				int prevState, nextState;
				lock(_locker)
				{
					prevState = _updateIndex;
					switch(_updateIndex)
					{
					case 0:
						nextState = _renderIndex == 1 ? 2 : 1;
						break;
					case 1:
						nextState = _renderIndex == 0 ? 2 : 0;
						break;
					default: // 2 or -1
						nextState = _renderIndex == 0 ? 1 : 0;
						break;
					}
				}

				// Update the game logic
				_display.Update();
//				_updateRoot.StepState(prevState, nextState); // TODO: Use correct state indices
				((Window)_display).Title = UpdateRate + " u/s " + RenderRate + " fps";

				// Store information about the frame we just updated
				lock(_locker)
				{
					_updateIndex = nextState;
					++_updateFrame;
				}

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

		#region Render

		/// <summary>
		/// Default length of time to target for rendering a frame
		/// </summary>
		public const double DefaultTargetRenderPeriod = 1f / 30f;

		/// <summary>
		/// Target length of time (in seconds) for renders per frame
		/// </summary>
		private double _targetRenderPeriod = DefaultTargetRenderPeriod;

		/// <summary>
		/// Actual length of time (in seconds) that rendering took during the last frame
		/// </summary>
		private double _prevRenderPeriod;

		/// <summary>
		/// Length of time (in seconds) that passed during the last frame rendering
		/// </summary>
		private double _renderPeriod;

		/// <summary>
		/// Tracks the amount of time taken to render a frame
		/// </summary>
		private readonly Stopwatch _renderTimer = new Stopwatch();

		/// <summary>
		/// Current number of rendered frames per second
		/// </summary>
		public double RenderRate
		{
			get { return 1d / _renderPeriod; }
		}

		/// <summary>
		/// Target number of rendered frames per second.
		/// A value of 0 represents no limit of renders per second.
		/// </summary>
		public double TargetRenderRate
		{
			get { return (Math.Abs(_targetRenderPeriod) < Double.Epsilon) ? 0d : 1d / _targetRenderPeriod; }
			set { _targetRenderPeriod = (Math.Abs(value) < Double.Epsilon) ? 0d : 1d / value; }
		}

		/// <summary>
		/// Length of time (in seconds) that it took to render the last frame
		/// </summary>
		public double RenderTime
		{
			get { return _prevRenderPeriod; }
		}

		/// <summary>
		/// Threaded method that runs the render loop
		/// </summary>
		private void doRenderLoop ()
		{
			_renderTimer.Start();
			while(_running)
			{// Continue rendering until told to stop
				// Find out which frame we should be updating and referencing
				int state;
				lock(_locker)
				{
					state = _renderIndex;
					switch(_renderIndex)
					{
					case 0:
						state = _updateIndex == 1 ? 2 : 1;
						break;
					case 1:
						state = _updateIndex == 0 ? 2 : 0;
						break;
					default: // 2 or -1
						state = _updateIndex == 0 ? 1 : 0;
						break;
					}
				}

				// Render the frame
				_display.EnterFrame();
//				_renderRoot.DrawState(state); // TODO: Use correct index
				_display.ExitFrame();

				// Measure how long it took to render
				var elapsed = _renderTimer.Elapsed;
				_prevRenderPeriod = elapsed.TotalSeconds;

				// Calculate how long to sleep for the remaining time (if there's any) in the frame
				var sleepTime = (int)((_targetRenderPeriod - _prevRenderPeriod) * 1000);
				if(sleepTime < 0) // Don't sleep for a negative time
					sleepTime = 0;

				// The sleep time can be zero, which will yield the render thread.
				// This is important for single-core processors and allowing the update thread to continue.
				Thread.Sleep(sleepTime); // TODO: Reduce how much time is spent sleeping to avoid over sleeping

				// Save how long it took for reporting and restart the timer
				_renderPeriod = _renderTimer.Elapsed.TotalSeconds;
				_renderTimer.Restart();
			}
		}
		#endregion
		#endregion
	}
}
