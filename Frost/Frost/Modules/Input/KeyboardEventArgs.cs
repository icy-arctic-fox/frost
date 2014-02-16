using System;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Describes a keyboard event
	/// </summary>
	/// <remarks>Properties in this event can be modified internally.
	/// This allows the instance to be reused since keyboard events can occur frequently.</remarks>
	public class KeyboardEventArgs : EventArgs
	{
		/// <summary>
		/// Keyboard key that pertains to the event
		/// </summary>
		public Key Key { get; internal set; }

		/// <summary>
		/// Modifier keys that were pressed with the key
		/// </summary>
		public ModifierKey Modifiers { get; internal set; }

		/// <summary>
		/// Creates a new keyboard event
		/// </summary>
		/// <param name="key">Keyboard key that was pressed or released during the event</param>
		public KeyboardEventArgs (Key key)
		{
			Key = key;
		}
	}
}
