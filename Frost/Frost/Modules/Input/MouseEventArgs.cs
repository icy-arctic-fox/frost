using System;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Describes a mouse event
	/// </summary>
	public class MouseEventArgs : EventArgs
	{
		/// <summary>
		/// Mouse buttons that were pressed during the event
		/// </summary>
		public MouseButton Buttons { get; internal set; }
	}
}
