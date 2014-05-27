using System;
using System.Collections.Generic;

namespace Frost
{
	/// <summary>
	/// Information for game logic update each frame
	/// </summary>
	/// <remarks>The contents of the class are internally mutable so that a single instance can be reused.</remarks>
	public class FrameStepEventArgs : EventArgs
	{
		/// <summary>
		/// Index of the state that was updated last frame
		/// </summary>
		public int PreviousStateIndex { get; internal set; }

		/// <summary>
		/// Index of the state to update this frame
		/// </summary>
		public int NextStateIndex { get; internal set; }

		/// <summary>
		/// Current frame number
		/// </summary>
		public long FrameNumber { get; internal set; }

		/// <summary>
		/// Length of time that the game has been running for
		/// </summary>
		public TimeSpan GameTime { get; internal set; }

		/// <summary>
		/// Indicates whether game updates are falling behind of the target update rate
		/// </summary>
		public bool IsRunningSlow { get; internal set; }

		#region PreUpdate

		private readonly List<Action> _preUpdateActions = new List<Action>();

		/// <summary>
		/// Appends any queued pre-update actions to queues for processing before the next update.
		/// After appending, the queued actions are cleared.
		/// </summary>
		/// <param name="stateQueue1">First queue to append actions to</param>
		/// <param name="stateQueue2">Second queue to append actions to</param>
		internal void AppendAndClearPreUpdateActions (Queue<Action> stateQueue1, Queue<Action> stateQueue2)
		{
			for(var j = 0; j < _preUpdateActions.Count; ++j)
			{// Append each action to the queues
				var action = _preUpdateActions[j];
				stateQueue1.Enqueue(action);
				stateQueue2.Enqueue(action);
			}

			_preUpdateActions.Clear();
		}

		/// <summary>
		/// Queues some logic to be executed before the next update for other states.
		/// This can be used to perform initialization for entity states.
		/// </summary>
		/// <param name="action">Code to execute before the other states are updated</param>
		/// <exception cref="ArgumentNullException">The <paramref name="action"/> to queue can't be null.</exception>
		public void QueuePreUpdateAction (Action action)
		{
			if(action == null)
				throw new ArgumentNullException("action");

			_preUpdateActions.Add(action);
		}
		#endregion
	}
}
