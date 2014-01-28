using System;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Values for mouse buttons
	/// </summary>
	[Flags]
	public enum MouseButton
	{
		/// <summary>
		/// No mouse buttons are pressed
		/// </summary>
		None = 0x00,

		/// <summary>
		/// Left mouse button
		/// </summary>
		Left = 0x01,

		/// <summary>
		/// Right mouse button
		/// </summary>
		Right = 0x02,

		/// <summary>
		/// Middle mouse button
		/// </summary>
		Middle = 0x04,

		// TODO: Add other two extra buttons

		/// <summary>
		/// Number of detectable mouse buttons
		/// </summary>
		Count = 4
	}
}
