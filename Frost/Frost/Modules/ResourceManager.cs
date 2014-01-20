using System;
using System.Collections.Generic;
using System.IO;
using Frost.IO.Resources;

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
		/// Adds all resource package files contained in a directory
		/// </summary>
		/// <param name="path">Path to the directory containing the resources</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="path"/> is null</exception>
		public void AddResourceDirectory (string path)
		{
			if(path == null)
				throw new ArgumentNullException("path", "The path to the resource directory can't be null.");

			foreach(var filepath in Directory.EnumerateFiles(path, "*.frp", SearchOption.AllDirectories))
				AddResourcePackage(filepath);
		}

		/// <summary>
		/// Adds a resource package that can be referenced to retrieve resources
		/// </summary>
		/// <param name="filepath">Path to the resource package file</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="filepath"/> is null</exception>
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
		/// Stores the names of each resource in the package and a reference back to the reader
		/// </summary>
		/// <param name="package">Package to index</param>
		private void indexResourcePackage (ResourcePackageReader package)
		{
			foreach(var resource in package.Resources)
			{
				var name = resource.Name;
				if(_knownResources.ContainsKey(name) && !_originalResources.ContainsKey(name))
					_originalResources.Add(name, _knownResources[name]); // Store original resource
				_knownResources[name] = package;
			}
		}

		/// <summary>
		/// Attempts to find and retrieve a resource by a given name
		/// </summary>
		/// <param name="name">Name of the requested resource</param>
		/// <param name="allowMod">When true, allows overwritten (modded) resources to be retrieved</param>
		/// <returns>Raw data for the resource or null if the resource doesn't exist</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public byte[] GetResource (string name, bool allowMod = true)
		{
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource can't be null.");

			lock(_readers)
			{
				if(!allowMod && _originalResources.ContainsKey(name))
				{ // Don't allow mods and use the original resource
					var reader = _originalResources[name];
					return reader.GetResource(name);
				}
				return _knownResources.ContainsKey(name) ? _knownResources[name].GetResource(name) : null;
			}
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
