using System;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Describes an event when user input has occurred
	/// </summary>
	/// <remarks>Properties in this class are modifiable internally to allowed a single instance to be reused.</remarks>
	public class InputEventArgs : EventArgs
	{
		/// <summary>
		/// Description of the input that occurred
		/// </summary>
		public InputDescriptor Input { get; internal set; }

		/// <summary>
		/// Amount of input for varying "strength" (e.g.: joysticks)
		/// </summary>
		public float Strength { get; internal set; }
	}
}
