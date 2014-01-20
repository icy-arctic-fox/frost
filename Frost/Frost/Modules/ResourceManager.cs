using System;
using System.Collections.Generic;
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
		/// Adds a resource package that can be referenced to retrieve resources
		/// </summary>
		/// <param name="filepath">Path to the resource package file</param>
		public void AddResourcePackage (string filepath)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds a resource package that can be referenced to retrieve resources
		/// </summary>
		/// <param name="package">Resource package</param>
		public void AddResourcePackage (ResourcePackageReader package)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Attempts to find and retrieve a resource by a given name
		/// </summary>
		/// <param name="name">Name of the requested resource</param>
		/// <param name="allowMod">When true, allows overwritten (modded) resources to be retrieved</param>
		/// <returns>Raw data for the resource</returns>
		public byte[] GetResource (string name, bool allowMod = true)
		{
			throw new NotImplementedException();
		}
	}
}
