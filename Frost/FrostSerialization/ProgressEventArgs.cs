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
		/// <exception cref="ArgumentException">Thrown if <paramref name="max"/> is less than or equal to <paramref name="min"/></exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than <paramref name="min"/> or greater than <paramref name="max"/></exception>
		public ProgressEventArgs (long min, long max, long value)
		{
#if DEBUG
			if(max <= min) // Checking for equality here prevents divide-by-zero in Progress property
				throw new ArgumentException("The maximum value must be larger than the minimum value.");
			if(value < min || value > max)
				throw new ArgumentOutOfRangeException("value", "The current value can't be less than the minimum or larger than the maximum.");
#endif
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
