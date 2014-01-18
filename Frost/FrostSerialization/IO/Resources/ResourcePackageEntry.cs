using System;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Information about an entry in a resource package.
	/// This information is contained in the header of a resource package.
	/// </summary>
	public class ResourcePackageEntry
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

		private readonly long _size;

		/// <summary>
		/// Size of the resource (uncompressed and unencrypted) in bytes
		/// </summary>
		public long Size
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
		public ResourcePackageEntry (Guid id, string name, int offset, long size)
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
	}
}
