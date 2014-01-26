using System;
using SFML.Window;
using M = SFML.Window.Mouse;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Mouse position and status
	/// </summary>
	public static class Mouse
	{
		/// <summary>
		/// Location of the mouse on the screen along the x-axis
		/// </summary>
		public static int X
		{
			get { return M.GetPosition().X; }
			set { M.SetPosition(new Vector2i(value, Y)); }
		}

		/// <summary>
		/// Location of the mouse on the screen along the y-axis
		/// </summary>
		public static int Y
		{
			get { return M.GetPosition().Y; }
			set { M.SetPosition(new Vector2i(X, value)); }
		}

		/// <summary>
		/// Location of the mouse on the screen
		/// </summary>
		public static int Position
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Buttons currently being pressed
		/// </summary>
		public static MouseButton Buttons
		{
			get
			{
				var buttons = MouseButton.None;
				if(M.IsButtonPressed(M.Button.Left))
					buttons |= MouseButton.Left;
				if(M.IsButtonPressed(M.Button.Right))
					buttons |= MouseButton.Right;
				if(M.IsButtonPressed(M.Button.Middle))
					buttons |= MouseButton.Middle;
				return buttons;
			}
		}

		/// <summary>
		/// Indicates whether the left mouse button is being pressed
		/// </summary>
		public static bool Left
		{
			get { return M.IsButtonPressed(M.Button.Left); }
		}

		/// <summary>
		/// Indicates whether the right mouse button is being pressed
		/// </summary>
		public static bool Right
		{
			get { return M.IsButtonPressed(M.Button.Right); }
		}

		/// <summary>
		/// Indicates whether the middle mouse button is being pressed
		/// </summary>
		public static bool Middle
		{
			get { return M.IsButtonPressed(M.Button.Middle); }
		}
	}
}
