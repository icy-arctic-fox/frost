using System;
using System.Threading;

namespace Frost.Logic
{
	/// <summary>
	/// Tracks the states to be updated and rendered
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
		/// Creates a new state manager
		/// </summary>
		public StateManager ()
		{
			// TODO: Add "backlog" state count argument (for roll-back)
		}

		/// <summary>
		/// Frame number that each of the states are on
		/// </summary>
		private readonly long[] _stateFrameNumbers = new[] { 0L, 0L, 0L };

		#region Update

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

#if DEBUG
		/// <summary>
		/// ID of the thread that is allowed to acquire update states
		/// </summary>
		internal int UpdateThreadId { private get; set; }
#endif

		/// <summary>
		/// Gets the next state to update and marks it
		/// </summary>
		/// <returns>A state index</returns>
		internal int AcquireNextUpdateState (out int prevStateIndex)
		{
#if DEBUG
			if(Thread.CurrentThread.ManagedThreadId != UpdateThreadId)
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
		internal void ReleaseUpdateState ()
		{
#if DEBUG
			if(Thread.CurrentThread.ManagedThreadId != UpdateThreadId)
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
		internal bool WaitForRender (TimeSpan timeout)
		{
			lock(_stateFrameNumbers)
			{
				if((_curRenderStateIndex == -1 && _prevRenderStateIndex == _prevUpdateStateIndex) || // Render thread just finished it
					_curRenderStateIndex == _prevUpdateStateIndex) // Render thread just started it
					return true;
				_renderSignal.Reset();
			}
			return _renderSignal.Wait(timeout);
		}
		#endregion

		#region Render

		/// <summary>
		/// Total number of frames encountered that are duplicates
		/// </summary>
		public long DuplicatedFrames { get; private set; }

		/// <summary>
		/// Total number of frames that were skipped.
		/// This represents the frames that were updated, but were not rendered.
		/// </summary>
		public long SkippedFrames { get; private set; }

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

#if DEBUG
		/// <summary>
		/// ID of the thread that is allowed to acquire render states
		/// </summary>
		internal int RenderThreadId { private get; set; }
#endif

		/// <summary>
		/// Marks the next state as being rendered and retrieves the index
		/// </summary>
		/// <param name="nextstateIndex">Index of the previous state that was rendered (used to detect duplicates)</param>
		/// <returns>A state index</returns>
		internal int AcquireNextRenderState (out int nextstateIndex)
		{
#if DEBUG
			if(Thread.CurrentThread.ManagedThreadId != RenderThreadId)
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
		internal void ReleaseRenderState ()
		{
#if DEBUG
			if(Thread.CurrentThread.ManagedThreadId != RenderThreadId)
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
		internal bool WaitForUpdate (TimeSpan timeout)
		{
			lock(_stateFrameNumbers)
			{
				if(_prevRenderFrameNumber < _prevUpdateFrameNumber)
					return true; // There's already a frame waiting
				_updateSignal.Reset();
			}
			return _updateSignal.Wait(timeout);
		}
		#endregion

		/// <summary>
		/// Generates a string that contains the frame states and the operations being performed on them.
		/// &lt; &gt; signify that the state is being rendered.
		/// [ ] signify that the state is being updated.
		/// </summary>
		/// <returns>A string that shows which frames are being rendered and updated</returns>
		public override string ToString ()
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
