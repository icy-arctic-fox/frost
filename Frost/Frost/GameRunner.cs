using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Frost.Display;
using Frost.Logic;
using Frost.Modules;
using Frost.UI;
using Frost.Utility;

namespace Frost
{
	/// <summary>
	/// Runs the game loop and controls the flow of the game between states and scenes
	/// </summary>
	public class GameRunner : IFullDisposable, IDebugOverlayLine
	{
		private const string DebugOverlayFontFile = "crystal.TTF";
		private const uint DebugOverlayFontSize   = 12;

		/// <summary>
		/// Display that will be rendered upon
		/// </summary>
		private readonly IDisplay _display;

		/// <summary>
		/// Tracks the active scene
		/// </summary>
		private readonly SceneManager _scenes;

		/// <summary>
		/// Game scenes being ran
		/// </summary>
		public SceneManager Scenes
		{
			get { return _scenes; }
		}

		/// <summary>
		/// Debug information
		/// </summary>
		private readonly DebugOverlay _debugOverlay;

		private volatile bool _debug
#if DEBUG
			= true // Enable debug overlay by default with debug builds
#endif
			;

		/// <summary>
		/// Indicates whether the debug overlay is displayed
		/// </summary>
		public bool Debug
		{
			get { return _debug && _debugOverlay != null; }
			set { _debug = value; }
		}

		/// <summary>
		/// Creates a new game runner
		/// </summary>
		/// <param name="display">Display to render to</param>
		/// <param name="initialScene">Initial scene to process</param>
		public GameRunner (IDisplay display, Scene initialScene)
		{
			if(display == null)
				throw new ArgumentNullException("display", "The display to render to can't be null.");

			// Try to load the debug overlay font
			Graphics.Text.Font font;
			try
			{
				font = Graphics.Text.Font.LoadFromFile(DebugOverlayFontFile);
			}
			catch(SFML.LoadingFailedException)
			{// Font failed to load
				// TODO: Report this failure
				font = null;
			}

			_display = display;
			_scenes  = new SceneManager(initialScene, display);

			if(font != null)
			{// Set up the debug overlay
				_debugOverlay = new DebugOverlay(this, font, DebugOverlayFontSize);

				// Default overlay lines
				_debugOverlay.AddLine(this); // Frame information
				_debugOverlay.AddLine(new SceneDebugOverlayLine(this)); // Scene information
			}
			else
				_debugOverlay = null; // Font failed to load, can't use the overlay
		}

		#region Modules
		/// <summary>
		/// Describes a method that updates a module
		/// </summary>
		private delegate void ModuleUpdate ();

		/// <summary>
		/// Collection of module update methods to call for each logic update
		/// </summary>
		private readonly List<ModuleUpdate> _moduleUpdates = new List<ModuleUpdate>();

		/// <summary>
		/// Adds a module to the game which is processed each logic update
		/// </summary>
		/// <param name="module">Module to add</param>
		internal void AddModule (IModule module)
		{
			if(module == null)
				throw new ArgumentNullException("module", "The module to add for processing can't be null.");
			_moduleUpdates.Add(module.Update);
		}
		#endregion

		#region Flow control (start/stop)

		/// <summary>
		/// Indicates if the game is running.
		/// When false and executing, the update and render loops should exit.
		/// </summary>
		private volatile bool _running;

		/// <summary>
		/// Indicates whether the game has started and is currently operating
		/// </summary>
		public bool Running
		{
			get { return _running; }
		}

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

		/// <summary>
		/// Starts the game runner.
		/// This call blocks until told to exit by the <see cref="Stop"/> method.
		/// </summary>
		/// <param name="rate">Target number of updates and rendered frames per second - use 0 for no limit</param>
		/// <param name="multiThreaded">Indicates whether frame processing should be multi-threaded</param>
		public void Run (double rate, bool multiThreaded = true)
		{
			Run(rate, rate, multiThreaded);
		}

