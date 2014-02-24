using System;
using System.Diagnostics;

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
			var used      = GC.GetTotalMemory(false);
			var allocated = _process.PrivateMemorySize64;
			var working   = _process.WorkingSet64;

			var usedString      = toByteString(used);
			var allocatedString = toByteString(allocated);
			var workingString   = toByteString(working);

			return String.Format("{0} used {1} allocated {2} working", usedString, allocatedString, workingString);
		}

		private static readonly string[] _units = new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

		/// <summary>
		/// Creates a friendly string from a number of bytes
		/// </summary>
		/// <param name="bytes">Number of bytes</param>
		/// <returns>Reduced bytes with units</returns>
		private static string toByteString(long bytes)
		{
			var unitIndex = 0;
			var b = (double)bytes;
			while (b > 1000d)
			{
				b /= 1024d;
				++unitIndex;
			}
			var unit = _units[unitIndex];
			return String.Format("{0:0.00} {1}", b, unit);
		}
	}
}
