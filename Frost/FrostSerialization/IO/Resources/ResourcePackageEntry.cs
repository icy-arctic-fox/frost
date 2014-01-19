using System;
using Frost.IO.Tnt;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Information about an entry in a resource package.
	/// This information is contained in the header of a resource package.
	/// </summary>
	public class ResourcePackageEntry : INodeMarshal
	{
		private readonly Guid _id;
		
		/// <summary>
		/// Globally unique ID of the resource.
		/// Every resource in a single <see cref="ResourcePackage"/> has a unique ID.
		/// </summary>
		/// <remarks>Even if a resource is replaced (i.e. a mod),
		/// the ID of the resource should be different everywhere.</remarks>
		public Guid Id
		{
			get { return _id; }
		}

		private readonly string _name;

		/// <summary>
		/// Name of the resource.
		/// Every resource in a single <see cref="ResourcePackage"/> has a unique name.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		private readonly int _offset;

		/// <summary>
		/// Block offset in the data section that contains the start of the resource data
		/// </summary>
		public int BlockOffset
		{
			get { return _offset; }
		}

		private readonly int _size;

		/// <summary>
		/// Size of the resource (compressed and encrypted) in bytes
		/// </summary>
		public int Size
		{
			get { return _size; }
		}

		// TODO: Add optional property for checksum (sha256)
		// TODO: Add optional property for encryption key

		/// <summary>
		/// Creates information about a resource package entry
		/// </summary>
		/// <param name="id">Unique ID of the resource</param>
		/// <param name="name">Name of the resource</param>
		/// <param name="offset">Block offset to where the resource's data starts</param>
		/// <param name="size">Size in bytes of the resource</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null.
		/// The name of the resource can't be null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> or <paramref name="size"/> are negative.
		/// The block offset and resource size can't be negative.</exception>
		public ResourcePackageEntry (Guid id, string name, int offset, int size)
		{
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource can't be null.");
			if(offset < 0)
				throw new ArgumentOutOfRangeException("offset", "The block offset can't be negative.");
			if(size < 0)
				throw new ArgumentOutOfRangeException("size", "The size of the resource can't be negative.");

			_id     = id;
			_name   = name;
			_offset = offset;
			_size   = size;
		}

		#region Node marshal

		private const string IdNodeName     = "id";
		private const string NameNodeName   = "name";
		private const string OffsetNodeName = "offset";
		private const string SizeNodeName   = "size";

		/// <summary>
		/// Creates information about a resource package entry by extracting it from a node.
		/// The node passed as <paramref name="node"/> should be the same format as a node returned by <see cref="ToNode"/>.
		/// </summary>
		/// <param name="node">Node that contains information about the resource package entry</param>
		public ResourcePackageEntry (Node node)
		{
			var root = node.ExpectComplexNode();
			_id      = root.ExpectGuidNode(IdNodeName);
			_name    = root.ExpectStringNode(NameNodeName);
			_offset  = root.ExpectIntNode(OffsetNodeName);
			_size    = root.ExpectIntNode(SizeNodeName);
		}

		/// <summary>
		/// Packs information about the resource into a node
		/// </summary>
		/// <returns>A node containing information about the resource</returns>
		public Node ToNode ()
		{
			return new ComplexNode {
				{IdNodeName,     new GuidNode(_id)},
				{NameNodeName,   new StringNode(_name)},
				{OffsetNodeName, new IntNode(_offset)},
				{SizeNodeName,   new IntNode(_size)}
			};
		}
		#endregion
	}
}