		/// <summary>
		/// Starts the game runner.
		/// This call blocks until told to exit by the <see cref="Stop"/> method.
		/// </summary>
		/// <param name="updateRate">Target number of updates per second - use 0 for no limit</param>
		/// <param name="renderRate">Target number of rendered frames per second - use 0 for no limit</param>
		/// <param name="multiThreaded">Indicates whether frame processing should be multi-threaded</param>
		/// <exception cref="InvalidOperationException">Thrown if the game is already running</exception>
		/// <exception cref="ObjectDisposedException">Thrown if the game has already been disposed</exception>
		public void Run (double updateRate = DefaultTargetUpdateRate, double renderRate = DefaultTargetRenderRate, bool multiThreaded = true)
		{
			if(Disposed)
				throw new ObjectDisposedException(GetType().FullName);
			if(_running)
				throw new InvalidOperationException("The game is already running.");

			_targetUpdateInterval = (updateRate <= 0d) ? 0d : 1d / updateRate;
			_targetRenderInterval = (renderRate <= 0d) ? 0d : 1d / renderRate;

			if(_debug)
				_scenes.AddOverlay(_debugOverlay);

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
			var tid = Thread.CurrentThread.ManagedThreadId;
			_scenes.UpdateThreadId = tid;
			_scenes.RenderThreadId = tid;
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
		/// Stops the game.
		/// This will cause the Run() method to return.
		/// </summary>
		public void Stop ()
		{
			_running = false;
		}
		#endregion

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
		/// Repeatedly performs game logic updates until told to stop
		/// </summary>
		/// <remarks>This method should only be called in multi-threaded mode.</remarks>
		private void doUpdateLoop ()
		{
#if DEBUG
			_scenes.UpdateThreadId = Thread.CurrentThread.ManagedThreadId;
#endif
			var timeout = TimeSpan.FromSeconds(MaxUpdateInterval);

			// Place these on the stack for faster access
			var stopwatch      = new Stopwatch();
			var nextUpdateTime = 0d;
			stopwatch.Start();

			_scenes.Update(); // Generate the first frame to start the process
			while(_running)
			{
				if(!ThreadSynchronization || _scenes.StateManager.WaitForRender(timeout))
					updateTiming(stopwatch, ref nextUpdateTime);

				// TODO: Do something to reduce high CPU utilization
			}
		}

		/// <summary>
		/// Handles timing for the update phase and only updates if it's time to do so.
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
				for(var i = 0; i < _moduleUpdates.Count; ++i)
					_moduleUpdates[i]();
				if(_debug && _debugOverlay != null)
					_debugOverlay.Update();
				if(!_scenes.Update())
					_running = false; // All scenes exited

				// Calculate how long the update took
				var elapsed = stopwatch.Elapsed.TotalSeconds;
				_updateCounter.AddMeasurement(elapsed);
				LastUpdateInterval = time = elapsed - time;
				nextUpdateTime  -= time;
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
			get { return _renderCounter.Average; }
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
			_scenes.RenderThreadId = Thread.CurrentThread.ManagedThreadId;
#endif
			if(!_display.SetActive())
				throw new AccessViolationException("Could not activate rendering to the display on the render thread. It may be active on another thread.");
			var timeout = TimeSpan.FromSeconds(MaxRenderInterval);

			// Stack access is faster for these since they're checked quite frequently
			var stopwatch      = new Stopwatch();
			var nextRenderTime = 0d;
			stopwatch.Start();

			while(_running)
			{
				if(!ThreadSynchronization || _scenes.StateManager.WaitForUpdate(timeout))
					renderTiming(stopwatch, ref nextRenderTime);

				// TODO: Do something to reduce high CPU utilization
			}
		}

		/// <summary>
		/// Handles timing for the render phase and only draws if it's time to do so.
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
					_scenes.Render();
					LastRenderInterval = stopwatch.Elapsed.TotalSeconds;
				}
			}
		}
		#endregion

		#region Disposable

		/// <summary>
		/// Flag that indicates whether the game runner has been disposed
		/// </summary>
		public bool Disposed { get; private set; }

		/// <summary>
		/// Triggered when the game runner is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Frees the resources held by the runner
		/// </summary>
		/// <remarks>Disposing of the game runner will stop it if it is still running.</remarks>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Tears down the game runner
		/// </summary>
		~GameRunner ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Underlying method that releases the resources held by the runner
		/// </summary>
		/// <param name="disposing">True if <see cref="Dispose"/> was called and inner resources should be disposed as well</param>
		protected void Dispose (bool disposing)
		{
			if(!Disposed)
			{// Don't do anything if the runner is already disposed
				Disposed = true;
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);
				if(disposing)
				{// Dispose of the resources this object holds
					// TODO
				}
			}
		}
		#endregion

		/// <summary>
		/// Generates a string that represents the status of the game
		/// </summary>
		/// <returns>A string in the form:
		/// Frame: # - # u/s - # f/s (#/# dups, # skipped)</returns>
		public override string ToString ()
		{
			var sm = _scenes.StateManager;
			var sb = new StringBuilder();
			sb.Append("Frame: ");
			sb.Append(sm.FrameNumber);
			sb.Append(" - ");
			sb.Append(String.Format("{0:0.00}", UpdateRate));
			sb.Append(" u/s ");
			sb.Append(String.Format("{0:0.00}", RenderRate));
			sb.Append(" f/s (");
			sb.Append(_scenes.RenderedDuplicateFrames);
			sb.Append('/');
			sb.Append(sm.DuplicatedFrames);
			sb.Append(" dups, ");
			sb.Append(sm.SkippedFrames);
			sb.Append(" skipped)");
			return sb.ToString();
		}
	}
}
