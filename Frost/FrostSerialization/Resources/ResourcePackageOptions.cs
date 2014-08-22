using System;

namespace Frost.Resources
{
	/// <summary>
	/// Flags that indicate options for a resource package file
	/// </summary>
	[Flags]
	public enum ResourcePackageOptions : ushort
	{
		/// <summary>
		/// No optional changes to the resource package file
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// The header's contents are encrypted
		/// </summary>
		EncryptedHeader = 0x0001
	}
}
