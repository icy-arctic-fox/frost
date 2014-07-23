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
		/// Retrieves the contents of the debug overlay line
		/// </summary>
		/// <param name="contents">String to store the debug information in</param>
		public void GetDebugInfo (MutableString contents)
		{
			var managed   = GC.GetTotalMemory(false);
			var allocated = _process.PrivateMemorySize64;
			var working   = _process.WorkingSet64;

			var managedString   = StringUtility.AsByteUnitString(managed);
			var allocatedString = StringUtility.AsByteUnitString(allocated);
			var workingString   = StringUtility.AsByteUnitString(working);

			contents.AppendFormat("{0} managed {1} allocated {2} working", managedString, allocatedString, workingString);
		}
	}
}
