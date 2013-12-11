namespace Frost.Modules
{
	/// <summary>
	/// A node that can draw the state of a component
	/// </summary>
	public interface IDrawableNode
	{
		/// <summary>
		/// Draws the state of a component
		/// </summary>
		/// <param name="state">Index of the state to draw</param>
		/// <remarks>None of the game states should be modified by this process - including the state indicated by <paramref name="state"/>.
		/// Modifying the game state info during this process would corrupt the game state.</remarks>
		void DrawState (int state);
	}
}
