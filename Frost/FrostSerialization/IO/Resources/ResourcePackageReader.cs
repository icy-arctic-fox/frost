using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Frost.IO.Tnt;
using Frost.Utility;
using Ionic.Zlib;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Provides access to existing resource package files.
	/// Resource packages can contain many resources of any type.
	/// Resources contained in the package files can also be encrypted and compressed.
	/// </summary>
	public class ResourcePackageReader : IDisposable
	{
		private const int Kilobyte = 1024;

		private readonly byte _ver;

		/// <summary>
		/// Version of the resource package
		/// </summary>
		public byte Version
		{
			get { return _ver; }
		}

		private readonly ResourcePackageOptions _opts;

		/// <summary>
		/// Flags that specify options for the resource package
		/// </summary>
		public ResourcePackageOptions Options
		{
			get { return _opts; }
		}

		/// <summary>
		/// Size of each file block in bytes
		/// </summary>
		private readonly int _blockSize;

		/// <summary>
		/// Offset in the file (measured in blocks) to where the data starts
		/// </summary>
		/// <remarks>This is also equal to the number of blocks used by the header.</remarks>
		private readonly int _dataOffset;

		/// <summary>
		/// Underlying stream to access the resource data
		/// </summary>
		private readonly Stream _s;

		/// <summary>
		/// Reader used to pull data from the file
		/// </summary>
		private readonly BinaryReader _br;

		private ResourcePackageReader (byte ver, int blockSize, int dataOffset, ResourcePackageOptions opts, Stream s, BinaryReader br)
		{
			_ver        = ver;
			_blockSize  = blockSize;
			_dataOffset = dataOffset;
			_opts       = opts;
			_s          = s;
			_br         = br;
		}

		#region Entries
		private readonly Dictionary<string, ResourcePackageEntry> _entries = new Dictionary<string, ResourcePackageEntry>();
		#endregion

		#region IO
		#region Load

		/// <summary>
		/// Opens a resource package file to start pulling resources from it
		/// </summary>
		/// <param name="path">Path to the resource file</param>
		/// <returns>A reference to a resource package file</returns>
		/// <exception cref="FileNotFoundException">Thrown if the resource package file wasn't found under <paramref name="path"/></exception>
		/// <exception cref="InvalidDataException">Thrown if the data contained in the resource file is invalid</exception>
		/// <remarks>The file will remain open until <see cref="Close"/> or <see cref="Dispose"/> is called</remarks>
		public static ResourcePackageReader Load (string path)
		{
			// Create the file stream
			var fs = new FileStream(path, FileMode.Open);
			var br = new EndianBinaryReader(fs, Endian.Big);

			// Read the file header info
			var fileInfo  = readFileInfo(br);
			var blockSize = (fileInfo.KbCount + 1) * Kilobyte; // +1 to make 0 mean 1 KB block size

			// TODO: Implement header info encryption
			// TODO: Implement info about the package (creator, name, description, mod, etc.)

			// Read the resource header information (meta-data for resources in the file)
			NodeContainer header;
			try
			{
				header = NodeContainer.ReadFromStream(br);
			}
			catch(FormatException e)
			{
				br.Dispose();
				throw new InvalidDataException("The header data is in an unrecognized format.", e);
			}
			catch(InvalidDataException e)
			{
				br.Dispose();
				throw new InvalidDataException("The header data is in an unrecognized format.", e);
			}

			// Calculate how big the header is (and where the data starts)
			var headerBytes = fs.Position;
			var blockOffset = (int)(headerBytes / blockSize);
			if(headerBytes % blockSize != 0)
				++blockOffset; // Round up

			var pkg = new ResourcePackageReader(fileInfo.Version, blockSize, blockOffset, fileInfo.Options, fs, br);
			extractHeaderEntries(header, pkg);
			return pkg;
		}

		/// <summary>
		/// Reads resource package header information from the file
		/// </summary>
		/// <param name="br">Reader used to get data from the file</param>
		/// <returns>Raw header information</returns>
		private static HeaderInfo readFileInfo (BinaryReader br)
		{
			var ver     = br.ReadByte();
			var opts    = (ResourcePackageOptions)br.ReadUInt16();
			var kbCount = br.ReadByte();
			return new HeaderInfo(ver, opts, kbCount);
		}

		/// <summary>
		/// Extracts resource entries from the header and adds them to the package's records
		/// </summary>
		/// <param name="header">Header to extract entries from</param>
		/// <param name="pkg">Package to add resource entries to</param>
		private static void extractHeaderEntries (NodeContainer header, ResourcePackageReader pkg)
		{
			try
			{
				var root = header.Root.ExceptComplexNode();
				// TODO: Capture package name, creator, and description
				var entries = root.ExpectListNode("entries", NodeType.Complex);
				foreach(ComplexNode node in entries)
				{
					var id     = node.ExpectGuidNode("id");
					var name   = node.ExpectStringNode("name");
					var offset = node.ExpectIntNode("offset");
					var size   = node.ExpectIntNode("size");
					var entry  = new ResourcePackageEntry(id, name, offset, size);
					pkg._entries.Add(name, entry);
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
			if(_entries.TryGetValue(name, out entry)) // TODO: Add locking
			{// Resource exists
				seekDataBlock(entry.BlockOffset);
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
			var compressedData = _br.ReadBytes(length);

			// Decompress the data
			// TODO: Handle encryption
			var blocks = new Queue<byte[]>();
			using(var ms = new MemoryStream(compressedData))
			using(var ds = new DeflateStream(ms, CompressionMode.Decompress))
			{
				int bytesRead;
				do
				{// Continue reading data
					var buffer = new byte[bufferSize];
					bytesRead  = ds.Read(buffer, 0, bufferSize);
					if(bytesRead < bufferSize) // Reduce the buffer size
						buffer = buffer.Duplicate(0, bytesRead);
					blocks.Enqueue(buffer);
				} while(bytesRead >= bufferSize);
			}

			// Assemble the data into a single array
			var size = blocks.Sum(block => block.Length);
			var data = new byte[size];
			var pos  = 0;
			while(blocks.Count > 0)
			{
				var block     = blocks.Dequeue();
				var blockSize = block.Length;
				block.Copy(data, 0, pos, blockSize);
				pos += blockSize;
			}
			return data;
		}

		/// <summary>
		/// Seeks to a block in the file
		/// </summary>
		/// <param name="block">Block index</param>
		private void seekBlock(int block)
		{
			var offset = block * _blockSize;
			_s.Seek(offset, SeekOrigin.Begin);
		}

		/// <summary>
		/// Seeks to a data block in the file
		/// </summary>
		/// <param name="block">Block index</param>
		private void seekDataBlock (int block)
		{
			seekBlock(block + _dataOffset);
		}
		#endregion

		/// <summary>
		/// Closes the resource package file
		/// </summary>
		public void Close ()
		{
			_br.Close();
		}
		#endregion

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the resource package reader has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Disposes of the resource package reader by closing the file and freeing resources
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Destructor - disposes of the resource package reader
		/// </summary>
		~ResourcePackageReader ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the resource package reader
		/// </summary>
		/// <param name="disposing">True if inner-resources should be disposed of (<see cref="Dispose"/> was called)</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				if(disposing)
					_br.Dispose();
				_disposed = true;
			}
		}
		#endregion
	}
}
