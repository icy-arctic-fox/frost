using System;
using System.Threading;
using Frost.Display;
using Frost.Modules.State;
using Frost.Utility;

namespace Frost.Modules
{
	/// <summary>
	/// Tracks the frames to be updated and rendered.
	/// The state manager uses multiple states in memory so that updates and rendering can take place on separate threads concurrently.
	/// </summary>
	public class StateManager
	{
		// TODO: Add ability to save older states for roll-back

		/// <summary>
		/// Number of active states in memory.
		/// Each component that can be modified during runtime needs to have this many states.
		/// </summary>
		public const int StateCount = 3;

		/// <summary>
		/// Thread that runs the render loop
		/// </summary>
		private readonly Thread _renderThread;

		/// <summary>
		/// Display that will be rendered upon
		/// </summary>
		private readonly IDisplay _display;

		/// <summary>
		/// Root node for updating the game state
		/// </summary>
		private readonly IStepableNode _updateRoot;

		/// <summary>
		/// Root node for drawing the game state
		/// </summary>
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
			: this(display, updateRoot, renderRoot, DefaultTargetUpdateRate, DefaultTargetRenderRate)
		{
			// ...
		}

		/// <summary>
		/// Creates a new state manager with the update and render root nodes
		/// </summary>
		/// <param name="display">Display that shows frames on the screen</param>
		/// <param name="updateRoot">Root node for updating the game state</param>
		/// <param name="renderRoot">Root node for rendering the game state</param>
		/// <param name="updateRate">Targeted number of updates per second - use 0 to represent no limit</param>
		/// <param name="renderRate">Targeted number of rendered frames per second - use 0 to represent no limit</param>
		/// <remarks>Generally, <paramref name="updateRoot"/> and <paramref name="renderRoot"/> are the same object.
		/// They can be different for more complex situations.</remarks>
		public StateManager (IDisplay display, IStepableNode updateRoot, IDrawableNode renderRoot, double updateRate, double renderRate)
		{
#if DEBUG
			if(display == null)
				throw new ArgumentNullException("display", "The display that renders frames can't be null.");
			if(updateRoot == null)
				throw new ArgumentNullException("updateRoot", "The game update root node can't be null.");
			if(renderRoot == null)
				throw new ArgumentNullException("renderRoot", "The game render root node can't be null.");
#endif
			_renderThread = new Thread(doRenderLoop) {
				Name     = "State Manager Render Thread",
				Priority = ThreadPriority.AboveNormal
			};

			_display = display;
			_updateRoot = updateRoot;
			_renderRoot = renderRoot;

			_targetUpdateInterval = (Math.Abs(updateRate) < Double.Epsilon) ? 0d : 1d / updateRate;
			_targetRenderInterval = (Math.Abs(renderRate) < Double.Epsilon) ? 0d : 1d / renderRate;
		}

		/// <summary>
		/// Indicates if the state manager is running.
		/// When false and executing, the update and render loops should exit.
		/// </summary>
		private volatile bool _running;

		/// <summary>
		/// Indicates if the state manager has started and is currently operating
		/// </summary>
		public bool Running
		{
			get { return _running; }
		}

#if DEBUG
		private int _updateThreadId = -1, _renderThreadId = -1;
#endif

		/// <summary>
		/// Starts the state manager.
		/// This call blocks until told to exit by the <see cref="Stop"/> method.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if the state manager has already stopped with <see cref="Stop"/>.
		/// The state manager cannot be restarted after it has been stopped.</exception>
		public void Run ()
		{
			// Disable rendering on the current thread so that the render thread can do it
			_display.SetActive(false);

			if(!_renderThread.IsAlive)
			{// Thread is not running
				try
				{// ... but has it already exited?
					_running = true;
					// TODO: Set reset event
					_renderThread.Start();
				}
				catch(ThreadStateException e)
				{// Thread has already started and finished
					_running = false;
					throw new InvalidOperationException("The state manager has already exited - cannot restart.", e);
				}
#if DEBUG
				_updateThreadId = Thread.CurrentThread.ManagedThreadId;
#endif
				doUpdateLoop();
			}
		}

