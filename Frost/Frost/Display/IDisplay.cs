namespace Frost.Display
{
	/// <summary>
	/// Describes an object that displays frames on the screen
	/// </summary>
	public interface IDisplay
	{
		/// <summary>
		/// Performs any updates to the display's state (not part of the rendered frames)
		/// </summary>
		void Update ();

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
