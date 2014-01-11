using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Wraps a typed-node structure.
	/// This class is used to serialize and deserialize a node structure.
	/// </summary>
	public class NodeContainer
	{
		/// <summary>
		/// Current version
		/// </summary>
		public const int TypedNodeVersion = 1;

		private readonly Node _root;

		/// <summary>
		/// Root node in the container
		/// </summary>
		public Node Root
		{
			get { return _root; }
		}

		private readonly int _ver;

		/// <summary>
		/// Version of the nodes used in the structure
		/// </summary>
		public int Version
		{
			get { return _ver; }
		}

		/// <summary>
		/// Creates a new node container
		/// </summary>
		/// <param name="root">Root node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="root"/> is null.
		/// The root node of the container can't be null.</exception>
		public NodeContainer (Node root)
		{
			if(root == null)
				throw new ArgumentNullException("root", "The root node of the container can't be null.");
			_root = root;
			_ver  = TypedNodeVersion;
		}
	}
}
