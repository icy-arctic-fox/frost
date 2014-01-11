using System;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Flags that indicate options for a resource package file
	/// </summary>
	[Flags]
	public enum ResourcePackageOptions
	{
		/// <summary>
		/// The header's contents are encrypted
		/// </summary>
		EncryptedHeader = 0x01
	}
}
