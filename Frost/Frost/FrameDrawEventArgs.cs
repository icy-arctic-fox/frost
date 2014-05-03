using System;

namespace Frost
{
	/// <summary>
	/// Information for rendering each frame
	/// </summary>
	/// <remarks>The contents of the class are internally mutable so that a single instance can be reused.</remarks>
	public class FrameDrawEventArgs : EventArgs
	{
		/// <summary>
		/// Index of the state to draw
		/// </summary>
		public int StateIndex { get; internal set; }

		/// <summary>
		/// Frame number currently being drawn
		/// </summary>
		public long FrameNumber { get; internal set; }

		/// <summary>
		/// Fraction of time that has passed since the last frame update and the next one.
		/// This is a number from 0 to 1, but can be greater than 1 if the renderer is ahead of game updates.
		/// </summary>
		public double Interpolation { get; internal set; }

		/// <summary>
		/// Length of time that the game has been running for
		/// </summary>
		public TimeSpan GameTime { get; internal set; }

		/// <summary>
		/// Indicates whether game updates are falling behind of the target update rate
		/// </summary>
		public bool IsRunningSlow { get; internal set; }
	}
}
