using System;
using System.Diagnostics;
using System.Threading;
using Frost.Display;
using Frost.Graphics;
using Frost.Utility;

namespace Frost
{
	/// <summary>
	/// Tracks the frames to be updated and rendered.
	/// The state manager uses multiple states in memory so that updates and rendering can take place on separate threads concurrently.
	/// </summary>
	public class StateManager : IDisposable
	{
		// TODO: Add ability to save older states for roll-back

		/// <summary>
		/// Number of active states in memory.
		/// Each component that can be modified during runtime needs to have this many states.
		/// </summary>
		public const int StateCount = 3;

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
		/// <param name="rate">Targeted number of frames to update and render per second - use 0 to represent no limit</param>
		/// <remarks>Generally, <paramref name="updateRoot"/> and <paramref name="renderRoot"/> are the same object.
		/// They can be different for more complex situations.</remarks>
		public StateManager (IDisplay display, IStepableNode updateRoot, IDrawableNode renderRoot, double rate)
			: this(display, updateRoot, renderRoot, rate, rate)
		{
			// ...
		}

		/// <summary>
		/// Creates a new state manager with the update and render root nodes
		/// </summary>
		/// <param name="display">Display that shows frames on the screen</param>
		/// <param name="updateRoot">Root node for updating the game state</param>
		/// <param name="renderRoot">Root node for rendering the game state</param>
		/// <param name="updateRate">Targeted number of logical game updates second - use 0 to represent no limit</param>
		/// <param name="renderRate">Targeted number of frames to render per second - use 0 to represent no limit</param>
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

			_display = display;
			_updateRoot = updateRoot;
			_renderRoot = renderRoot;

			_targetUpdateInterval = (updateRate < Double.Epsilon) ? 0d : 1d / updateRate;
			_targetRenderInterval = (renderRate < Double.Epsilon) ? 0d : 1d / renderRate;
		}

		#region Flow control (start/stop)

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
		/// <summary>
		/// IDs of the update and render thread - used to verify that the correct thread is acquiring states
		/// </summary>
		private int _updateThreadId = -1, _renderThreadId = -1;
#endif

		/// <summary>
		/// Starts the state manager.
		/// This call blocks until told to exit by the <see cref="Stop"/> method.
		/// </summary>
		/// <param name="multiThreaded">Indicates whether frame processing should be multi-threaded</param>
		/// <exception cref="InvalidOperationException">Thrown if the state manager is already running</exception>
		/// <exception cref="ObjectDisposedException">Thrown if the state manager has already been disposed</exception>
		public void Run (bool multiThreaded = true)
		{
			if(Disposed)
				throw new ObjectDisposedException(GetType().FullName);
			if(_running)
				throw new InvalidOperationException("The state manager is already running.");

			_running = true;
			if(multiThreaded)
				multiThreadedGameLoop();
			else
				singleThreadedGameLoop();
		}

		/// <summary>
		/// Single-threaded game loop
		/// </summary>
		private void singleThreadedGameLoop ()
		{
#if DEBUG
			_renderThreadId = _updateThreadId = Thread.CurrentThread.ManagedThreadId;
#endif

			// Allocate these on the stack for faster access
			var updateStopwatch = new Stopwatch();
			var renderStopwatch = new Stopwatch();
			var nextUpdateTime  = 0d;
			var nextRenderTime  = 0d;
			updateStopwatch.Start();
			renderStopwatch.Start();

			while(_running)
			{
				updateTiming(updateStopwatch, ref nextUpdateTime);
				renderTiming(renderStopwatch, ref nextRenderTime);

				// TODO: Do something to reduce high CPU utilization
			}
		}

		/// <summary>
		/// Multi-threaded game loop
		/// </summary>
		private void multiThreadedGameLoop ()
		{
			// Disable rendering on the current thread so that the render thread can do it
			_display.SetActive(false);
			
			// Create and start the render thread
			var renderThread = new Thread(doRenderLoop) {
				Name     = "Render Thread",
				Priority = ThreadPriority.AboveNormal
			};
			renderThread.Start();

			// Process updates until stopped and wait for the render thread to stop
			doUpdateLoop();
			renderThread.Join();
		}

		/// <summary>
		/// Stops the state manager.
		/// This will cause the <see cref="Run"/> method to return.
		/// </summary>
		public void Stop ()
		{
			_running = false;
		}
		#endregion

		#region Update and render loops/logic

		/// <summary>
		/// Frame number that each of the states are on
		/// </summary>
		private readonly long[] _stateFrameNumbers = new[] { 0L, 0L, 0L };

