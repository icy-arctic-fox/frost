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

		/// <summary>
		/// Creates new header information
		/// </summary>
		/// <param name="ver">Version number</param>
		/// <param name="opts">Options for the resource package</param>
		public HeaderInfo (byte ver, ResourcePackageOptions opts)
		{
			_ver  = ver;
			_opts = opts;
		}
	}
}