		/// <summary>
		/// Stops the state manager.
		/// This will cause the <see cref="Run"/> method to return.
		/// </summary>
		/// <remarks>This thread will block until the render thread exits.</remarks>
		public void Stop ()
		{
			_running = false;
			_renderThread.Join();
		}

		#region Update and render loops/logic

		/// <summary>
		/// Frame number that each of the states are on
		/// </summary>
		private readonly long[] _stateFrameNumbers = new[] {0L, 0L, 0L};

		/// <summary>
		/// Total number of duplicated frames encountered.
		/// This is not the number of duplicate frames rendered, see <seealso cref="RenderedDuplicateFrames"/> for that.
		/// An increasing value indicates that the update thread is running slower than the render thread.
		/// </summary>
		public long DuplicatedFrames { get; private set; }

		/// <summary>
		/// Total number of duplicates frames that have been rendered.
		/// An increasing value indicates that the update thread is running slower than the render thread.
		/// </summary>
		public long RenderedDuplicateFrames { get; private set; }

		/// <summary>
		/// Total number of frames that were skipped.
		/// This represents the frames that were updated, but were not rendered.
		/// An increasing value indicates that the render thread is running slower than the update thread.
		/// </summary>
		public long SkippedFrames { get; private set; }

		/// <summary>
		/// Indicates whether threads are synchronized in an attempt to reduce stuttering
		/// </summary>
		/// <remarks>This option is only effective when the update and render rates should be the same.</remarks>
		public bool SynchronizeThreads { get; set; }

		/// <summary>
		/// Indicates whether a frame can be rendered more than once
		/// </summary>
		public bool RenderDuplicateFrames { get; set; }

		/// <summary>
		/// Maximum number of measurements to take for averaging update and render intervals
		/// </summary>
		private const int MeasurementCount = 10;

		private readonly AverageCounter _updateCounter = new AverageCounter(MeasurementCount),
			_renderCounter = new AverageCounter(MeasurementCount);

		/// <summary>
		/// Maximum proportion of time allowed to pass before frame skipping or duplication takes effect
		/// </summary>
		private const double MaxFrameDrift = 1.15;

		/// <summary>
		/// Indicates whether frames are currently being skipped to help the render thread catch up
		/// </summary>
		public bool FrameSkipping
		{
			get
			{
				if(/* TODO: Disposed || */!SynchronizeThreads || UnboundedUpdateRate || _renderCounter.Count == 0)
					return true;
				return _targetUpdateInterval > _renderCounter.Average * MaxFrameDrift;
			}
		}

		/// <summary>
		/// Indicates whether frames are currently being duplicated to help the update thread catch up
		/// </summary>
		public bool FrameDuplication
		{
			get
			{
				if(/* TODO: Disposed || */!SynchronizeThreads || UnboundedRenderRate || _updateCounter.Count == 0)
					return true;
				return _targetRenderInterval > _updateCounter.Average * MaxFrameDrift;
			}
		}

		#region Update

		/// <summary>
		/// Default targeted number of logic updates to perform per second
		/// </summary>
		public const double DefaultTargetUpdateRate = 60d;

		/// <summary>
		/// Default length of time (in seconds) to target for each update frame
		/// </summary>
		public const double DefaultTargetUpdateInterval = 1d / DefaultTargetUpdateRate;

		/// <summary>
		/// Target length of time (in seconds) for updates per frame
		/// </summary>
		private double _targetUpdateInterval = DefaultTargetUpdateInterval;

		/// <summary>
		/// Length of time (in seconds) that it took to just update the previous frame
		/// </summary>
		public double UpdateInterval { get; private set; }

		/// <summary>
		/// Actual length of time (in seconds) taken to update and sleep
		/// </summary>
		private double _actualUpdateInterval;

		/// <summary>
		/// Current number of logic updates per second
		/// </summary>
		public double UpdateRate
		{
			get { return (Math.Abs(_actualUpdateInterval) < Double.Epsilon) ? Double.PositiveInfinity : 1d / _actualUpdateInterval; }
		}

