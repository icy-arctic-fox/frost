namespace Frost.ResourcePackager
{
	/// <summary>
	/// Error codes that the program can return
	/// </summary>
	public enum ReturnCode : int
	{
		/// <summary>
		/// Everything went ok
		/// </summary>
		Ok = 0,

		/// <summary>
		/// The program was invoked in a way that was unknown
		/// </summary>
		BadUsage = 1
	}
}
