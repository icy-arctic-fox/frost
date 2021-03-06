using System;
using System.Collections.Generic;
using System.IO;
using Frost.Resources;
using Frost.Utility;

namespace Frost
{
	/// <summary>
	/// Responsible for tracking all available resources and caching commonly used resources
	/// </summary>
	public class ResourceManager : IFullDisposable
	{
		private readonly object _locker = new object();

		/// <summary>
		/// Collection of all known package readers
		/// </summary>
		private readonly HashSet<ResourcePackageReader> _readers = new HashSet<ResourcePackageReader>();

		/// <summary>
		/// Mapping of resource names to a reader that can retrieve the resource.
		/// References to all resources are stored in _knownResources.
		/// When a resource is overwritten for the first time, it is moved to _originalResources.
		/// _originalResources contains "un-modded" resources only.
		/// </summary>
		private readonly Dictionary<string, ResourcePackageReader>
			_knownResources    = new Dictionary<string, ResourcePackageReader>(),
			_originalResources = new Dictionary<string, ResourcePackageReader>();

		/// <summary>
		/// Mapping of resource IDs to a reader that can retrieve the resource.
		/// This dictionary also tracks used IDs,
		/// since resource IDs must be globally unique across all referenced resource packages.
		/// </summary>
		private readonly Dictionary<Guid, ResourcePackageReader> _ids = new Dictionary<Guid, ResourcePackageReader>();

		/// <summary>
		/// Tracks resources that have been transformed so there isn't a need to process the load and transformation again
		/// </summary>
		/// <remarks><see cref="Guid"/>s are used here because all resources are required to have a unique ID.
		/// Using the resource ID prevents confusion between which resource transformation was cached;
		/// the modded resource, or the original resource.</remarks>
		private readonly Cache<Guid, object> _cachedResources = new Cache<Guid, object>();

		/// <summary>
		/// Adds all resource package files contained in a directory
		/// </summary>
		/// <param name="path">Fully qualified path to the directory containing the resources</param>
		/// <exception cref="ArgumentNullException">The <paramref name="path"/> to the directory can't be null.</exception>
		/// <exception cref="ApplicationException">Thrown if a resource was found with an ID that conflicts with an existing resource</exception>
		/// <exception cref="DirectoryNotFoundException">Thrown if the path to the resource directory doesn't exist</exception>
		public void AddResourceDirectory (string path)
		{
			if(path == null)
				throw new ArgumentNullException("path");

			Exception caught = null;
			foreach(var filepath in Directory.EnumerateFiles(path, "*.frp", SearchOption.AllDirectories))
			{
				try
				{
					AddResourcePackage(filepath);
				}
				catch(ApplicationException e)
				{
					caught = e;
				}
			}
			if(caught != null)
				throw caught;
		}

		/// <summary>
		/// Adds a resource package that can be referenced to retrieve resources
		/// </summary>
		/// <param name="filepath">Path to the resource package file</param>
		/// <exception cref="ArgumentNullException">The fully qualified file path (<paramref name="filepath"/>) can't be null.</exception>
		/// <exception cref="ApplicationException">Thrown if a resource was found with an ID that conflicts with an existing resource</exception>
		public void AddResourcePackage (string filepath)
		{
			if(filepath == null)
				throw new ArgumentNullException("filepath");

			var package = new ResourcePackageReader(filepath);
			AddResourcePackage(package);
		}

		/// <summary>
		/// Adds a resource package that can be referenced to retrieve resources
		/// </summary>
		/// <param name="package">Resource package</param>
		/// <exception cref="ArgumentNullException">The new resource <paramref name="package"/> can't be null.</exception>
		/// <exception cref="ApplicationException">Thrown if a resource was found with an ID that conflicts with an existing resource</exception>
		public void AddResourcePackage (ResourcePackageReader package)
		{
			if(package == null)
				throw new ArgumentNullException("package");

			lock(_locker)
				if(!_readers.Contains(package))
				{
					_readers.Add(package);
					indexResourcePackage(package);
				}
		}

