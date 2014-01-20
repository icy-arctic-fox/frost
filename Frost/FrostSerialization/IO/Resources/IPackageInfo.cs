namespace Frost.IO.Resources
{
	/// <summary>
	/// Information about a package
	/// </summary>
	public interface IPackageInfo
	{
		/// <summary>
		/// Displayed name of the package
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Information about the creator of the package.
		/// This contains their name and possibly an email or web address.
		/// </summary>
		string Creator { get; }

		/// <summary>
		/// Brief description of what the package is for
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Size of the package in bytes
		/// </summary>
		long Size { get; }
	}
}
