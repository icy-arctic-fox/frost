namespace Frost.Entities
{
	/// <summary>
	/// Empty interface that serves as a base for all entity component types
	/// </summary>
	public interface IEntityComponent
	{
		/// <summary>
		/// Creates a copy of the information in the component
		/// </summary>
		/// <returns>Copy of the component</returns>
		IEntityComponent CloneComponent ();
	}
}
