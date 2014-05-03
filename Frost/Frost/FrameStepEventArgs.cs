using System;

namespace Frost
{
	/// <summary>
	/// Information for game logic update each frame
	/// </summary>
	/// <remarks>The contents of the class are internally mutable so that a single instance can be reused.</remarks>
	public class FrameStepEventArgs : EventArgs
	{
		/// <summary>
		/// Index of the state that was updated last frame
		/// </summary>
		public int PreviousStateIndex { get; internal set; }

		/// <summary>
		/// Index of the state to update this frame
		/// </summary>
		public int NextStateIndex { get; internal set; }

		/// <summary>
		/// Current frame number
		/// </summary>
		public ulong Frame { get; internal set; }

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
