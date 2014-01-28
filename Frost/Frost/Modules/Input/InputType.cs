namespace Frost.Modules.Input
{
	/// <summary>
	/// Types of input devices
	/// </summary>
	public enum InputType
	{
		// TODO: Add unassigned type

		/// <summary>
		/// Keyboard keys
		/// </summary>
		Keyboard = 0,

		/// <summary>
		/// Mouse buttons and movement
		/// </summary>
		Mouse,

		/// <summary>
		/// Joystick positions and buttons
		/// </summary>
		Joystick,

		/// <summary>
		/// Number of input types
		/// </summary>
		/// <remarks>Keep this entry last.</remarks>
		Count
	}
}
