namespace Frost.Logic
{
	/// <summary>
	/// A node that can update the state of a component step by step
	/// </summary>
	public interface IStepableNode
	{
		/// <summary>
		/// Updates the state of the component by a single step
		/// </summary>
		/// <param name="prev">Index of the previous state that was updated</param>
		/// <param name="next">Index of the next state that should be updated</param>
		/// <remarks>The only game state that should be modified during this process is the state indicated by <paramref name="next"/>.
		/// The state indicated by <paramref name="prev"/> can be used for reference (if needed), but should not be modified.
		/// Modifying any other game state info during this process would corrupt the game state.</remarks>
		void StepState (int prev, int next);
	}
}
