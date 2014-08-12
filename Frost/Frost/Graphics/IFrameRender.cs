namespace Frost.Graphics
{
	/// <summary>
	/// Able to render a state to a visual frame
	/// </summary>
	/// <seealso cref="FrameDrawEventArgs"/>
	public interface IFrameRender
	{
		/// <summary>
		/// Renders the frame using provided state information
		/// </summary>
		/// <param name="args">Render information</param>
		/// <remarks>None of the game states should be modified by this process - including the state indicated by <see cref="FrameDrawEventArgs.StateIndex"/>.
		/// Modifying the game state info during this process would corrupt the game state.</remarks>
		void Render (FrameDrawEventArgs args);
	}
}
