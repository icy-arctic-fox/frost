using System;

namespace Frost
{
	/// <summary>
	/// Information about an event that can be canceled before any modifications or actions are performed.
	/// This is useful for letting subscribers know that an event will happen, but gives them the chance to prevent it.
	/// </summary>
	public class CancellableEventArgs : EventArgs, ICancellable
	{
		private volatile bool _cancelled;

		/// <summary>
		/// Indicates that the event has been canceled
		/// </summary>
		public bool IsCanceled
		{
			get { return _cancelled; }
		}

		/// <summary>
		/// Cancels the event to prevent its outcome from occurring
		/// </summary>
		public void Cancel ()
		{
			_cancelled = true;
		}
	}
}
