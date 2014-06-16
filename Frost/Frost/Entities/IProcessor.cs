namespace Frost.Entities
{
	/// <summary>
	/// Processes multiple entities by fetching information from their components
	/// </summary>
	public interface IProcessor
	{
		/// <summary>
		/// Determines whether the processor can handle the entity
		/// </summary>
		/// <param name="entity">Entity to check</param>
		/// <returns>True if the entity can be processed by the processor</returns>
		bool CanProcess (Entity entity);
	}
}