		/// <summary>
		/// Running average of the updated frames per second
		/// </summary>
		public double AverageUpdateRate
		{
			get
			{
				var avg = _updateCounter.Average;
				return (avg < Double.Epsilon) ? 0d : 1d / avg;
			}
		}

		/// <summary>
		/// Target number of logic updates per second.
		/// A value of 0 represents no limit of frames per second.
		/// </summary>
		public double TargetUpdateRate
		{
			get { return (Math.Abs(_targetUpdateInterval) < Double.Epsilon) ? 0d : 1d / _targetUpdateInterval; }
			set { _targetUpdateInterval = (Math.Abs(value) < Double.Epsilon) ? 0d : 1d / value; }
		}

		/// <summary>
		/// Indicates whether the number of logic updates per second is unbounded
		/// </summary>
		public bool UnboundedUpdateRate
		{
			get { return Math.Abs(_targetUpdateInterval) < Double.Epsilon; }
		}

		/// <summary>
		/// Index of the current state being updated.
		/// -1 means no state is currently being updated.
		/// </summary>
		private int _curUpdateStateIndex = -1;

		/// <summary>
		/// Index of the previous state that was updated
		/// </summary>
		private int _prevUpdateStateIndex;

		/// <summary>
		/// Frame number of the previous state that was updated
		/// </summary>
		private long _prevUpdateFrame;

		/// <summary>
		/// Current frame number
		/// </summary>
		public long FrameNumber
		{
			get { return _prevUpdateFrame; }
		}

		/// <summary>
		/// Reset event that indicates whether the update thread has generated a frame
		/// </summary>
		private readonly ManualResetEventSlim _updateSignal = new ManualResetEventSlim();

		/// <summary>
		/// Marks the next state as being rendered and retrieves the index
		/// </summary>
		/// <returns>A state index</returns>
		private int acquireNextUpdateState ()
		{
			lock(_stateFrameNumbers)
			{
#if DEBUG
				if(Thread.CurrentThread.ManagedThreadId != _updateThreadId)
					throw new AccessViolationException("Only the update thread may acquire an update state.");
				if(_curUpdateStateIndex != -1)
					throw new InvalidOperationException("Cannot acquire the next state to update until the previous state has been released.");
#endif
				if(_curUpdateStateIndex == -1 || _prevUpdateStateIndex == _curRenderStateIndex)
				{
					switch(_prevUpdateStateIndex)
					{
					case 0:
						_curUpdateStateIndex = (_stateFrameNumbers[1] < _stateFrameNumbers[2]) ? 1 : 2;
						break;
					case 1:
						_curUpdateStateIndex = (_stateFrameNumbers[0] < _stateFrameNumbers[2]) ? 0 : 2;
						break;
					case 2:
						_curUpdateStateIndex = (_stateFrameNumbers[0] < _stateFrameNumbers[1]) ? 0 : 1;
						break;
					default:
						throw new InvalidOperationException("The previously updated state index should be 0, 1, or 2.");
					}
				}
				else
					_curUpdateStateIndex = StateCount - (_prevUpdateStateIndex + _curRenderStateIndex);
				return _curUpdateStateIndex;
			}
		}

		/// <summary>
		/// Releases a state from updating so that it can be rendered
		/// </summary>
		/// <param name="frame">Number of the frame that was just updated</param>
		private void releaseUpdateState (long frame)
		{
			lock(_stateFrameNumbers)
			{
#if DEBUG
				if(Thread.CurrentThread.ManagedThreadId != _updateThreadId)
					throw new AccessViolationException("Only the update thread may release an update state.");
				if(_curUpdateStateIndex == -1)
					throw new InvalidOperationException("Cannot release an update state when no state is currently being updated.");
#endif
				_prevUpdateStateIndex = _curUpdateStateIndex;
				_prevUpdateFrame      = _stateFrameNumbers[_prevUpdateStateIndex] = frame;
				_curUpdateStateIndex  = -1;
				_updateSignal.Set();
			}
		}