		/// <summary>
		/// Stores the IDs and names of each resource in the package and a reference back to the reader
		/// </summary>
		/// <param name="package">Package to index</param>
		/// <exception cref="ApplicationException">Thrown if a resource with a duplicate ID was found</exception>
		private void indexResourcePackage (ResourcePackageReader package)
		{
			var duplicateId = false;
			foreach(var resource in package.Resources)
			{
				var id   = resource.Id;
				var name = resource.Name;

				// Index by ID
				if(_ids.ContainsKey(id))
					duplicateId = true;
				else
				{// Ok to index
					_ids.Add(id, package);

					// Index by name
					if(_knownResources.ContainsKey(name) && !_originalResources.ContainsKey(name))
						_originalResources.Add(name, _knownResources[name]); // Store original resource
					_knownResources[name] = package;
				}
			}
			if(duplicateId)
				throw new ApplicationException("One or more resources with a duplicate ID were encountered.");
		}

		/// <summary>
		/// Attempts to find and retrieve a resource by a given name
		/// </summary>
		/// <param name="name">Name of the requested resource</param>
		/// <param name="allowMod">When true, allows overwritten (modded) resources to be retrieved</param>
		/// <returns>Raw data for the resource or null if the resource doesn't exist</returns>
		/// <remarks>This method will not cache resource data.</remarks>
		/// <exception cref="ArgumentNullException">The <paramref name="name"/> of the resource to retrieve can't be null.</exception>
		public byte[] GetResource (string name, bool allowMod = true)
		{
			if(name == null)
				throw new ArgumentNullException("name");

			lock(_locker)
			{
				if(!allowMod && _originalResources.ContainsKey(name))
				{// Don't allow mods and use the original resource
					var reader = _originalResources[name];
					return reader.GetResource(name);
				}
				return _knownResources.ContainsKey(name) ? _knownResources[name].GetResource(name) : null;
			}
		}

		/// <summary>
		/// Attempts to find and retrieve a resource by a given name
		/// </summary>
		/// <param name="name">Name of the requested resource</param>
		/// <param name="allowMod">When true, allows overwritten (modded) resources to be retrieved</param>
		/// <returns>A stream containing the data for the resource or null if the resource doesn't exist</returns>
		/// <remarks>This method will not cache resource data.</remarks>
		/// <exception cref="ArgumentNullException">The <paramref name="name"/> of the resource to retrieve can't be null.</exception>
		public Stream GetResourceStream (string name, bool allowMod = true)
		{
			if(name == null)
				throw new ArgumentNullException("name");

			lock(_locker)
			{
				if(!allowMod && _originalResources.ContainsKey(name))
				{// Don't allow mods and use the original resource
					var reader = _originalResources[name];
					return reader.GetResourceStream(name);
				}
				return _knownResources.ContainsKey(name) ? _knownResources[name].GetResourceStream(name) : null;
			}
		}

		/// <summary>
		/// Describes a method that transforms a resource from its raw form to a usable format
		/// </summary>
		/// <typeparam name="TResource">Type of resource that is produced</typeparam>
		/// <param name="info">Information about the resource</param>
		/// <param name="data">Raw data representing the resource</param>
		/// <returns>A transformed resource</returns>
		public delegate TResource ResourceTranformation<out TResource> (ResourcePackageEntry info, byte[] data) where TResource : class;

		/// <summary>
		/// Attempts to find and retrieve a resource by a given name.
		/// If the resource has been loaded before and is still in the cache,
		/// the previous resource will be retrieved.
		/// </summary>
		/// <param name="name">Name of the requested resource</param>
		/// <param name="transform">Method used to transform raw resource data into a usable form</param>
		/// <param name="allowMod">When true, allows overwritten (modded) resources to be retrieved</param>
		/// <returns>Transformed resource or null if the resource wasn't found</returns>
		/// <remarks>This method will cache the transformed resource before it is returned.
		/// Caution, a modified resource that has been cached will stay modified the next time it is referenced.
		/// To prevent this from happening, treat resources as read-only.</remarks>
		/// <exception cref="ArgumentNullException">The <paramref name="name"/> of the resource to retrieve and the <paramref name="transform"/> method can't be null.</exception>
		public TResource GetResource<TResource> (string name, ResourceTranformation<TResource> transform, bool allowMod = true) where TResource : class
		{
			if(name == null)
				throw new ArgumentNullException("name");
			if(transform == null)
				throw new ArgumentNullException("transform");

			lock(_locker)
			{
				// Find a package reader that provides the resource
				ResourcePackageReader reader;
				if(!allowMod && _originalResources.ContainsKey(name)) // Don't allow mods and use the original resource
					reader = _originalResources[name];
				else if(_knownResources.ContainsKey(name))
					reader = _knownResources[name];
				else // Couldn't find the resource
					return null;

				// Get the ID of the resource
				ResourcePackageEntry info;
				Guid id;
				if(reader.TryGetResourceInfo(name, out info))
					id = info.Id;
				else
					return null;

				// Get the cached resource or executes the transformation method to generate it
				return (TResource)_cachedResources.GetItem(id, resId => {
					var data = reader.GetResource(resId);
					return transform(info, data);
				});
			}
		}

