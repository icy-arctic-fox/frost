using System;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Very basic controller that reports when any input is detected
	/// </summary>
	public interface IControllerBase
	{
		/// <summary>
		/// Triggered when any input is initially detected (such as a key being pressed)
		/// </summary>
		event EventHandler<InputEventArgs> InputStarted;

		/// <summary>
		/// Triggered when any input stops (such as a key being released)
		/// </summary>
		event EventHandler<InputEventArgs> InputEnded;
	}
}
