using System;
using SFML.Window;
using Frost.Utility;
using M = SFML.Window.Mouse;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Mouse position and status
	/// </summary>
	public static class Mouse
	{
		#region Position

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
		public static Point2D Position
		{
			get { return M.GetPosition(); }
			set { M.SetPosition(value); }
		}
		#endregion

		#region Buttons

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
		#endregion

		#region Events
		#region Move

		/// <summary>
		/// Triggered when the mouse moves
		/// </summary>
		public static event EventHandler<MouseEventArgs> Move;

		/// <summary>
		/// This method should be called from <see cref="InputModule"/> when a mouse move is detected
		/// </summary>
		/// <param name="args">Event arguments</param>
		internal static void OnMove (MouseEventArgs args)
		{
			Move.NotifySubscribers(null, args);
		}
		#endregion

		#region Press

		/// <summary>
		/// Triggered when a mouse button is initially pressed down
		/// </summary>
		public static event EventHandler<MouseEventArgs> Press;

		/// <summary>
		/// This method should be called from <see cref="InputModule"/> when a mouse button initially becomes pressed
		/// </summary>
		/// <param name="args">Event arguments</param>
		internal static void OnPress (MouseEventArgs args)
		{
			Press.NotifySubscribers(null, args);
		}
		#endregion

		#region Release

		/// <summary>
		/// Triggered when a mouse button is released
		/// </summary>
		public static event EventHandler<MouseEventArgs> Release;

		/// <summary>
		/// This method should be called from <see cref="InputModule"/> when a mouse button becomes released
		/// </summary>
		/// <param name="args">Event arguments</param>
		internal static void OnRelease (MouseEventArgs args)
		{
			Release.NotifySubscribers(null, args);
		}
		#endregion

		#region Click

		/// <summary>
		/// Triggered when a mouse button is clicked (pressed and released)
		/// </summary>
		public static event EventHandler<MouseEventArgs> Click;

		/// <summary>
		/// This method should be called from <see cref="InputModule"/> when a mouse click is detected
		/// </summary>
		/// <param name="args">Event arguments</param>
		internal static void OnClick (MouseEventArgs args)
		{
			Click.NotifySubscribers(null, args);
		}
		#endregion
		#endregion
	}
}