		/// <summary>
		/// Waits for the render thread to start drawing the frame that was just updated
		/// </summary>
		/// <param name="timeout">Amount of time to wait for</param>
		/// <returns>True if the render thread finished before the timeout elapsed or false if it didn't</returns>
		private bool waitForRender (TimeSpan timeout)
		{
			lock(_stateFrameNumbers)
			{
				if(/* TODO: Disposed || */
					(_curRenderStateIndex == -1 && _prevRenderStateIndex == _prevUpdateStateIndex) || // Render thread just finished
					_curRenderStateIndex == _prevUpdateStateIndex) // Render thread just started it
					return true;
				_renderSignal.Reset();
			}
			return _renderSignal.Wait(timeout);
		}

		/// <summary>
		/// Performs updates to the game state
		/// </summary>
		private void doUpdateLoop ()
		{
			var timeout = TimeSpan.FromSeconds(1); // 1 second timeout waiting for render thread

			// Set up for the initial frame
			var frameCount    = 0;  // Number of updated frames kept in the target interval (used to get an average sleep time)
			var frameNumber   = 0L; // Logical frame number
			var prevStartTime = DateTime.Now;  // Time that the previous update started
			var curStartTime  = prevStartTime; // Time that the update started

			while(_running)
			{
				// Update the next state
				var state = acquireNextUpdateState();
				_display.Update();
				_updateRoot.StepState(_prevUpdateStateIndex, state);
				((Window)_display).Title = ToString() + " - " + StateString; // TODO: Remove this
				releaseUpdateState(frameNumber++);

				// Calculate the amount of time to sleep
				var now           = DateTime.Now;
				UpdateInterval    = (now - curStartTime).TotalSeconds;
				var nextStartTime = prevStartTime.AddSeconds(_targetUpdateInterval * (++frameCount));

				if(nextStartTime < now)
				{// Took too long to update the frame
					frameCount    = 0;
					prevStartTime = now;
					Thread.Sleep(0); // Yield to other threads briefly
				}
				else
				{// Sleep for the remaining slot of time
					var sleepTime = (int)Math.Ceiling((nextStartTime - now).TotalMilliseconds);
					Thread.Sleep(sleepTime);
				}

				if(!FrameSkipping) // Wait for the render thread to process
					while(!waitForRender(timeout)) // Use a timeout to prevent lock-up which would not allow the update loop to exit
						if(!_running) // Exit if not running anymore
							return;

				// Update time measurements
				now = DateTime.Now;
				_actualUpdateInterval = (now - curStartTime).TotalSeconds;
				curStartTime = now;
				_updateCounter.AddMeasurement(_actualUpdateInterval);
			}
		}
		#endregion

		#region Render

		/// <summary>
		/// Default targeted number of frames to draw per second
		/// </summary>
		public const double DefaultTargetRenderRate = 60d;

		/// <summary>
		/// Default length of time (in seconds) to target for each frame rendering
		/// </summary>
		public const double DefaultTargetRenderInterval = 1d / DefaultTargetRenderRate;

		/// <summary>
		/// Target length of time (in seconds) for rendering per frame
		/// </summary>
		private double _targetRenderInterval = DefaultTargetRenderInterval;

		/// <summary>
		/// Length of time (in seconds) that it took to just render the last frame
		/// </summary>
		public double RenderInterval { get; private set; }

		/// <summary>
		/// Actual time that it took (in seconds) to render and sleep the last frame
		/// </summary>
		private double _actualRenderInterval;

		/// <summary>
		/// Current number of frames rendered per second
		/// </summary>
		public double RenderRate
		{
			get { return (Math.Abs(_actualRenderInterval) < Double.Epsilon) ? 0d : 1d / _actualRenderInterval; }
		}

		/// <summary>
		/// Running average of the rendered frames per second
		/// </summary>
		public double AverageRenderRate
		{
			get
			{
				var avg = _renderCounter.Average;
				return (avg < Double.Epsilon) ? 0d : 1d / avg;
			}
		}

