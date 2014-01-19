using System;
using System.Collections.Generic;
using System.IO;
using Frost.IO.Tnt;
using Ionic.Zlib;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Creates resource package files
	/// </summary>
	public class ResourcePackageWriter : ResourcePackage
	{
		/// <summary>
		/// Current resource package version
		/// </summary>
		private const int CurrentVersion = 1;

		/// <summary>
		/// Writer used to put data into the file
		/// </summary>
		private readonly BinaryWriter _bw;

		/// <summary>
		/// Opens a resource package file to start storing resources into it
		/// </summary>
		/// <param name="filepath">Path to the resource file</param>
		/// <param name="blockSize">Size (in kilobytes) of each block minus 1 (0 means 1 KB)</param>
		/// <param name="opts">Options for the resource package file</param>
		/// <remarks>The file will remain open until <see cref="Close"/> or <see cref="Dispose"/> is called.</remarks>
		public ResourcePackageWriter (string filepath, byte blockSize = 3, ResourcePackageOptions opts = ResourcePackageOptions.None)
		{
			// Create the file stream
			FileStream = new FileStream(filepath, FileMode.Create);
			_bw        = new EndianBinaryWriter(FileStream, Endian.Big);

			// Write the file header info
			var info  = new HeaderInfo(CurrentVersion, opts, blockSize);
			BlockSize = (blockSize + 1) * Kilobyte; // +1 to make 0 mean 1 KB block size
			Options   = opts;
			writeFileInfo(_bw, info);

			// The header is created when Write() is called
		}

		#region Resource management

		private int _curOffset; // Current block offset in the file
		private readonly List<byte[]> _packedResources = new List<byte[]>();

		/// <summary>
		/// Packs data so that it is in the form that will be written to the package
		/// </summary>
		/// <param name="data">Unpacked data</param>
		/// <returns>Packed data</returns>
		private static byte[] packData (byte[] data)
		{
			using(var ms = new MemoryStream())
			{
				using(var ds = new DeflateStream(ms, CompressionMode.Compress))
					ds.Write(data, 0, data.Length);
				return ms.ToArray();
			}
		}

		/// <summary>
		/// Adds a resource to the package
		/// </summary>
		/// <param name="id">Globally unique ID of the resource</param>
		/// <param name="name">Name of the resource</param>
		/// <param name="data">Data contained in the resource</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> or <paramref name="data"/> are null</exception>
		/// <exception cref="ArgumentException">Thrown if a resource by the name <paramref name="name"/> has already been added</exception>
		public void Add (Guid id, string name, byte[] data) // TODO: Add encryption and other options
		{
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource can't be null.");
			if(data == null)
				throw new ArgumentNullException("data", "The raw data for the resource can't be null.");
			if(Entries.ContainsKey(name)) // TODO: Lock properly
				throw new ArgumentException("A resource by the same name already exists.", "name");

			var packedData = packData(data);
			var size = packedData.Length;
			var blockCount = size / BlockSize;
			if(blockCount % BlockSize != 0)
				++blockCount; // Round up
			var offset  = _curOffset;
			_curOffset += blockCount;
			var entry = new ResourcePackageEntry(id, name, offset, packedData.Length);
			_packedResources.Add(packedData);
			Entries.Add(name, entry);
		}
		#endregion

		#region IO
		#region Save

		/// <summary>
		/// Writes the resource package header to the file
		/// </summary>
		/// <param name="bw">Writer used to put data into the file</param>
		/// <param name="info">Raw header information</param>
		private static void writeFileInfo (BinaryWriter bw, HeaderInfo info)
		{
			bw.Write(info.Version);
			bw.Write((ushort)info.Options);
			bw.Write(info.KbCount);
		}

		/// <summary>
		/// Constructs the node container that contains information about all of the resource entries in the package
		/// </summary>
		/// <param name="entries">Information about resources in the package</param>
		/// <returns>A node container that contains the resource information</returns>
		private static NodeContainer constructEntriesHeader (IEnumerable<ResourcePackageEntry> entries)
		{
			var root = new ComplexNode();
			// TODO: Add package file name, description, and creator fields
			var list = new ListNode(NodeType.Complex);
			foreach(var entry in entries)
				list.Add(entry.ToNode());
			root.Add("entries", list);
			return new NodeContainer(root);
		}

		/// <summary>
		/// Writes the header of the package which contains information about all of the resources
		/// </summary>
		private void writeHeader ()
		{
			// Write the file header first
			SeekBlock(0); // Go to the start of the file
			var info = new HeaderInfo(CurrentVersion, Options, ((byte)((BlockSize / Kilobyte) - 1)));
			writeFileInfo(_bw, info);

			// Write the resource entries
			var container = constructEntriesHeader(Entries.Values);
			container.WriteToStream(_bw); // TODO: Handle encryption of the header

			// Update the data offset and pad the block
			var headerSize = (int)FileStream.Position;
			var blockCount = headerSize / BlockSize;
			if(headerSize % BlockSize != 0)
			{// Need to pad the current block
				++blockCount;
				var totalSize = blockCount * BlockSize;
				var padSize   = totalSize - headerSize;
				var padding   = new byte[padSize]; // TODO: Fill this with random garbage if the header is encrypted
				_bw.Write(padding, 0, padSize);
			}
		}

		/// <summary>
		/// Writes a single resource out to the package
		/// </summary>
		/// <param name="resource">Packed resource data</param>
		private void writeResource (byte[] resource)
		{
			var startPos = FileStream.Position;
			_bw.Write(resource, 0, resource.Length);
			var endPos = FileStream.Position;
			var size = endPos - startPos;
			var blockCount = size / BlockSize;
			if(size % BlockSize != 0)
			{// Need to pad the current block
				++blockCount;
				var totalSize = blockCount * BlockSize;
				var padSize   = (int)(totalSize - size);
				var padding   = new byte[padSize]; // TODO: Fill this with random garbage if the resource is encrypted
				_bw.Write(padding, 0, padSize);
			}
		}

		/// <summary>
		/// Writes out all of the resources to the package file
		/// </summary>
		public void Flush ()
		{
			writeHeader();
			foreach(var resource in _packedResources)
				writeResource(resource);
		}
		#endregion

		/// <summary>
		/// Closes the resource package file.
		/// No more operations to the contents of the package will be allowed for this instance.
		/// </summary>
		public override void Close ()
		{
			Flush();
			_bw.Close();
		}
		#endregion

		/// <summary>
		/// Disposes of the resource package
		/// </summary>
		/// <param name="disposing">True if inner-resources should be disposed of (<see cref="Dispose"/> was called)</param>
		protected override void Dispose (bool disposing)
		{
			if(!Disposed)
			{
				Disposed = true;
				Close();
			}
		}
	}
}
