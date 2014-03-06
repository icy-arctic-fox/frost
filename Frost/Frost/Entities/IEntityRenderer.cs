using Frost.Graphics;

namespace Frost.Entities
{
	/// <summary>
	/// Processes multiple entities each frame
	/// </summary>
	public interface IEntityRenderer
	{
		/// <summary>
		/// Processes a single entity
		/// </summary>
		/// <param name="e">Entity to process</param>
		/// <param name="target">Target to draw the entity to</param>
		/// <param name="stateIndex">Index of the state of the entity to draw</param>
		/// <param name="t">Amount of interpolation to apply</param>
		void DrawEntity (Entity e, IRenderTarget target, int stateIndex, double t);
	}
}
