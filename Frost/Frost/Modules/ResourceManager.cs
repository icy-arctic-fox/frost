using System;
using System.Collections.Generic;
using System.IO;
using Frost.IO.Resources;
using Frost.Utility;

namespace Frost.Modules
{
	/// <summary>
	/// Responsible for tracking all available resources and caching commonly used resources
	/// </summary>
	public class ResourceManager
	{
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
		private readonly Cache<Guid, object> _cachedResources = new Cache<Guid, object>();

		/// <summary>
		/// Adds all resource package files contained in a directory
		/// </summary>
		/// <param name="path">Path to the directory containing the resources</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="path"/> is null</exception>
		/// <exception cref="ApplicationException">Thrown if a resource was found with an ID that conflicts with an existing resource</exception>
		public void AddResourceDirectory (string path)
		{
			if(path == null)
				throw new ArgumentNullException("path", "The path to the resource directory can't be null.");

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
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="filepath"/> is null</exception>
		/// <exception cref="ApplicationException">Thrown if a resource was found with an ID that conflicts with an existing resource</exception>
		public void AddResourcePackage (string filepath)
		{
			if(filepath == null)
				throw new ArgumentNullException("filepath", "The path to the resource package file can't be null.");

			var package = new ResourcePackageReader(filepath);
			AddResourcePackage(package);
		}

		/// <summary>
		/// Adds a resource package that can be referenced to retrieve resources
		/// </summary>
		/// <param name="package">Resource package</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="package"/> is null</exception>
		/// <exception cref="ApplicationException">Thrown if a resource was found with an ID that conflicts with an existing resource</exception>
		public void AddResourcePackage (ResourcePackageReader package)
		{
			if(package == null)
				throw new ArgumentNullException("package", "The package to pull resources from can't be null.");

			lock(_readers)
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
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public byte[] GetResource (string name, bool allowMod = true)
		{
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource can't be null.");

			lock(_readers)
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
		/// Describes a method that transforms a resource from its raw form to a usable resource
		/// </summary>
		/// <typeparam name="TResource">Type of resource that is produced</typeparam>
		/// <param name="info">Information about the resource</param>
		/// <param name="data">Raw data representing the resource</param>
		/// <returns>A transformed resource</returns>
		public delegate TResource ResourceTranformation<out TResource> (ResourcePackageEntry info, byte[] data);

		/// <summary>
		/// Attempts to find and retrieve a resource by a given name
		/// </summary>
		/// <param name="name">Name of the requested resource</param>
		/// <param name="transform">Method used to transform raw resource data into a usable form</param>
		/// <param name="allowMod">When true, allows overwritten (modded) resources to be retrieved</param>
		/// <returns>Transformed resource</returns>
		/// <remarks>This method will cache the transformed resource.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> or <paramref name="transform"/> are null</exception>
		public TResource GetResource<TResource> (string name, ResourceTranformation<TResource> transform, bool allowMod = true)
		{
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource can't be null.");
			if(transform == null)
				throw new ArgumentNullException("transform", "The transformation method can't be null.");

			throw new NotImplementedException();
		}

		/// <summary>
		/// List of the resource packages being used
		/// </summary>
		public IPackageInfo[] Packages
		{
			get
			{
				lock(_readers)
				{
					var info = new IPackageInfo[_readers.Count];
					var i = 0;
					foreach(var reader in _readers)
						info[i++] = reader;
					return info;
				}
			}
		}
	}
}
