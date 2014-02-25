using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Frost.IO.Tnt;
using Frost.Utility;
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
		/// <param name="name">Name of the resource package</param>
		/// <param name="creator">Information about the creator of the package</param>
		/// <param name="description">Brief description of what the resource package is for</param>
		/// <remarks>The file will remain open until <see cref="Close"/> or <see cref="Dispose"/> is called.</remarks>
		public ResourcePackageWriter (string filepath, ResourcePackageOptions opts = ResourcePackageOptions.None,
			string name = null, string creator = null, string description = null)
		{
			// Create the file stream
			FileStream = new FileStream(filepath, FileMode.Create);
			_bw        = new EndianBinaryWriter(FileStream, Endian.Big);

			Name        = name        ?? Path.GetFileNameWithoutExtension(filepath) ?? filepath;
			Creator     = creator     ?? Environment.UserName;
			Description = description ?? String.Empty;

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
		/// <exception cref="ObjectDisposedException">Thrown if the package writer has been disposed</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> or <paramref name="data"/> are null</exception>
		/// <exception cref="ArgumentException">Thrown if a resource by the <paramref name="name"/> or <paramref name="id"/> has already been added</exception>
		public void Add (Guid id, string name, byte[] data) // TODO: Add encryption and other options
		{
			EnsureUndisposed();
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource can't be null.");
			if(data == null)
				throw new ArgumentNullException("data", "The raw data for the resource can't be null.");

			var packedData = packData(data);
			var size       = packedData.Length;

			lock(Locker)
			{
				var entry = new ResourcePackageEntry(id, name, _curOffset, packedData.Length);
				AddResource(entry);
				_packedResources.Add(packedData);
				_curOffset += size;
			}
		}
		#endregion

		#region IO
		#region Save

		/// <summary>
		/// Triggered when progress is made while writing the resource package
		/// </summary>
		/// <remarks>This event can be subscribed to cross-thread.</remarks>
		public event EventHandler<ProgressEventArgs> Progress;

		#region Header

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

		private const int MinIterations = 16;

		/// <summary>
		/// Reads the encryption information from the header
		/// </summary>
		/// <param name="bw">Writer used to put data into the file</param>
		/// <param name="password">Password to protect the header entries with</param>
		/// <returns>A transformation object used to encrypt the header entries</returns>
		private static ICryptoTransform writeEncryptionHeader (BinaryWriter bw, string password)
		{
			using(var aes = new RijndaelManaged())
			{
				aes.GenerateIV();

				var iv     = aes.IV;
				var ivSize = iv.Length;
				var salt   = new byte[SaltSize];
				int iterations;
				using(var rng = new RNGCryptoServiceProvider())
				{
					rng.GetBytes(salt);
					do
					{// Guarantee that there are at least MinIterations
						var iterBytes = new byte[sizeof(byte)];
						rng.GetBytes(iterBytes);
						iterations = iterBytes[0];
					} while(iterations < MinIterations);
				}

				bw.Write(salt);
				bw.Write(ivSize);
				bw.Write(iv);
				bw.Write((byte)iterations);

				var passBytes = System.Text.Encoding.UTF8.GetBytes(password);
				using(var keygen = new Rfc2898DeriveBytes(passBytes, salt, iterations))
					aes.Key = keygen.GetBytes(aes.KeySize / 8);

				return aes.CreateEncryptor();
			}
		}

		/// <summary>
		/// Constructs the node container that contains information about all of the resource entries in the package
		/// </summary>
		/// <param name="entries">Information about resources in the package</param>
		/// <param name="name">Name of the resource package</param>
		/// <param name="creator">Information about the creator of the resource package</param>
		/// <param name="description">Brief description of the contents of the resource package</param>
		/// <returns>A node container that contains the resource information</returns>
		private static NodeContainer constructHeaderContainer (IEnumerable<ResourcePackageEntry> entries, string name, string creator, string description)
		{
			var root = new ComplexNode {
				{"name",        new StringNode(name)},
				{"creator",     new StringNode(creator)},
				{"description", new StringNode(description)}
			};

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
			var container = constructHeaderContainer(Resources, Name, Creator, Description);
			byte[] headerData;
			using(var ms = new MemoryStream())
			{
				if((info.Options & ResourcePackageOptions.EncryptedHeader) == ResourcePackageOptions.EncryptedHeader)
				{// Encrypt header entries
					var encryptor = writeEncryptionHeader(_bw, String.Empty /* TODO */);
					using(var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
					using(var ds = new DeflateStream(cs, CompressionMode.Compress))
						container.WriteToStream(ds);
				}
				else
				{// Don't encrypt the header entries
					using(var ds = new DeflateStream(ms, CompressionMode.Compress))
						container.WriteToStream(ds);
				}
				headerData = ms.ToArray();
			}
			_bw.Write(headerData.Length);
			_bw.Write(headerData, 0, headerData.Length);
		}
		#endregion

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
		/// <exception cref="ObjectDisposedException">Thrown if the packager writer has been disposed</exception>
		public void Flush ()
		{
			if(Disposed)
				throw new ObjectDisposedException(GetType().FullName);
			Progress.NotifyThreadedSubscribers(this, new ProgressEventArgs(0L, 1L, 0L)); // TODO
			lock(Locker)
			{
				writeHeader();
				writeResources();
				Size = FileStream.Position;
			}
			Progress.NotifyThreadedSubscribers(this, new ProgressEventArgs(0L, 1L, 1L)); // TODO
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
				if(disposing)
					Close();
				Disposed = true;
			}
		}
	}
}
