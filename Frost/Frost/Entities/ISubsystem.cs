namespace Frost.Entities
{
	/// <summary>
	/// Processes multiple entities by using their components
	/// </summary>
	public interface ISubsystem
	{
		/// <summary>
		/// Determines whether the subsystem can process the entity
		/// </summary>
		/// <param name="entity">Entity to check</param>
		/// <returns>True if the entity can be processed by the subsystem</returns>
		bool CanProcess (Entity entity);
	}
}
