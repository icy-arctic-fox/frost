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
		/// <remarks>This method triggers the <see cref="KeyPress"/> event.</remarks>
		internal static void OnKeyPress (KeyboardEventArgs args)
		{
			// Update modifiers
			var modifier = ToModifier(args.Key);
			if(modifier != ModifierKey.None)
			{
				Modifiers |= modifier;
				args.Modifiers = Modifiers;
			}

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
		/// <remarks>This method triggers the <see cref="KeyRelease"/> event.</remarks>
		internal static void OnKeyRelease (KeyboardEventArgs args)
		{
			// Update modifiers
			var modifier = ToModifier(args.Key);
			if(modifier != ModifierKey.None)
			{
				Modifiers &= ~modifier;
				args.Modifiers = Modifiers;
			}

			KeyRelease.NotifySubscribers(null, args);
		}
		#endregion
		#endregion

		#region Logic (detecting key modifiers and typed characters)

		/// <summary>
		/// Flags indicating which modifier keys are currently being pressed
		/// </summary>
		public static ModifierKey Modifiers { get; private set; }

		/// <summary>
		/// Creates a modifier key flag from a key
		/// </summary>
		/// <param name="key">Key to convert</param>
		/// <returns>A <see cref="ModifierKey"/> - <see cref="ModifierKey.None"/> will be returned if <paramref name="key"/> is not a modifier</returns>
		internal static ModifierKey ToModifier (this Key key)
		{
			switch(key)
			{
			case Key.LShift:
			case Key.RShift:
				return ModifierKey.Shift;
			case Key.LControl:
			case Key.RControl:
				return ModifierKey.Control;
			case Key.LAlt:
			case Key.RAlt:
				return ModifierKey.Alt;
			case Key.LSystem:
			case Key.RSystem:
				return ModifierKey.System;
			default:
				return ModifierKey.None;
			}
		}
		#endregion
	}
}
