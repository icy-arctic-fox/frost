using System;
using Frost.Utility;
using K = SFML.Window.Keyboard;

namespace Frost.Modules.Input
{
	/// <summary>
	/// State of the player's keyboard
	/// </summary>
	public static class Keyboard
	{
		/// <summary>
		/// Checks if a key is currently being pressed
		/// </summary>
		/// <param name="key">Key to check for</param>
		/// <returns>True if <paramref name="key"/> is being pressed</returns>
		public static bool IsKeyPressed (Key key)
		{
			return K.IsKeyPressed((K.Key)key);
		}

		#region Events
		#region KeyPress

		/// <summary>
		/// Triggered when a key is initially pressed on the keyboard
		/// </summary>
		public static event EventHandler<KeyboardEventArgs> KeyPress;

		/// <summary>
		/// This method should be called from <see cref="InputModule"/> when a key press is detected
		/// </summary>
		/// <param name="args">Event arguments</param>
		internal static void OnKeyPress (KeyboardEventArgs args)
		{
			KeyPress.NotifySubscribers(null, args);
		}
		#endregion

		#region KeyRelease

		/// <summary>
		/// Triggered when a key is released on the keyboard
		/// </summary>
		public static event EventHandler<KeyboardEventArgs> KeyRelease;

		/// <summary>
		/// This method should be called from <see cref="InputModule"/> when a key release is detected
		/// </summary>
		/// <param name="args">Event arguments</param>
		internal static void OnKeyRelease (KeyboardEventArgs args)
		{
			KeyRelease.NotifySubscribers(null, args);
		}
		#endregion
		#endregion
	}
}
