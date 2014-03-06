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
		/// <param name="stateIndex">Index of the state of the entity to draw</param>
		/// <param name="t">Amount of interpolation to apply</param>
		void DrawEntity (Entity e, int stateIndex, double t);
	}
}
