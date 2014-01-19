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
		/// <param name="opts">Options for the resource package file</param>
		/// <remarks>The file will remain open until <see cref="Close"/> or <see cref="Dispose"/> is called.</remarks>
		public ResourcePackageWriter (string filepath, ResourcePackageOptions opts = ResourcePackageOptions.None)
		{
			// Create the file stream
			FileStream = new FileStream(filepath, FileMode.Create);
			_bw        = new EndianBinaryWriter(FileStream, Endian.Big);

			// Write the file header info
			var info = new HeaderInfo(CurrentVersion, opts);
			Version = CurrentVersion;
			Options = opts;
			writeFileInfo(_bw, info);

			// The header is created when Write() is called
		}

		#region Resource management

		private long _curOffset; // Current offset in the file from the header
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
			var size    = packedData.Length;
			var offset  = _curOffset;
			_curOffset += size;

			var entry = new ResourcePackageEntry(id, name, offset, packedData.Length);
			Entries.Add(name, entry);
			_packedResources.Add(packedData);
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
			bw.Write((byte)0); // Unused byte
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
			FileStream.Seek(0L, SeekOrigin.Begin); // Go to the start of the file
			var info = new HeaderInfo(Version, Options);
			writeFileInfo(_bw, info);

			// Write the resource entries
			var container = constructEntriesHeader(Entries.Values);
			container.WriteToStream(_bw); // TODO: Handle encryption of the header
		}

		/// <summary>
		/// Writes all of the resources to the package
		/// </summary>
		private void writeResources ()
		{
			foreach(var resource in _packedResources)
				_bw.Write(resource, 0, resource.Length);
		}

		/// <summary>
		/// Writes out all of the resources to the package file
		/// </summary>
		public void Flush ()
		{
			writeHeader();
			writeResources();
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
