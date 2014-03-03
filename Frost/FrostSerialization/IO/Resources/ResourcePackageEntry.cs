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

		private readonly long _offset;

		/// <summary>
		/// Offset in bytes from the package header to where the packed resource data starts
		/// </summary>
		public long Offset
		{
			get { return _offset; }
		}

		private readonly int _size;

		/// <summary>
		/// Size of the packed resource data in bytes
		/// </summary>
		public int Size
		{
			get { return _size; }
		}

		// TODO: Add optional property for checksum (sha256)

		private readonly string _secret;

		/// <summary>
		/// Symmetric key and IV used to encrypt and decrypt the resource
		/// </summary>
		/// <remarks>This property can be null.
		/// A null value signifies that the resource is not encrypted.</remarks>
		/// <seealso cref="Encrypted"/>
		public string Secret
		{
			get { return _secret; }
		}

		/// <summary>
		/// Indicates whether the resource is encrypted with a key
		/// </summary>
		/// <seealso cref="Secret"/>
		public bool Encrypted
		{
			get { return _secret != null; }
		}

		/// <summary>
		/// Creates information about a resource package entry
		/// </summary>
		/// <param name="id">Unique ID of the resource</param>
		/// <param name="name">Name of the resource</param>
		/// <param name="offset">Block offset to where the resource's data starts</param>
		/// <param name="size">Size in bytes of the resource</param>
		/// <param name="secret">Key and IV used for symmetrical encryption</param>
		/// <exception cref="ArgumentNullException">The <paramref name="name"/> of the resource can't be null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> or <paramref name="size"/> are negative.
		/// The block offset and resource size can't be negative.</exception>
		public ResourcePackageEntry (Guid id, string name, long offset, int size, string secret = null)
		{
			if(name == null)
				throw new ArgumentNullException("name");
			if(offset < 0)
				throw new ArgumentOutOfRangeException("offset", "The block offset can't be negative.");
			if(size < 0)
				throw new ArgumentOutOfRangeException("size", "The size of the resource can't be negative.");

			_id     = id;
			_name   = name;
			_offset = offset;
			_size   = size;
			_secret = secret;
		}

		#region Node marshal

		private const string IdNodeName     = "id";
		private const string NameNodeName   = "name";
		private const string OffsetNodeName = "offset";
		private const string SizeNodeName   = "size";
		private const string SecretNodeName = "secret";

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
			_offset  = root.ExpectLongNode(OffsetNodeName);
			_size    = root.ExpectIntNode(SizeNodeName);
			if(root.ContainsKey(SecretNodeName))
				_secret = root.ExpectStringNode(SecretNodeName);
		}

		/// <summary>
		/// Packs information about the resource into a node
		/// </summary>
		/// <returns>A node containing information about the resource</returns>
		public Node ToNode ()
		{
			var root = new ComplexNode {
				{IdNodeName,     new GuidNode(_id)},
				{NameNodeName,   new StringNode(_name)},
				{OffsetNodeName, new LongNode(_offset)},
				{SizeNodeName,   new IntNode(_size)}
			};
			if(_secret != null)
				root.Add(SecretNodeName, new StringNode(_secret));

			return root;
		}
		#endregion
	}
}