		/// <summary>
		/// Total number of frames that were skipped.
		/// This represents the frames that were updated, but were not rendered.
		/// </summary>
		public long SkippedFrames { get; private set; }

		/// <summary>
		/// Total number of frames encountered that are duplicates
		/// </summary>
		public long DuplicatedFrames { get; private set; }

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
		/// Indicates if the update and render threads should stay in sync with each other.
		/// This option is only valid and works where multi-threaded is enabled and the update and render rates should be the same.
		/// </summary>
		/// <remarks>Thread synchronization will prevent duplicate and skipped frames.</remarks>
		public bool ThreadSynchronization { get; set; }

		/// <summary>
		/// Indicates whether the update process is falling behind
		/// </summary>
		public bool IsRunningSlow { get; private set; }

		/// <summary>
		/// Maximum number of measurements to take for averaging update and render intervals
		/// </summary>
		private const int MeasurementCount = 120;

		/// <summary>
		/// Tracks the average time interval taken
		/// </summary>
		private readonly SampleCounter _updateCounter = new SampleCounter(MeasurementCount),
			_renderCounter = new SampleCounter(MeasurementCount);

		#region Update

		/// <summary>
		/// Default targeted number of logic updates per second
		/// </summary>
		public const double DefaultTargetUpdateRate = 60d;

		/// <summary>
		/// Default length of time (in seconds) to target for each frame update
		/// </summary>
		public const double DefaultTargetUpdateInterval = 1d / DefaultTargetUpdateRate;

		/// <summary>
		/// Maximum length of time (in seconds) that can pass before the game is forced to update.
		/// This is used to prevent the game from becoming unresponsive.
		/// </summary>
		private const double MaxUpdateInterval = 1d;

		/// <summary>
		/// Target length of time (in seconds) for each frame update
		/// </summary>
		private double _targetUpdateInterval = DefaultTargetUpdateInterval;

		/// <summary>
		/// Number of frames updated per second
		/// </summary>
		/// <remarks>This is a running average.</remarks>
		public double UpdateRate
		{
			get
			{
				var avg = _updateCounter.Average;
				return (avg < Double.Epsilon) ? TargetUpdateRate : 1d / avg;
			}
		}

		/// <summary>
		/// Target number of frame updates per second.
		/// A value of 0 represents no limit of frames per second.
		/// </summary>
		public double TargetUpdateRate
		{
			get { return (_targetUpdateInterval < Double.Epsilon) ? 0d : 1d / _targetUpdateInterval; }
			set { _targetUpdateInterval = (value < Double.Epsilon) ? 0d : 1d / value; }
		}

		/// <summary>
		/// Indicates whether the number of updated frames per second is unbounded
		/// </summary>
		public bool UnboundedUpdateRate
		{
			get { return _targetUpdateInterval < Double.Epsilon; }
		}

		/// <summary>
		/// Maximum number of updates that can occur consecutively to help the game catch up
		/// </summary>
		private const int MaxConsecutiveUpdates = 10;

		/// <summary>
		/// Length of time (in seconds) that it took to just update the previous frame (does not include sleep time)
		/// </summary>
		public double LastUpdateInterval { get; private set; }

		/// <summary>
		/// Running average length of time (in seconds) taken to perform a game update
		/// </summary>
		public double UpdateInterval
		{
			get { return _updateCounter.Average; }
		}

		/// <summary>
		/// Index of the previous state that was updated
		/// </summary>
		private int _prevUpdateStateIndex;

		/// <summary>
		/// Index of the current state being updated.
		/// -1 means no state is currently being updated.
		/// </summary>
		private int _curUpdateStateIndex = -1;

		/// <summary>
		/// Frame number of the previous state that was updated
		/// </summary>
		private long _prevUpdateFrameNumber = -1;

		/// <summary>
		/// Current frame number
		/// </summary>
		/// <remarks>Technically, this is the frame number of the frame currently being updated.</remarks>
		public long FrameNumber { get; private set; }

		/// <summary>
		/// Reset event that indicates whether the update thread has produced a frame
		/// </summary>
		private readonly ManualResetEventSlim _updateSignal = new ManualResetEventSlim();

		/// <summary>
		/// Gets the next state to update and marks it
		/// </summary>
		/// <returns>A state index</returns>
		private int acquireNextUpdateState (out int prevStateIndex)
		{
#if DEBUG
			if(Thread.CurrentThread.ManagedThreadId != _updateThreadId)
				throw new AccessViolationException("Only the update thread may acquire an update state.");
#endif
			lock(_stateFrameNumbers)
			{
#if DEBUG
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
				prevStateIndex = _prevUpdateStateIndex;
				return _curUpdateStateIndex;
			}
		}

