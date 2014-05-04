namespace Frost.Entities
{
	/// <summary>
	/// Processes multiple entities during a logic update by using their components
	/// </summary>
	public interface IUpdateSubsystem : ISubsystem
	{
		/// <summary>
		/// Processes a single entity
		/// </summary>
		/// <param name="entity">Entity to process</param>
		/// <param name="args">Update information</param>
		void Process (Entity entity, FrameStepEventArgs args);
	}
}
