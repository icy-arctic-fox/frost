using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frost.IO.Tnt;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Contains information about a resource in a resource package file
	/// </summary>
	internal class ResourceInfo
	{
		/// <summary>
		/// Name of the resource
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Block offset into the package data section where the resource's data begins
		/// </summary>
		public int BlockOffset { get; set; }

		// TODO: Add other properties

		/// <summary>
		/// Creates a new resource information container
		/// </summary>
		/// <param name="name">Name of the resource</param>
		/// <param name="blockOffset">Block offset into the package data section where the resource's data begins</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null.
		/// The name of the resource can't be null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="blockOffset"/> is less than 0</exception>
		public ResourceInfo (string name, int blockOffset)
		{
			if(name == null)
				throw new ArgumentNullException("name", "The name of the resource can't be null.");
			if(blockOffset < 0)
				throw new ArgumentOutOfRangeException("blockOffset", "The block offset can't be negative.");

			Name        = name;
			BlockOffset = blockOffset;
		}

		/// <summary>
		/// Creates a new resource information container by extracting data from a node
		/// </summary>
		/// <param name="node">Node containing the resource information</param>
		public ResourceInfo (Node node)
		{
			throw new NotImplementedException();
		}
	}
}
