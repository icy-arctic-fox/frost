using System;

namespace Frost
{
	/// <summary>
	/// Additional information about a disposable object
	/// </summary>
	public interface IFullDisposable : IDisposable
	{
		/// <summary>
		/// Indicates whether the object has been disposed
		/// </summary>
		bool Disposed { get; }

		/// <summary>
		/// Triggered when the object is being disposed
		/// </summary>
		event EventHandler<EventArgs> Disposing;
	}
}
