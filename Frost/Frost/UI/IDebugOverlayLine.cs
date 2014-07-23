using System;

namespace Frost.UI
{
	/// <summary>
	/// An object that can display a single line of information in a debug overlay
	/// </summary>
	public interface IDebugOverlayLine
	{
		/// <summary>
		/// Event that is triggered when the object producing the information is being disposed.
		/// This is used to indicate when the line should be removed from the overlay.
		/// </summary>
		event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Retrieves the contents of the debug overlay line
		/// </summary>
		/// <param name="contents">String to store the debug information in</param>
		void GetDebugInfo (MutableString contents);
	}
}
