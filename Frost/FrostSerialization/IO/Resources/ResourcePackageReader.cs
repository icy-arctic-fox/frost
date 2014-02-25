using System;
using System.IO;
using System.Security.Cryptography;
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
		/// <param name="password">Password used to encrypt the resource package</param>
		/// <exception cref="FileNotFoundException">Thrown if the resource package file wasn't found under <paramref name="filepath"/></exception>
		/// <exception cref="InvalidDataException">Thrown if the data contained in the resource file is invalid</exception>
		/// <remarks>The file will remain open until <see cref="Close"/> or <see cref="Dispose"/> is called.</remarks>
		public ResourcePackageReader (string filepath, string password = null)
		{
			// Create the file stream
			FileStream = new FileStream(filepath, FileMode.Open);
			_br        = new EndianBinaryReader(FileStream, Endian.Big);

			// Read the file header info
			var fileInfo = readFileInfo(_br);
			Version = fileInfo.Version;
			Options = fileInfo.Options;
			Size    = new FileInfo(filepath).Length;

			// Read the resource header information (meta-data for resources in the file)
			NodeContainer header;
			try
			{
				header = readHeader(_br, fileInfo, password);

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
			catch(CryptographicException e)
			{
				_br.Dispose();
				throw new InvalidDataException("Incorrect password.", e);
			}

			// Calculate how big the header is (and where the data starts)
			DataOffset = FileStream.Position;

			// Pull entry information from the header
			extractHeaderEntries(header);
		}

		#region IO
		#region Load
		#region Header

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
		/// Reads the encryption information from the header
		/// </summary>
		/// <param name="br">Reader used to get data from the file</param>
		/// <param name="password">Password needed to decrypt the header entries</param>
		/// <returns>A transformation object used to decrypt the header entries</returns>
		private static ICryptoTransform readEncryptionHeader (BinaryReader br, string password)
		{
			using(var aes = new RijndaelManaged())
			{
				var salt       = br.ReadBytes(SaltSize);
				var ivSize     = br.ReadInt32();
				var iv         = br.ReadBytes(ivSize);
				var iterations = br.ReadByte();
				var passBytes  = System.Text.Encoding.UTF8.GetBytes(password);

				aes.IV = iv;
				using(var keygen = new Rfc2898DeriveBytes(passBytes, salt, iterations))
					aes.Key = keygen.GetBytes(aes.KeySize / 8);
				return aes.CreateDecryptor();
			}
		}

		/// <summary>
		/// Reads the header entries from the package header
		/// </summary>
		/// <param name="br">Reader used to get data from the file</param>
		/// <param name="info">File header information</param>
		/// <param name="password">Password used to encrypt the header entries</param>
		/// <returns>Header data</returns>
		private static NodeContainer readHeader (BinaryReader br, HeaderInfo info, string password = null)
		{
			var encrypted = (info.Options & ResourcePackageOptions.EncryptedHeader) == ResourcePackageOptions.EncryptedHeader;
			var decryptor = encrypted ? readEncryptionHeader(br, password ?? String.Empty) : null;

			var headerSize = br.ReadInt32();
			var headerData = br.ReadBytes(headerSize);
			using(var ms = new MemoryStream(headerData))
			{
				if(encrypted)
				{// Header is encrypted
					using(var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
					using(var ds = new DeflateStream(cs, CompressionMode.Decompress))
						return NodeContainer.ReadFromStream(ds);
				}
				// Header is compressed, but not encrypted
				using(var ds = new DeflateStream(ms, CompressionMode.Decompress))
					return NodeContainer.ReadFromStream(ds);
			}
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
		#endregion

		#region Access

		/// <summary>
		/// Gets a resource from the package by its name
		/// </summary>
		/// <param name="name">Name of the resource to retrieve</param>
		/// <returns>The data for the resource or null if no resource by the name <paramref name="name"/> exists</returns>
		/// <exception cref="ObjectDisposedException">Thrown if the package reader has been disposed</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public byte[] GetResource (string name)
		{
			EnsureUndisposed();
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource to retrieve can't be null.");

			ResourcePackageEntry entry;
			lock(Locker)
				if(TryGetResourceInfo(name, out entry))
				{// Resource exists
					FileStream.Seek(DataOffset + entry.Offset, SeekOrigin.Begin);
					return readData(entry.Size, entry.Key);
				}
			return null;
		}

		/// <summary>
		/// Gets a resource from the package by its ID
		/// </summary>
		/// <param name="id">Unique ID of the resource to retrieve</param>
		/// <returns>The data for the resource or null if no resource by the ID <paramref name="id"/> exists</returns>
		/// <exception cref="ObjectDisposedException">Thrown if the package reader has been disposed</exception>
		public byte[] GetResource (Guid id)
		{
			EnsureUndisposed();
			ResourcePackageEntry entry;
			lock(Locker)
				if(TryGetResourceInfo(id, out entry))
				{// Resource exists
					FileStream.Seek(DataOffset + entry.Offset, SeekOrigin.Begin);
					return readData(entry.Size, entry.Key);
				}
			return null;
		}

		/// <summary>
		/// Gets a stream for a resource from a package by its name
		/// </summary>
		/// <param name="name">Name of the resource to retrieve</param>
		/// <returns>A stream that can be used to pull resource data or null if the resource doesn't exist</returns>
		/// <exception cref="ObjectDisposedException">Thrown if the package reader has been disposed</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public Stream GetResourceStream (string name)
		{
			EnsureUndisposed();
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource to retrieve can't be null.");

			ResourcePackageEntry entry;
			lock(Locker)
				if(TryGetResourceInfo(name, out entry))
				{// Resource exists
					FileStream.Seek(DataOffset + entry.Offset, SeekOrigin.Begin);
					return getDataStream(entry.Size, entry.Key);
				}
			return null;
		}

		/// <summary>
		/// Gets a stream for a resource from a package by its ID
		/// </summary>
		/// <param name="id">Unique ID of the resource to retrieve</param>
		/// <returns>A stream that can be used to pull resource data or null if the resource doesn't exist</returns>
		/// <exception cref="ObjectDisposedException">Thrown if the package reader has been disposed</exception>
		public Stream GetResourceStream (Guid id)
		{
			EnsureUndisposed();
			ResourcePackageEntry entry;
			lock(Locker)
				if(TryGetResourceInfo(id, out entry))
				{// Resource exists
					FileStream.Seek(DataOffset + entry.Offset, SeekOrigin.Begin);
					return getDataStream(entry.Size, entry.Key);
				}
			return null;
		}

		/// <summary>
		/// Reads bytes from a stream using a buffer
		/// </summary>
		/// <param name="s">Stream to read from</param>
		/// <returns>Array of bytes read from the stream</returns>
		private byte[] readBytesFromStream (Stream s)
		{
			const int bufferSize = 4 * Kilobyte;

			using(var rs = new MemoryStream(bufferSize))
			{
				int bytesRead;
				do
				{ // Continue reading data
					var buffer = new byte[bufferSize];
					bytesRead = s.Read(buffer, 0, bufferSize);
					rs.Write(buffer, 0, bytesRead);
				} while(bytesRead >= bufferSize);
				return rs.ToArray();
			}
		}

		/// <summary>
		/// Reads data from the current position in the package
		/// </summary>
		/// <param name="length">Amount of packed data to read</param>
		/// <returns>Raw data read from the package (decompressed and decrypted)</returns>
		private byte[] readData (int length, byte[] key)
		{
			// Read in the compressed data
			var packedData = _br.ReadBytes(length);

			// Decompress the data
			using(var ms = new MemoryStream(packedData))
			{
				if(key != null)
				{// Resource is encrypted
					var ivSize = _br.ReadInt32();
					var iv = _br.ReadBytes(ivSize);
					using(var aes = new RijndaelManaged())
					using(var decryptor = aes.CreateDecryptor(key, iv))
					using(var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
						return readBytesFromStream(cs);
				}
				// Resource is not encrypted
				using(var ds = new DeflateStream(ms, CompressionMode.Decompress))
					return readBytesFromStream(ds);
			}
		}

		/// <summary>
		/// Reads data from the current position in the package and creates a stream from it
		/// </summary>
		/// <param name="length">Amount of packed data to read</param>
		/// <returns>Raw data read</returns>
		private Stream getDataStream (int length, byte[] key)
		{
			// Read in the compressed data
			var packedData = _br.ReadBytes(length);

			// Create a stream used to decompress the data
			Stream s;
			var ms = new MemoryStream(packedData);
			if(key != null)
			{// Resource is encrypted
				var ivSize = _br.ReadInt32();
				var iv = _br.ReadBytes(ivSize);
				using(var aes = new RijndaelManaged())
				{
					var decryptor = aes.CreateDecryptor(key, iv);
					s = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
				}
			}
			else // Resource is not encrypted
				s = ms;
			return new DeflateStream(s, CompressionMode.Decompress);
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
