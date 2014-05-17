using System;
using Frost.Resources;

namespace Frost.ResourcePackagerGui
{
	class MockResourcePackage : IPackageInfo
	{
		/// <summary>
		/// Displayed name of the package
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Information about the creator of the package.
		/// This contains their name and possibly an email or web address.
		/// </summary>
		public string Creator { get; set; }

		/// <summary>
		/// Brief description of what the package is for
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Size of the package in bytes
		/// </summary>
		public long Size
		{
			get { throw new NotSupportedException(); }
		}
	}
}
