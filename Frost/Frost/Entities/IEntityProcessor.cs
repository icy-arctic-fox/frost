namespace Frost.Entities
{
	/// <summary>
	/// Processes multiple entities each frame
	/// </summary>
	public interface IEntityProcessor
	{
		/// <summary>
		/// Processes a single entity
		/// </summary>
		/// <param name="e">Entity to process</param>
		void ProcessEntity (Entity e);
	}
}
