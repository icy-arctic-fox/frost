namespace Frost
{
	/// <summary>
	/// An object that can update the state of a component step by step
	/// </summary>
	public interface IStepable
	{
		/// <summary>
		/// Updates the state of the component by a single step
		/// </summary>
		/// <param name="args">Update information</param>
		/// <remarks>The only game state that should be modified during this process is the state indicated by <see cref="FrameStepEventArgs.NextStateIndex"/>.
		/// The state indicated by <see cref="FrameStepEventArgs.PreviousStateIndex"/> can be used for reference (if needed), but should not be modified.
		/// Modifying any other game state info during this process could corrupt the game state.</remarks>
		void Step (FrameStepEventArgs args);
	}
}
