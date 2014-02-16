using System;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Collection of modifier keys
	/// </summary>
	[Flags]
	public enum ModifierKey
	{
		/// <summary>
		/// No modifiers are being pressed
		/// </summary>
		None = 0x00,

		/// <summary>
		/// Shift key
		/// </summary>
		Shift = 0x01,

		/// <summary>
		/// Alt key
		/// </summary>
		Alt = 0x02,

		/// <summary>
		/// Control key
		/// </summary>
		Control = 0x04,

		/// <summary>
		/// System (Windows logo) key
		/// </summary>
		System = 0x08
	}
}