		/// <summary>
		/// Releases a state from updating so that it can be rendered
		/// </summary>
		private void releaseUpdateState ()
		{
#if DEBUG
			if(Thread.CurrentThread.ManagedThreadId != _updateThreadId)
				throw new AccessViolationException("Only the update thread may release an update state.");
#endif
			lock(_stateFrameNumbers)
			{
#if DEBUG
				if(_curUpdateStateIndex == -1)
					throw new InvalidOperationException("Cannot release an update state when no state is currently being updated.");
#endif
				_prevUpdateStateIndex  = _curUpdateStateIndex;
				_prevUpdateFrameNumber = _stateFrameNumbers[_prevUpdateStateIndex] = FrameNumber++;
				_curUpdateStateIndex   = -1;
				_updateSignal.Set(); // Let the renderer know that there's a frame ready
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
				if(Disposed ||
					(_curRenderStateIndex == -1 && _prevRenderStateIndex == _prevUpdateStateIndex) || // Render thread just finished it
					_curRenderStateIndex == _prevUpdateStateIndex) // Render thread just started it
					return true;
				_renderSignal.Reset();
			}
			return _renderSignal.Wait(timeout);
		}

		/// <summary>
		/// Repeatedly performs game logic updates until told to stop
		/// </summary>
		/// <remarks>This method should only be called in multi-threaded mode.</remarks>
		private void doUpdateLoop ()
		{
#if DEBUG
			_updateThreadId = Thread.CurrentThread.ManagedThreadId;
#endif
			var timeout = TimeSpan.FromSeconds(MaxUpdateInterval);

			// Place these on the stack for faster access
			var stopwatch      = new Stopwatch();
			var nextUpdateTime = 0d;
			stopwatch.Start();

			update(); // Generate the first frame to start the process
			while(_running)
			{
				if(!ThreadSynchronization || waitForRender(timeout))
					updateTiming(stopwatch, ref nextUpdateTime);

				// TODO: Do something to reduce high CPU utilization
			}
		}

		/// <summary>
		/// Handles timing for the update phase and only calls <see cref="update"/> if it's time.
		/// This method returns without updating if it's not time to perform an update step.
		/// </summary>
		/// <param name="stopwatch">Stopwatch used to calculate when updates should occur</param>
		/// <param name="nextUpdateTime">Amount of time (in seconds) until the next update should occur</param>
		private void updateTiming (Stopwatch stopwatch, ref double nextUpdateTime)
		{
			var updatesProcessed = 0;  // Number of updates processed since this method was called
			var totalUpdateTime  = 0d; // Total time taken to perform all updates since this method was called

			var time = stopwatch.Elapsed.TotalSeconds;
			if(time <= 0d)
				return; // What are we even doing here?
			if(time > MaxUpdateInterval)
				time = MaxUpdateInterval; // Prevent the game from becoming unresponsive

			// Continue performing updates to catch up (if fallen behind)
			while(nextUpdateTime - time <= 0d && time > 0d)
			{// It's time for an update
				// Schedule the next update
				nextUpdateTime -= time;
				nextUpdateTime += _targetUpdateInterval;
				if(nextUpdateTime < -MaxUpdateInterval)
					nextUpdateTime = -MaxUpdateInterval; // ... but don't schedule it too soon to prevent overload
				IsRunningSlow = (nextUpdateTime <= 0d && !UnboundedUpdateRate);

				// Perform the update
				update();

				// Calculate how long the update took
				var elapsed = stopwatch.Elapsed.TotalSeconds;
				_updateCounter.AddMeasurement(elapsed);
				LastUpdateInterval = time = elapsed - time;
				nextUpdateTime -= time;
				totalUpdateTime += time;

				// Reset the stopwatch, since it isn't accurate over longer periods of time.
				stopwatch.Reset();
				stopwatch.Start();

				// Allow consecutive updates to occur, but not too many.
				// This allows the hardware to catch up.
				if(++updatesProcessed >= MaxConsecutiveUpdates || UnboundedUpdateRate)
					break;
			}

			if(updatesProcessed > 0)
			{// Update statistics
				var avgTime = totalUpdateTime / updatesProcessed;
				LastUpdateInterval = avgTime;
			}
		}

