using System;
using Frost.Utility;

namespace Frost.UI
{
	/// <summary>
	/// Debug overlay line that displays frame rate information from a <see cref="GameRunner"/>
	/// </summary>
	public class FrameDebugOverlayLine : IDebugOverlayLine
	{
		private readonly GameRunner _runner;

		/// <summary>
		/// Creates a new frame debug overlay line
		/// </summary>
		/// <param name="runner">Game runner to display information for</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="runner"/> is null</exception>
		public FrameDebugOverlayLine (GameRunner runner)
		{
			if(runner == null)
				throw new ArgumentNullException("runner", "The game runner can't be null.");

			_runner = runner;
			_runner.Disposing += _runner_Disposing;
		}

		/// <summary>
		/// Called when the game runner is disposing and the overlay line should be removed
		/// </summary>
		/// <param name="sender">Game runner</param>
		/// <param name="e">Event arguments</param>
		private void _runner_Disposing (object sender, EventArgs e)
		{
			Disposing.NotifyThreadedSubscribers(this, e);
		}

		/// <summary>
		/// Triggered when the game runner is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Generates the text displayed in the debug overlay
		/// </summary>
		/// <returns>Runner information</returns>
		public override string ToString ()
		{
			return _runner.ToString();
		}
	}
}
