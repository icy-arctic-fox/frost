using System;
using System.Collections.Generic;
using System.IO;
using Frost.Utility;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Basic functionality for processing resource packages.
	/// Resource packages can contain many resources of any type.
	/// Resources contained in the package files can also be encrypted and compressed.
	/// </summary>
	public abstract class ResourcePackage : IPackageInfo, IFullDisposable
	{
		protected const int Kilobyte = 1024;

		protected const int SaltSize = 32;

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
		/// Size of the resource package in bytes
		/// </summary>
		public long Size { get; protected set; }

		/// <summary>
		/// Underlying stream to access the resource data
		/// </summary>
		protected Stream FileStream;

		#region Entries

		/// <summary>
		/// Used to lock access to the package entries
		/// </summary>
		protected readonly object Locker = new object();

		/// <summary>
		/// Mapping of resource names to information about a resource in the package
		/// </summary>
		private readonly Dictionary<string, ResourcePackageEntry> _entries = new Dictionary<string, ResourcePackageEntry>();

		/// <summary>
		/// Taken resource IDs
		/// </summary>
		private readonly HashSet<Guid> _ids = new HashSet<Guid>();

		/// <summary>
		/// Checks if the package contains a resource
		/// </summary>
		/// <param name="name">Name of the resource</param>
		/// <returns>True if the resource exists or false if it doesn't</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public bool ContainsResource (string name)
		{
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource to check for can't be null.");

			lock(Locker)
				return _entries.ContainsKey(name);
		}

		/// <summary>
		/// Checks if the package contains a resource
		/// </summary>
		/// <param name="id">Globally unique ID of the resource</param>
		/// <returns>True if the resource exists or false if it doesn't</returns>
		public bool ContainsResource (Guid id)
		{
			lock(Locker)
				return _ids.Contains(id);
		}

		/// <summary>
		/// Adds information about an entry in the package
		/// </summary>
		/// <param name="entry">Resource information</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="entry"/> is null</exception>
		/// <exception cref="ArgumentException">Thrown if an entry with the same name or ID already exists</exception>
		protected void AddResource (ResourcePackageEntry entry)
		{
			if(entry == null)
				throw new ArgumentNullException("entry", "The new resource entry can't be null.");

			lock(Locker)
			{
				try
				{
					_entries.Add(entry.Name, entry);
				}
				catch(ArgumentException)
				{
					throw new ArgumentException("An entry with the same name already exists.");
				}
				if(!_ids.Add(entry.Id))
					throw new ArgumentException("An entry with the same ID already exists.");
			}
		}

		/// <summary>
		/// Attempts to retrieve information about a resource
		/// </summary>
		/// <param name="name">Name of the resource to retrieve information for</param>
		/// <param name="entry">Information about the entry</param>
		/// <returns>True if the resource exists</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public bool TryGetResourceInfo (string name, out ResourcePackageEntry entry)
		{
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource to retrieve for can't be null.");

			lock(Locker)
				return _entries.TryGetValue(name, out entry);
		}

		/// <summary>
		/// Attempts to retrieve information about a resource
		/// </summary>
		/// <param name="id">ID of the resource to retrieve information for</param>
		/// <param name="entry">Information about the entry</param>
		/// <returns>True if the resource exists</returns>
		public bool TryGetResourceInfo (Guid id, out ResourcePackageEntry entry)
		{
			lock(Locker)
				foreach(var resource in _entries.Values)
					if(resource.Id == id)
					{
						entry = resource;
						return true;
					}
			entry = null;
			return false;
		}

		/// <summary>
		/// Collection of all resources in the package
		/// </summary>
		public IEnumerable<ResourcePackageEntry> Resources
		{
			get { return _entries.Values; }
		}

		/// <summary>
		/// Number of resources contained in the package
		/// </summary>
		public int Count
		{
			get { return _entries.Count; }
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
		/// Triggered when the resource package is being disposed
		/// </summary>
		/// <remarks>This event can be subscribed to cross-thread.</remarks>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Disposes of the resource package by closing the file and freeing resources
		/// </summary>
		public void Dispose ()
		{
			Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);
			Dispose(true);
			GC.SuppressFinalize(this);
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

		/// <summary>
		/// Checks if the resource package has been disposed
		/// </summary>
		/// <exception cref="ObjectDisposedException">Thrown if the resource package has been disposed</exception>
		protected void EnsureUndisposed ()
		{
			if(Disposed)
				throw new ObjectDisposedException(GetType().FullName);
		}
		#endregion
	}
}
