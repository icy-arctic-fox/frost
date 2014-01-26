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
	}
}
