namespace Frost.ResourcePackager
{
	/// <summary>
	/// Error codes that the program can return
	/// </summary>
	public enum ReturnCode
	{
		/// <summary>
		/// Everything went ok
		/// </summary>
		Ok = 0,

		/// <summary>
		/// The program was invoked in a way that was unknown
		/// </summary>
		BadUsage = 1,

		/// <summary>
		/// A required source file or directory doesn't exist
		/// </summary>
		MissingSource = 2
	}
}