		/// <summary>
		/// Target number of rendered frames per second.
		/// A value of 0 represents no limit of frames per second.
		/// </summary>
		public double TargetRenderRate
		{
			get { return (Math.Abs(_targetRenderInterval) < Double.Epsilon) ? 0d : 1d / _targetRenderInterval; }
			set { _targetRenderInterval = (Math.Abs(value) < Double.Epsilon) ? 0d : 1d / value; }
		}

		/// <summary>
		/// Indicates whether the number of frames rendered per second is unbounded
		/// </summary>
		public bool UnboundedRenderRate
		{
			get { return Math.Abs(_targetRenderInterval) < Double.Epsilon; }
		}

		/// <summary>
		/// Index of the current state being drawn
		/// </summary>
		private int _curRenderStateIndex = -1;

		/// <summary>
		/// Index of the previous state that was drawn
		/// </summary>
		private int _prevRenderStateIndex = -1;

		/// <summary>
		/// Frame number of the previous state that was drawn
		/// </summary>
		private long _prevRenderFrame;

		/// <summary>
		/// Reset event that indicates whether the render thread is busy rendering a state
		/// </summary>
		private readonly ManualResetEventSlim _renderSignal = new ManualResetEventSlim();

		/// <summary>
		/// Marks the next state as being rendered and retrieves the index
		/// </summary>
		/// <returns>A state index</returns>
		private int acquireNextRenderState ()
		{
			lock(_stateFrameNumbers)
			{
#if DEBUG
				if(Thread.CurrentThread.ManagedThreadId != _renderThreadId)
					throw new AccessViolationException("Only the render thread may acquire a render state.");
				if(_curRenderStateIndex != -1)
					throw new InvalidOperationException("Cannot acquire the next state to draw until the previous state has been released.");
#endif
				_curRenderStateIndex = _prevUpdateStateIndex;
				var frame = _stateFrameNumbers[_curRenderStateIndex];
				if(_prevRenderStateIndex != -1)
				{
					if(frame == _prevRenderFrame)
						++DuplicatedFrames;
					else
						SkippedFrames += frame - _prevRenderFrame - 1;
				}
				else // First frame being drawn
					SkippedFrames += frame;
				_renderSignal.Set();
				return _curRenderStateIndex;
			}
		}

		/// <summary>
		/// Releases the state currently be rendered
		/// </summary>
		private void releaseRenderState()
		{
			lock(_stateFrameNumbers)
			{
#if DEBUG
				if(Thread.CurrentThread.ManagedThreadId != _renderThreadId)
					throw new AccessViolationException("Only the render thread may release a render state.");
				if(_curRenderStateIndex == -1)
					throw new InvalidOperationException("Cannot release a render state when no state is currently being drawn.");
#endif
				_prevRenderStateIndex = _curRenderStateIndex;
				_prevRenderFrame = _stateFrameNumbers[_prevRenderStateIndex];
				_curRenderStateIndex = -1;
			}
		}

		/// <summary>
		/// Waits for the update thread to produce the next frame to draw
		/// </summary>
		/// <param name="timeout">Maximum time to wait</param>
		/// <returns>True if a frame was updated before the timeout elapsed</returns>
		private bool waitForUpdate (TimeSpan timeout)
		{
			lock(_stateFrameNumbers)
			{
				if(_prevRenderFrame < _prevUpdateFrame)
					return true; // There's already a frame waiting
				_updateSignal.Reset();
			}
			return _updateSignal.Wait(timeout);
		}

