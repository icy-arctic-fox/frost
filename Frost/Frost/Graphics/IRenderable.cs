using Frost.Display;

namespace Frost.Graphics
{
	/// <summary>
	/// Component that can be drawn
	/// </summary>
	public interface IRenderable
	{
		/// <summary>
		/// Draws the state of a component
		/// </summary>
		/// <param name="display">Display to draw the object's state onto</param>
		/// <param name="args">Render information</param>
		/// <remarks>None of the game states should be modified by this process - including the state indicated by <see cref="FrameDrawEventArgs.StateIndex"/>.
		/// Modifying the game state info during this process would corrupt the game state.</remarks>
		void Draw (IDisplay display, FrameDrawEventArgs args);
	}
}