		/// <summary>
		/// Describes a method that transforms a resource from its raw form to a usable format
		/// </summary>
		/// <typeparam name="TResource">Type of resource that is produced</typeparam>
		/// <param name="info">Information about the resource</param>
		/// <param name="s">Stream that can be used to pull resource data from</param>
		/// <returns>A transformed resource</returns>
		public delegate TResource ResourceStreamTranformation<out TResource> (ResourcePackageEntry info, Stream s) where TResource : class;

		/// <summary>
		/// Attempts to find and retrieve a resource by a given name.
		/// If the resource has been loaded before and is still in the cache,
		/// the previous resource will be retrieved.
		/// </summary>
		/// <param name="name">Name of the requested resource</param>
		/// <param name="transform">Method used to transform raw resource data into a usable form</param>
		/// <param name="allowMod">When true, allows overwritten (modded) resources to be retrieved</param>
		/// <returns>Transformed resource or null if the resource wasn't found</returns>
		/// <remarks>This method will cache the transformed resource before it is returned.
		/// Caution, a modified resource that has been cached will stay modified the next time it is referenced.
		/// To prevent this from happening, treat resources as read-only.</remarks>
		/// <exception cref="ArgumentNullException">The <paramref name="name"/> of the resource to retrieve and the <paramref name="transform"/> method can't be null.</exception>
		public TResource GetResource<TResource> (string name, ResourceStreamTranformation<TResource> transform, bool allowMod = true) where TResource : class
		{
			if(name == null)
				throw new ArgumentNullException("name");
			if(transform == null)
				throw new ArgumentNullException("transform");

			lock(_locker)
			{
				// Find a package reader that provides the resource
				ResourcePackageReader reader;
				if(!allowMod && _originalResources.ContainsKey(name)) // Don't allow mods and use the original resource
					reader = _originalResources[name];
				else if(_knownResources.ContainsKey(name))
					reader = _knownResources[name];
				else // Couldn't find the resource
					return null;

				// Get the ID of the resource
				ResourcePackageEntry info;
				Guid id;
				if(reader.TryGetResourceInfo(name, out info))
					id = info.Id;
				else
					return null;

				// Get the cached resource or executes the transformation method to generate it
				return (TResource)_cachedResources.GetItem(id, resId => {
					var s = reader.GetResourceStream(resId);
					return transform(info, s);
				});
			}
		}

		/// <summary>
		/// List of the resource packages being used
		/// </summary>
		public IPackageInfo[] Packages
		{
			get
			{
				lock(_locker)
				{
					var info = new IPackageInfo[_readers.Count];
					var i = 0;
					foreach(var reader in _readers)
						info[i++] = reader;
					return info;
				}
			}
		}

		#region Disposable
		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the resource manager has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Triggered when the resource manager is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Disposes of the resource manager by releasing everything from the cache and references to all resources
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Destructor - disposes of the resource manager
		/// </summary>
		~ResourceManager ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the resource manager
		/// </summary>
		/// <param name="disposing">True if inner-resources should be disposed of (<see cref="Dispose"/> was called)</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);
				if(disposing)
				{
					lock(_locker)
					{
						_cachedResources.Clear();
						_ids.Clear();
						_knownResources.Clear();
						_originalResources.Clear();
					}
				}
			}
		}
		#endregion
	}
}