		/// <summary>
		/// Performs a single update to the game state
		/// </summary>
		private void update ()
		{
			// Get the previous state and next state to update
			int prevStateIndex;
			var nextStateIndex = acquireNextUpdateState(out prevStateIndex);

			// Perform the update
			_display.Update();
			_updateRoot.StepState(prevStateIndex, nextStateIndex);
//			((Window)_display).Title = ToString() + " - " + StateString; // TODO: Remove this

			// Release the state
			releaseUpdateState();
		}
		#endregion

		#region Render

		/// <summary>
		/// Default targeted number of drawn frames per second
		/// </summary>
		public const double DefaultTargetRenderRate = 60d;

		/// <summary>
		/// Default length of time (in seconds) to target for each frame render
		/// </summary>
		public const double DefaultTargetRenderInterval = 1d / DefaultTargetRenderRate;

		/// <summary>
		/// Maximum length of time (in seconds) that can pass before the game is forced to draw.
		/// This is used to prevent the game from appearing unresponsive.
		/// </summary>
		private const double MaxRenderInterval = 1d;

		/// <summary>
		/// Target length of time (in seconds) for each frame render
		/// </summary>
		private double _targetRenderInterval = DefaultTargetRenderInterval;

		/// <summary>
		/// Number of frames rendered per second
		/// </summary>
		/// <remarks>This is a running average.</remarks>
		public double RenderRate
		{
			get
			{
				var avg = _renderCounter.Average;
				return (avg < Double.Epsilon) ? TargetRenderRate : 1d / avg;
			}
		}

		/// <summary>
		/// Target number of rendered frames per second.
		/// A value of 0 represents no limit of frames per second.
		/// </summary>
		public double TargetRenderRate
		{
			get { return (_targetRenderInterval < Double.Epsilon) ? 0d : 1d / _targetRenderInterval; }
			set { _targetRenderInterval = (value < Double.Epsilon) ? 0d : 1d / value; }
		}

		/// <summary>
		/// Indicates whether the number of rendered frames per second is unbounded
		/// </summary>
		public bool UnboundedRenderRate
		{
			get { return _targetRenderInterval < Double.Epsilon; }
		}

		/// <summary>
		/// Length of time (in seconds) that it took to just render the previous frame (does not include sleep time)
		/// </summary>
		public double LastRenderInterval { get; private set; }

