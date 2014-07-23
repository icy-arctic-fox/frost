using System;
using System.Diagnostics;
using Frost.Utility;

namespace Frost.UI
{
	/// <summary>
	/// Debug overlay line that displays memory usage information
	/// </summary>
	public class MemoryDebugOverlayLine : IDebugOverlayLine
	{
		private readonly Process _process;

		/// <summary>
		/// Creates a new memory debug overlay line
		/// </summary>
		public MemoryDebugOverlayLine ()
		{
			_process = Process.GetCurrentProcess();
		}

		/// <summary>
		/// Triggered when the process is being disposed (which is never)
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Generates the text displayed in the debug overlay
		/// </summary>
		/// <returns>Scene information</returns>
		public override string ToString ()
		{
			var managed   = GC.GetTotalMemory(false);
			var allocated = _process.PrivateMemorySize64;
			var working   = _process.WorkingSet64;

			var managedString   = StringUtility.AsByteUnitString(managed);
			var allocatedString = StringUtility.AsByteUnitString(allocated);
			var workingString   = StringUtility.AsByteUnitString(working);

			return String.Format("{0} managed {1} allocated {2} working", managedString, allocatedString, workingString);
		}
	}
}
