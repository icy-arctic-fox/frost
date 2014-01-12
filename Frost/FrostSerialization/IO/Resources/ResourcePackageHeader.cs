namespace Frost.IO.Resources
{
	/// <summary>
	/// Raw information about the structure of the file from the resource package file's header
	/// </summary>
	internal struct HeaderInfo
	{
		private readonly byte _ver;

		/// <summary>
		/// Resource package version
		/// </summary>
		public byte Version
		{
			get { return _ver; }
		}

		private readonly ResourcePackageOptions _opts;

		/// <summary>
		/// Flags that specify options for the resource package
		/// </summary>
		public ResourcePackageOptions Options
		{
			get { return _opts; }
		}

		private readonly byte _kbCount;

		/// <summary>
		/// Number of kilobytes contained in each block starting at 0 (0 means 1 KB, 255 means 256 KB)
		/// </summary>
		public byte KbCount
		{
			get { return _kbCount; }
		}

		/// <summary>
		/// Creates new header information
		/// </summary>
		/// <param name="ver">Version number</param>
		/// <param name="opts">Options for the resource package</param>
		/// <param name="kbCount">Number of kilobytes contained in each block starting at 0 (0 means 1 KB, 255 means 256 KB)</param>
		public HeaderInfo (byte ver, ResourcePackageOptions opts, byte kbCount)
		{
			_ver = ver;
			_opts = opts;
			_kbCount = kbCount;
		}
	}
}