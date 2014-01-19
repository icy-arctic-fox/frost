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
			BlockSize = (fileInfo.KbCount + 1) * Kilobyte; // +1 to make 0 mean 1 KB block size

			// TODO: Implement header info encryption

			// Read the resource header information (meta-data for resources in the file)
			NodeContainer header;
			try
			{
				header = NodeContainer.ReadFromStream(_br);
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
			var headerBytes = FileStream.Position;
			DataOffset      = (int)(headerBytes / BlockSize);
			if(headerBytes % BlockSize != 0)
				++DataOffset; // Round up

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
			var ver     = br.ReadByte();
			var opts    = (ResourcePackageOptions)br.ReadUInt16();
			var kbCount = br.ReadByte();
			return new HeaderInfo(ver, opts, kbCount);
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
					Entries.Add(name, entry);
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
			if(Entries.TryGetValue(name, out entry)) // TODO: Add locking
			{// Resource exists
				SeekDataBlock(entry.BlockOffset);
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
				_br.Dispose();
			}
		}
	}
}
