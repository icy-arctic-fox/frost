using System;
using System.Diagnostics;
using System.Threading;
using Frost.Display;
using Frost.Modules.State;

namespace Frost.Modules
{
	/// <summary>
	/// Tracks the frames to be updated and rendered.
	/// The state manager uses multiple states in memory so that updates and rendering can take place on separate threads concurrently.
	/// </summary>
	public class StateManager
	{
		/// <summary>
		/// Number of active states in memory.
		/// Each component that can be modified during runtime needs to have this many states.
		/// </summary>
		public const int StateCount = 3;

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
			_renderThread = new Thread(doRenderLoop) {
				Name     = "State Manager Render Thread",
				Priority = ThreadPriority.AboveNormal
			};

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
			// Disable rendering on the current thread so that the render thread can do it
			_display.SetActive(false);

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
		public ulong Frame { get; private set; }

		/// <summary>
		/// Number of frames rendered
		/// </summary>
		public ulong FramesRendered { get; private set; }

		/// <summary>
		/// Number of times a frame has been rendered more than once
		/// </summary>
		public ulong DuplicateFrames { get; private set; }

		/// <summary>
		/// Number of frames that weren't rendered
		/// </summary>
		public ulong SkippedFrames { get; private set; }

		/// <summary>
		/// Frame number that each state is currently at
		/// </summary>
		private readonly ulong[] _stateFrames = new ulong[] {0, 0, 0};

		/// <summary>
		/// Index of the previous state updated
		/// </summary>
		private int _updateIndex;

		/// <summary>
		/// Index of the previous state rendered
		/// </summary>
		private int _renderIndex;

		#region Update

		/// <summary>
		/// Default length of time to target for each update frame
		/// </summary>
		public const double DefaultTargetUpdatePeriod = 1f / 30f;

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
			get { return (_updatePeriod > 0d) ? 1d / _updatePeriod : 0d; }
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
				// Find out which frame state object that should be updated
				int prevState, nextState;
				lock(_stateFrames)
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
					default: // 2
						nextState = _renderIndex == 0 ? 1 : 0;
						break;
					}

					// Store information about the frame we just updated
					_updateIndex = nextState;
					++Frame;
					_stateFrames[nextState] = Frame;
				}

				// Update the game logic
				_display.Update();
				_updateRoot.StepState(prevState, nextState);
				((Window)_display).Title = ToString() + " " + StateString;

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
			get { return (_renderPeriod > 0d) ? 1d / _renderPeriod : 0d; }
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
		/// Indicates if duplicate frames should be rendered.
		/// Duplicate frames occur when the game updates lag behind the screen (render) updates.
		/// </summary>
		public bool RenderDuplicateFrames { get; set; }

		/// <summary>
		/// Threaded method that runs the render loop
		/// </summary>
		/// <exception cref="AccessViolationException">Thrown if the display to render to could not be enabled for the current thread.
		/// Make sure the display is disabled on all other threads.</exception>
		private void doRenderLoop ()
		{
			if(!_display.SetActive())
				throw new AccessViolationException("Could not activate rendering to the display on the state manager's render thread. It may be active on another thread.");

			_renderTimer.Start();
			while(_running)
			{// Continue rendering until told to stop
				var dup = false;

				// Find out which frame should be rendered.
				// Make sure to NOT pick the frame that is currently being updated, as it will be unstable.
				int state;
				lock(_stateFrames)
				{
					switch(_renderIndex)
					{
					case 0:
						state = _updateIndex == 1 ? 2 : 1;
						break;
					case 1:
						state = _updateIndex == 0 ? 2 : 0;
						break;
					default: // 2
						state = _updateIndex == 0 ? 1 : 0;
						break;
					}

					// Check if this is a duplicated frame (update is behind render)
					var prevFrame = _stateFrames[_renderIndex];
					var nextFrame = _stateFrames[state];
					if(nextFrame <= prevFrame)
					{
						dup = true;
						++DuplicateFrames;
						// Set the state to be rendered back to the previous one to prevent "jitter" (briefly showing the previous frame again).
						state = _renderIndex;
					}
					else
					{// New frame
						SkippedFrames += nextFrame - prevFrame - 1;
						_renderIndex = state;
					}
				}

				if(!dup || RenderDuplicateFrames)
				{// Render the frame
					_display.EnterFrame();
					_renderRoot.DrawState(_display, state);
					_display.ExitFrame();
					++FramesRendered;
				}

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

		/// <summary>
		/// Generates a string that represents the status of the state manager
		/// </summary>
		/// <returns>A string in the form:
		/// Frame: # - # u/s - # f/s (# dups, # skipped)</returns>
		public override string ToString ()
		{
			var sb = new System.Text.StringBuilder();
			sb.Append("Frame: ");
			sb.Append(Frame);
			sb.Append(" - ");
			sb.Append(String.Format("{0:0.00}", UpdateRate));
			sb.Append(" u/s - ");
			sb.Append(String.Format("{0:0.00}", RenderRate));
			sb.Append(" f/s (");
			sb.Append(DuplicateFrames);
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
				var frames = new ulong[3];
				int updateIndex, renderIndex;
				lock(_stateFrames)
				{// Grab the values
					for(var i = 0; i < 3; ++i)
						frames[i] = _stateFrames[i];
					updateIndex = _updateIndex;
					renderIndex = _renderIndex;
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
