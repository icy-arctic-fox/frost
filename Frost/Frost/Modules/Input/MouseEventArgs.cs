using System;
using Frost.Geometry;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Describes a mouse event
	/// </summary>
	/// <remarks>Properties in this event can be modified internally.
	/// This allows the instance to be reused since mouse events occur frequently.</remarks>
	public class MouseEventArgs : EventArgs
	{
		/// <summary>
		/// Location of the mouse on the screen when the event occurred
		/// </summary>
		public Point2i Position { get; internal set; }

		/// <summary>
		/// Mouse buttons that were pressed during the event
		/// </summary>
		public MouseButton Buttons { get; internal set; }

		/// <summary>
		/// Creates a new mouse event with default values
		/// </summary>
		public MouseEventArgs ()
		{
			Position = Point2i.Origin;
			Buttons  = MouseButton.None;
		}

		/// <summary>
		/// Creates a new mouse event
		/// </summary>
		/// <param name="position">Location of the mouse on the screen when the event occurred</param>
		/// <param name="buttons">Mouse buttons that were pressed during the event</param>
		public MouseEventArgs (Point2i position, MouseButton buttons)
		{
			Position = position;
			Buttons  = buttons;
		}
	}
}
