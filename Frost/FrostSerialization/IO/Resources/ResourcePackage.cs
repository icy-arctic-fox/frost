using System;
using System.Collections.Generic;
using System.IO;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Basic functionality for processing resource packages
	/// </summary>
	public abstract class ResourcePackage : IDisposable
	{
		protected const int Kilobyte = 1024;

		/// <summary>
		/// Version of the resource package
		/// </summary>
		public byte Version { get; protected set; }

		/// <summary>
		/// Flags that specify options for the resource package
		/// </summary>
		public ResourcePackageOptions Options { get; protected set; }

		/// <summary>
		/// Size of each file block in bytes
		/// </summary>
		public int BlockSize { get; protected set; }

		/// <summary>
		/// Offset in the file (measured in blocks) to where the data starts
		/// </summary>
		/// <remarks>This is also equal to the number of blocks used by the header.</remarks>
		public int DataOffset { get; protected set; }

		/// <summary>
		/// Underlying stream to access the resource data
		/// </summary>
		protected Stream FileStream;

		/// <summary>
		/// Mapping of resource names to information about a resource in the package
		/// </summary>
		protected readonly Dictionary<string, ResourcePackageEntry> Entries = new Dictionary<string, ResourcePackageEntry>();

		#region IO
		#region Access

		/// <summary>
		/// Seeks to a block in the file
		/// </summary>
		/// <param name="block">Block index</param>
		protected void SeekBlock (int block)
		{
			var offset = block * BlockSize;
			FileStream.Seek(offset, SeekOrigin.Begin);
		}

		/// <summary>
		/// Seeks to a data block in the file
		/// </summary>
		/// <param name="block">Block index</param>
		protected void SeekDataBlock (int block)
		{
			SeekBlock(block + DataOffset);
		}
		#endregion

		/// <summary>
		/// Closes the resource package file.
		/// No more operations to the contents of the package will be allowed for this instance.
		/// </summary>
		public abstract void Close ();
		#endregion

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the resource package has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
			protected set { _disposed = value; }
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
		protected abstract void Dispose (bool disposing);
		#endregion
	}
}
