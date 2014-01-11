using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Provides access to existing resource package files.
	/// Resource packages can contain many resources of any type.
	/// Resources contained in the package files can also be encrypted and compressed.
	/// </summary>
	public class ResourcePackage
	{
		/// <summary>
		/// Default size of each block in bytes
		/// </summary>
		private const int DefaultBlockSize = 1024 * 4; // 4 KB

		private readonly byte _ver;
		private readonly int _blockSize;

		/// <summary>
		/// Version of the resource package
		/// </summary>
		public byte Version
		{
			get { return _ver; }
		}

		private ResourcePackage (byte version, int blockSize, ResourcePackageOptions options, BinaryReader br)
		{
			throw new NotImplementedException();
		}
	}
}
