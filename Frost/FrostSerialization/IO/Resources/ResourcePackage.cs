using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Frost.IO.Tnt;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Provides access to existing resource package files.
	/// Resource packages can contain many resources of any type.
	/// Resources contained in the package files can also be encrypted and compressed.
	/// </summary>
	public class ResourcePackage : IDisposable
	{
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

		private readonly int _blockSize;
		private readonly BinaryReader _br;

		private ResourcePackage (byte ver, int blockSize, ResourcePackageOptions opts, BinaryReader br)
		{
			_ver       = ver;
			_blockSize = blockSize;
			_opts     = opts;
			_br       = br;
		}

		#region IO

		/// <summary>
		/// Opens a resource package file to start pulling resources from it
		/// </summary>
		/// <param name="path">Path to the resource file</param>
		/// <returns>A reference to a resource package file</returns>
		/// <exception cref="FileNotFoundException">Thrown if the resource package file wasn't found under <paramref name="path"/></exception>
		/// <remarks>The file will remain open until <see cref="Close"/> or <see cref="Dispose"/> is called</remarks>
		public static ResourcePackage Load (string path)
		{
			// Create the file stream
			var fs = new FileStream(path, FileMode.Open);
			var br = new EndianBinaryReader(fs, Endian.Big);

			// Read the file header info
			var fileInfo  = readFileInfo(br);
			var blockSize = (fileInfo.KbCount + 1) * 1024;

			// TODO: Implement header info encryption

			var pkg = new ResourcePackage(fileInfo.Version, blockSize, fileInfo.Options, br);

			// Read the resource header information (meta-data for resources in the file)
			var header = NodeContainer.ReadFromStream(br);
			// TODO: Extract resource information from node data

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
		/// Indicates whether the resource package has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Disposes of the resource package by closing the file and freeing resources
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Destructor - disposes of the resource package
		/// </summary>
		~ResourcePackage ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the resource package
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