		/// <summary>
		/// Running average length of time (in seconds) taken to render a frame
		/// </summary>
		public double RenderInterval
		{
			get { return _updateCounter.Average; }
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
		private long _prevRenderFrameNumber;

		/// <summary>
		/// Reset event that indicates whether the render thread is busy rendering a state
		/// </summary>
		private readonly ManualResetEventSlim _renderSignal = new ManualResetEventSlim();

		/// <summary>
		/// Marks the next state as being rendered and retrieves the index
		/// </summary>
		/// <param name="nextstateIndex">Index of the previous state that was rendered (used to detect duplicates)</param>
		/// <returns>A state index</returns>
		private int acquireNextRenderState (out int nextstateIndex)
		{
#if DEBUG
			if(Thread.CurrentThread.ManagedThreadId != _renderThreadId)
				throw new AccessViolationException("Only the render thread may acquire a render state.");
#endif
			lock(_stateFrameNumbers)
			{
#if DEBUG
				if(_curRenderStateIndex != -1)
					throw new InvalidOperationException("Cannot acquire the next state to draw until the previous state has been released.");
#endif
				_curRenderStateIndex = _prevUpdateStateIndex;
				var frameNumber = _stateFrameNumbers[_curRenderStateIndex];
				if(_prevRenderStateIndex != -1)
				{
					if(frameNumber == _prevRenderFrameNumber)
						++DuplicatedFrames;
					else
						SkippedFrames += frameNumber - _prevRenderFrameNumber - 1;
				}
				else // First frame being drawn
					SkippedFrames += frameNumber;
				_renderSignal.Set();
				nextstateIndex = _prevRenderStateIndex;
				return _curRenderStateIndex;
			}
		}

		/// <summary>
		/// Releases the state currently be rendered
		/// </summary>
		private void releaseRenderState ()
		{
#if DEBUG
			
			if(Thread.CurrentThread.ManagedThreadId != _renderThreadId)
				throw new AccessViolationException("Only the render thread may release a render state.");
#endif
			lock(_stateFrameNumbers)
			{
#if DEBUG
				if(_curRenderStateIndex == -1)
					throw new InvalidOperationException("Cannot release a render state when no state is currently being drawn.");
#endif
				_prevRenderStateIndex  = _curRenderStateIndex;
				_prevRenderFrameNumber = _stateFrameNumbers[_prevRenderStateIndex];
				_curRenderStateIndex   = -1;
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
				if(_prevRenderFrameNumber < _prevUpdateFrameNumber)
					return true; // There's already a frame waiting
				_updateSignal.Reset();
			}
			return _updateSignal.Wait(timeout);
		}

		/// <summary>
		/// Repeatedly draws frames to the screen
		/// </summary>
		/// <remarks>This method should only be called in multi-threaded mode.</remarks>
		/// <exception cref="AccessViolationException">Thrown if the display to render to could not be enabled for the current thread.
		/// Make sure the display is disabled on all other threads.</exception>
		private void doRenderLoop ()
		{
#if DEBUG
			_renderThreadId = Thread.CurrentThread.ManagedThreadId;
#endif
			if(!_display.SetActive())
				throw new AccessViolationException("Could not activate rendering to the display on the state manager's render thread. It may be active on another thread.");
			var timeout = TimeSpan.FromSeconds(MaxRenderInterval);

			// Stack access is faster for these since they're checked quite frequently
			var stopwatch      = new Stopwatch();
			var nextRenderTime = 0d;
			stopwatch.Start();

			while(_running)
			{
				if(!ThreadSynchronization || waitForUpdate(timeout))
					renderTiming(stopwatch, ref nextRenderTime);

				// TODO: Do something to reduce high CPU utilization
			}
		}

		/// <summary>
		/// Handles timing for the render phase and only calls <see cref="render"/> if it's time.
		/// This method returns without drawing if it's not time to perform a render.
		/// </summary>
		/// <param name="stopwatch">Stopwatch used to </param>
		/// <param name="nextRenderTime">Amount of time remaining (in seconds) until the next render needs to occur</param>
		private void renderTiming (Stopwatch stopwatch, ref double nextRenderTime)
		{
			var time = stopwatch.Elapsed.TotalSeconds;
			if(time <= 0d)
				return; // "Way too early to render," abort to avoid timing corruption
			if(time > MaxRenderInterval)
				time = MaxRenderInterval; // Prevent the game from appearing unresponsive

			var timeRemaining = nextRenderTime - time;
			if(timeRemaining <= 0d)
			{// It's time to draw a frame
				// Calculate when the next frame should be rendered
				nextRenderTime = timeRemaining + _targetRenderInterval;
				if(nextRenderTime < -MaxRenderInterval)
					nextRenderTime = -MaxRenderInterval; // ... but don't schedule it too soon - give the system some time to breathe

				// Reset the stopwatch to keep accuracy
				stopwatch.Reset();
				stopwatch.Start();

				if(time > 0d)
				{
					// TODO: Implement adaptive VSync

					// Record intervals and draw the frame
					_renderCounter.AddMeasurement(time);
					render();
					LastRenderInterval = stopwatch.Elapsed.TotalSeconds;
				}
			}
		}

		/// <summary>
		/// Draws a single frame to the screen
		/// </summary>
		private void render ()
		{
			// Retrieve the next state to render
			int prevStateIndex;
			var nextStateIndex = acquireNextRenderState(out prevStateIndex);

			if(RenderDuplicateFrames || prevStateIndex != nextStateIndex)
			{// Render the frame
				_display.EnterFrame();
				_renderRoot.DrawState(_display, nextStateIndex);
				_display.ExitFrame();

				if(prevStateIndex == nextStateIndex)
					++RenderedDuplicateFrames;
			}

			// Release the state
			releaseRenderState();
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
			sb.Append(" u/s ");
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

		#region Disposable

		/// <summary>
		/// Flag that indicates whether the state manager has been disposed
		/// </summary>
		public bool Disposed { get; private set; }

		/// <summary>
		/// Frees the resources held by the state manager
		/// </summary>
		/// <remarks>Disposing of the state manager will stop it if it is still running.</remarks>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Tears down the state manager
		/// </summary>
		~StateManager ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Underlying method that releases the resources held by the state manager
		/// </summary>
		/// <param name="disposing">True if <see cref="Dispose"/> was called and inner resources should be disposed as well</param>
		protected void Dispose (bool disposing)
		{
			if(!Disposed)
			{// Don't do anything if the state manager is already disposed
				if(disposing)
				{// Dispose of the resources this object holds
					Stop();
				}
				Disposed = true;
			}
		}
		#endregion
	}
}
