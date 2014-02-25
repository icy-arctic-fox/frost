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
		private const int SaltSize = 32;

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

			// Read the resource header information (meta-data for resources in the file)
			NodeContainer header;
			try
			{
				header = readHeader(_br, fileInfo);

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

			var encAlg = ((opts & ResourcePackageOptions.EncryptedHeader) == ResourcePackageOptions.EncryptedHeader)
				? readEncryptionHeader(br) : null;

			return new HeaderInfo(ver, opts, encAlg);
		}

		#region Encryption

		/// <summary>
		/// Describes a method that retrieves the password needed to decrypt an entry
		/// </summary>
		/// <returns></returns>
		public delegate string PromptPassword ();

		/// <summary>
		/// Triggered when a password is required to decrypt the resource package header
		/// </summary>
		public static event PromptPassword PasswordNeeded;

		/// <summary>
		/// Reads the encryption information from the header
		/// </summary>
		/// <param name="br">Reader used to get data from the file</param>
		/// <returns>Algorithm used to decrypt the header</returns>
		private static SymmetricAlgorithm readEncryptionHeader (BinaryReader br)
		{
			var salt       = br.ReadBytes(SaltSize);
			var ivSize     = br.ReadInt32();
			var iv         = br.ReadBytes(ivSize);
			var iterations = br.ReadInt32();
			var passStr    = PasswordNeeded != null ? (PasswordNeeded() ?? String.Empty) : String.Empty;
			var passBytes  = System.Text.Encoding.UTF8.GetBytes(passStr);

			var aes = new RijndaelManaged {IV = iv};
			using(var keygen = new Rfc2898DeriveBytes(passBytes, salt, iterations))
				aes.Key = keygen.GetBytes(aes.KeySize / 8);
			return aes;
		}
		#endregion

		/// <summary>
		/// Reads the header entries from the package header
		/// </summary>
		/// <param name="br">Reader used to get data from the file</param>
		/// <param name="info">File header information</param>
		/// <returns>Header data</returns>
		private static NodeContainer readHeader (BinaryReader br, HeaderInfo info)
		{
			var headerSize = br.ReadInt32();
			var headerData = br.ReadBytes(headerSize);
			using(var ms = new MemoryStream(headerData))
			{
				if((info.Options & ResourcePackageOptions.EncryptedHeader) == ResourcePackageOptions.EncryptedHeader &&
					(info.EncryptionAlgorithm != null))
				{// Header is encrypted
					var decryptor = info.EncryptionAlgorithm.CreateDecryptor();
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
					return readData(entry.Size);
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
					return readData(entry.Size);
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
					return getDataStream(entry.Size);
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
					return getDataStream(entry.Size);
				}
			return null;
		}

		/// <summary>
		/// Reads data from the current position in the package
		/// </summary>
		/// <param name="length">Amount of packed data to read</param>
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

		/// <summary>
		/// Reads data from the current position in the package and creates a stream from it
		/// </summary>
		/// <param name="length">Amount of packed data to read</param>
		/// <returns>Raw data read</returns>
		private Stream getDataStream (int length)
		{
			// Read in the compressed data
			var packedData = _br.ReadBytes(length);

			// Create a stream used to decompress the data
			// TODO: Handle encryption
			var ms = new MemoryStream(packedData);
			return new DeflateStream(ms, CompressionMode.Decompress);
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
