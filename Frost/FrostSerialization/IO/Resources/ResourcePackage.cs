using System;
using System.Collections.Generic;
using System.IO;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Basic functionality for processing resource packages.
	/// Resource packages can contain many resources of any type.
	/// Resources contained in the package files can also be encrypted and compressed.
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
		/// Offset in the file (measured in bytes) to where the data starts
		/// </summary>
		/// <remarks>This is also equal to the number of bytes used by the header.</remarks>
		public long DataOffset { get; protected set; }

		/// <summary>
		/// Name of the resource package
		/// </summary>
		public string Name { get; protected set; }

		/// <summary>
		/// Information about the creator of the resource package
		/// </summary>
		public string Creator { get; protected set; }

		/// <summary>
		/// Brief description of the contents of the resource package
		/// </summary>
		public string Description { get; protected set; }

		/// <summary>
		/// Underlying stream to access the resource data
		/// </summary>
		protected Stream FileStream;

		#region Entries

		/// <summary>
		/// Mapping of resource names to information about a resource in the package
		/// </summary>
		protected readonly Dictionary<string, ResourcePackageEntry> Entries = new Dictionary<string, ResourcePackageEntry>();

		/// <summary>
		/// Collection of all resources in the package
		/// </summary>
		public IEnumerable<ResourcePackageEntry> Resources
		{
			get { return Entries.Values; }
		}

		/// <summary>
		/// Number of resources contained in the package
		/// </summary>
		public int Count
		{
			get { return Entries.Count; }
		}
		#endregion

		#region IO

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
