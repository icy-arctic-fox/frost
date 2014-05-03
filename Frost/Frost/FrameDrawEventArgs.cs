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
		/// Index of the previously rendered state
		/// </summary>
		/// <remarks>The state that this index refers to cannot be used to retrieve information from.
		/// An update process may be changing the state.</remarks>
		public int PreviousStateIndex { get; internal set; }

		/// <summary>
		/// Index of the state to draw
		/// </summary>
		public int StateIndex { get; internal set; }

		/// <summary>
		/// Indicates whether a frame is rendered more than once
		/// </summary>
		public bool Duplicate
		{
			get { return PreviousStateIndex == StateIndex; }
		}

		/// <summary>
		/// Fraction of time that has passed since the last frame update and the next one.
		/// This is a number from 0 to 1, but can be greater than 1 if the renderer is ahead of game updates.
		/// </summary>
		public double Interpolation { get; internal set; }

		/// <summary>
		/// Indicates whether game updates are falling behind of the target update rate
		/// </summary>
		public bool IsRunningSlow { get; internal set; }
	}
}
