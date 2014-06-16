namespace Frost.Entities
{
	/// <summary>
	/// Base for all entity components
	/// </summary>
	public interface IComponent
	{
		/// <summary>
		/// Creates a copy of the information in the component
		/// </summary>
		/// <returns>Copy of the component</returns>
		IComponent CloneComponent ();
	}
}
