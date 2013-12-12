namespace Frost.Display
{
	/// <summary>
	/// Describes an object that displays frames on the screen
	/// </summary>
	public interface IDisplay
	{
		/// <summary>
		/// Starts a new frame
		/// </summary>
		void EnterFrame ();

		/// <summary>
		/// Ends drawing actions for the current frame and displays it
		/// </summary>
		void ExitFrame ();
	}
}
