namespace Frost
{
	/// <summary>
	/// Able to perform a logic update to step forward to the next frame
	/// </summary>
	public interface IFrameUpdate
	{
		/// <summary>
		/// Updates the state by a single step, effectively moving it to the next frame
		/// </summary>
		/// <param name="args">Update information</param>
		/// <remarks>The only game state that should be modified during this process is the state indicated by <see cref="FrameStepEventArgs.NextStateIndex"/>.
		/// The state indicated by <see cref="FrameStepEventArgs.PreviousStateIndex"/> can be used for reference (if needed), but should not be modified.
		/// Modifying any other game state info during this process could corrupt the game state.</remarks>
		void Update (FrameStepEventArgs args);
	}
}
