using System;
using System.IO;
using Frost.IO.Tnt;
using Ionic.Zlib;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Provides access to existing resource package files
	/// </summary>
	public class ResourcePackageReader : ResourcePackage
	{
		/// <summary>
		/// Reader used to pull data from the file
		/// </summary>
		private readonly BinaryReader _br;

		/// <summary>
		/// Opens a resource package file to start pulling resources from it
		/// </summary>
		/// <param name="filepath">Path to the resource file</param>
		/// <exception cref="FileNotFoundException">Thrown if the resource package file wasn't found under <paramref name="filepath"/></exception>
		/// <exception cref="InvalidDataException">Thrown if the data contained in the resource file is invalid</exception>
		/// <remarks>The file will remain open until <see cref="Close"/> or <see cref="Dispose"/> is called.</remarks>
		public ResourcePackageReader (string filepath)
		{
			// Create the file stream
			FileStream = new FileStream(filepath, FileMode.Open);
			_br        = new EndianBinaryReader(FileStream, Endian.Big);

			// Read the file header info
			var fileInfo = readFileInfo(_br);
			Version = fileInfo.Version;
			Options = fileInfo.Options;
			Size    = new FileInfo(filepath).Length;

			// TODO: Implement header info encryption

			// Read the resource header information (meta-data for resources in the file)
			NodeContainer header;
			try
			{
				header = readHeader(_br);

				// Store information about the resource package
				var root = header.Root.ExpectComplexNode();
				Name        = root.ExpectStringNode("name");
				Creator     = root.ExpectStringNode("creator");
				Description = root.ExpectStringNode("description");
			}
			catch(FormatException e)
			{
				_br.Dispose();
				throw new InvalidDataException("The header data is in an unrecognized format.", e);
			}
			catch(InvalidDataException e)
			{
				_br.Dispose();
				throw new InvalidDataException("The header data is in an unrecognized format.", e);
			}

			// Calculate how big the header is (and where the data starts)
			DataOffset = FileStream.Position;

			// Pull entry information from the header
			extractHeaderEntries(header);
		}

		#region IO
		#region Load

		/// <summary>
		/// Reads resource package header information from the file
		/// </summary>
		/// <param name="br">Reader used to get data from the file</param>
		/// <returns>Raw header information</returns>
		private static HeaderInfo readFileInfo (BinaryReader br)
		{
			var ver  = br.ReadByte();
			var opts = (ResourcePackageOptions)br.ReadUInt16();
			br.ReadByte(); // Unused
			return new HeaderInfo(ver, opts);
		}

		/// <summary>
		/// Reads the header entries from the package header
		/// </summary>
		/// <param name="br">Reader used to get data from the file</param>
		/// <returns>Header data</returns>
		private static NodeContainer readHeader (BinaryReader br)
		{
			var headerSize = br.ReadInt32();
			var headerData = br.ReadBytes(headerSize);
			using(var ms = new MemoryStream(headerData))
			using(var ds = new DeflateStream(ms, CompressionMode.Decompress))
				return NodeContainer.ReadFromStream(ds);
		}

		/// <summary>
		/// Extracts resource entries from the header and adds them to the package's records
		/// </summary>
		/// <param name="header">Header to extract entries from</param>
		/// <exception cref="InvalidDataException">Thrown if the data contained in <paramref name="header"/> is in an unexpected format</exception>
		private void extractHeaderEntries (NodeContainer header)
		{
			try
			{
				var root = header.Root.ExpectComplexNode();
				// TODO: Capture package name, creator, and description
				var entries = root.ExpectListNode("entries", NodeType.Complex);
				foreach(var node in entries)
				{
					var entry = new ResourcePackageEntry(node);
					AddResource(entry);
				}
			}
			catch(Exception e)
			{
				throw new InvalidDataException("An error occurred while processing the package header", e);
			}
		}
		#endregion

		#region Access

		/// <summary>
		/// Gets a resource from the package by its name
		/// </summary>
		/// <param name="name">Name of the resource to retrieve</param>
		/// <returns>The data for the resource or null if no resource by the name <paramref name="name"/> exists</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public byte[] GetResource (string name)
		{
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource to retrieve can't be null.");

			ResourcePackageEntry entry;
			lock(Locker)
				if(TryGetResourceInfo(name, out entry))
				{// Resource exists
					FileStream.Seek(DataOffset + entry.Offset, SeekOrigin.Begin);
					return readData(entry.Size);
				}
			return null;
		}

		/// <summary>
		/// Gets a resource from the package by its ID
		/// </summary>
		/// <param name="id">Unique ID of the resource to retrieve</param>
		/// <returns>The data for the resource or null if no resource by the ID <paramref name="id"/> exists</returns>
		public byte[] GetResource (Guid id)
		{
			ResourcePackageEntry entry;
			lock(Locker)
				if(TryGetResourceInfo(id, out entry))
				{// Resource exists
					FileStream.Seek(DataOffset + entry.Offset, SeekOrigin.Begin);
					return readData(entry.Size);
				}
			return null;
		}

		/// <summary>
		/// Reads data from the current position in the package
		/// </summary>
		/// <param name="length">Amount of data to read</param>
		/// <returns>Raw data read from the package (decompressed and decrypted)</returns>
		private byte[] readData (int length)
		{
			const int bufferSize = 4 * Kilobyte;

			// Read in the compressed data
			var packedData = _br.ReadBytes(length);

			// Decompress the data
			// TODO: Handle encryption
			using(var ms = new MemoryStream(packedData))
			using(var ds = new DeflateStream(ms, CompressionMode.Decompress))
			using(var rs = new MemoryStream(bufferSize))
			{
				int bytesRead;
				do
				{// Continue reading data
					var buffer = new byte[bufferSize];
					bytesRead  = ds.Read(buffer, 0, bufferSize);
					rs.Write(buffer, 0, bytesRead);
				} while(bytesRead >= bufferSize);
				return rs.ToArray();
			}
		}
		#endregion

		/// <summary>
		/// Closes the resource package file.
		/// No more operations to the contents of the package will be allowed for this instance.
		/// </summary>
		public override void Close ()
		{
			_br.Close();
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
				if(disposing)
					Close();
			}
		}
	}
}
