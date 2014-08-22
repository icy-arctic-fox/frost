namespace Frost.Tnt
{
	/// <summary>
	/// An object that can marshal itself into a <see cref="Node"/> and extract itself from one as well.
	/// Classes implementing this interface should also have a constructor that accepts a node created by <see cref="ToNode"/>.
	/// </summary>
	public interface INodeMarshal
	{
		/// <summary>
		/// Creates a node from the contents of the object
		/// </summary>
		/// <returns>A node</returns>
		Node ToTntNode ();
	}
}
