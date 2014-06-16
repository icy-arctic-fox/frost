namespace Frost.Entities
{
	/// <summary>
	/// Processes multiple entities during a frame render by using their components
	/// </summary>
	public interface IRenderProcessor : IProcessor
	{
		/// <summary>
		/// Processes a single entity
		/// </summary>
		/// <param name="entity">Entity to process</param>
		/// <param name="args">Render information</param>
		void Process (Entity entity, FrameDrawEventArgs args);
	}
}
