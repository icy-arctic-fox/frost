using System;

namespace Frost
{
	/// <summary>
	/// Describes the progress of a larger event
	/// </summary>
	public class ProgressEventArgs : EventArgs
	{
		private readonly long _min, _max, _value;

		/// <summary>
		/// Creates a new progress event description
		/// </summary>
		/// <param name="min">Minimum value</param>
		/// <param name="max">Maximum value</param>
		/// <param name="value">Current value</param>
		public ProgressEventArgs (long min, long max, long value)
		{
			_min   = min;
			_max   = max;
			_value = value;
		}

		/// <summary>
		/// Minimum value
		/// </summary>
		public long Minimum
		{
			get { return _min; }
		}

		/// <summary>
		/// Maximum value
		/// </summary>
		public long Maximum
		{
			get { return _max; }
		}

		/// <summary>
		/// Current value
		/// </summary>
		public long Value
		{
			get { return _value; }
		}

		/// <summary>
		/// Progress from <see cref="Minimum"/> to <see cref="Maximum"/>
		/// </summary>
		public double Progress
		{
			get { return (double)_value / (_max - _min); }
		}
	}
}