		/// <summary>
		/// Threaded method that runs the render loop
		/// </summary>
		/// <exception cref="AccessViolationException">Thrown if the display to render to could not be enabled for the current thread.
		/// Make sure the display is disabled on all other threads.</exception>
		private void doRenderLoop ()
		{
			_renderThreadId = Thread.CurrentThread.ManagedThreadId;
			if(!_display.SetActive())
				throw new AccessViolationException("Could not activate rendering to the display on the state manager's render thread. It may be active on another thread.");

			// Variable initialization for timing
			var frameCount    = 0; // Number of rendered frames kept in the target interval (used to get an average sleep time)
			var prevStartTime = DateTime.Now;  // Time that the previous update started
			var curStartTime  = prevStartTime; // Time that the update started
			var timeout = TimeSpan.FromSeconds(1);

			// TODO: while(!Disposed)
			// {

			// Wait for the update thread to produce the first frame
			while(!waitForUpdate(timeout))
				if(!_running)
					return;

			var prevState = -1;
			while(_running)
			{
				if(!FrameDuplication) // Wait for a frame
					while(!waitForUpdate(timeout)) // Use a timeout to allow the render loop to exit if the render thread should stop
						if(!_running) // Exit if not running anymore
							return;

				// Get the next state and draw it
				var state = acquireNextRenderState();
				if(RenderDuplicateFrames || prevState != state)
				{// Render if dups are enabled or it's a new frame
					_display.EnterFrame();
					_renderRoot.DrawState(_display, state);
					_display.ExitFrame();
					if(prevState == state) // Just rendered a duplicate frame
						++RenderedDuplicateFrames;
				}
				prevState = state;
				releaseRenderState();

				// Calculate the amount of time to sleep
				var now           = DateTime.Now;
				RenderInterval    = (now - curStartTime).TotalSeconds;
				var nextStartTime = prevStartTime.AddSeconds(_targetRenderInterval * (++frameCount));

				if(nextStartTime < now)
				{// Took too long to render the frame
					frameCount    = 0;
					prevStartTime = now;
					Thread.Sleep(0); // Yield to other threads briefly
				}
				else
				{// Sleep for the remaining slot of time
					var sleepTime = (int)Math.Ceiling((nextStartTime - now).TotalMilliseconds);
					Thread.Sleep(sleepTime);
				}

				// Update time measurements
				now = DateTime.Now;
				_actualRenderInterval = (now - curStartTime).TotalSeconds;
				curStartTime = now;
				_renderCounter.AddMeasurement(_actualRenderInterval);
			}
			// }
		}
		#endregion
		#endregion

		/// <summary>
		/// Generates a string that represents the status of the state manager
		/// </summary>
		/// <returns>A string in the form:
		/// Frame: # - # u/s - # f/s (#/# dups, # skipped)</returns>
		public override string ToString ()
		{
			var sb = new System.Text.StringBuilder();
			sb.Append("Frame: ");
			sb.Append(FrameNumber);
			sb.Append(" - ");
			sb.Append(String.Format("{0:0.00}", UpdateRate));
			sb.Append(" u/s - ");
			sb.Append(String.Format("{0:0.00}", RenderRate));
			sb.Append(" f/s (");
			sb.Append(RenderedDuplicateFrames);
			sb.Append('/');
			sb.Append(DuplicatedFrames);
			sb.Append(" dups, ");
			sb.Append(SkippedFrames);
			sb.Append(" skipped)");
			return sb.ToString();
		}

		/// <summary>
		/// String that contains the frame states and the operations being performed on them.
		/// &lt; &gt; signify that the state is being rendered.
		/// [ ] signify that the state is being updated.
		/// </summary>
		public string StateString
		{
			get
			{
				var frames = new long[3];
				int updateIndex, renderIndex;
				lock(_stateFrameNumbers)
				{// Grab the values
					for(var i = 0; i < 3; ++i)
						frames[i] = _stateFrameNumbers[i];
					updateIndex = _curUpdateStateIndex;
					renderIndex = _curRenderStateIndex;
				}

				var sb = new System.Text.StringBuilder();
				for(var i = 0; i < frames.Length; ++i)
				{
					if(updateIndex == i)
					{
						sb.Append('[');
						sb.Append(frames[i]);
						sb.Append(']');
					}
					else if(renderIndex == i)
					{
						sb.Append('<');
						sb.Append(frames[i]);
						sb.Append('>');
					}
					else
					{
						sb.Append(' ');
						sb.Append(frames[i]);
						sb.Append(' ');
					}
					if(i - 1 < frames.Length)
						sb.Append(' ');
				}
				return sb.ToString();
			}
		}
	}
}
