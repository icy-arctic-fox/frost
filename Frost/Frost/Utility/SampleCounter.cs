namespace Frost.Utility
{
	/// <summary>
	/// Calculates the minimum, maximum, and average of multiple values.
	/// This counter tracks a maximum number of entries and wraps around - creating a running average.
	/// </summary>
	public class SampleCounter
	{
		private readonly object _locker = new object();
		private readonly double[] _measurements;
		private double _min, _max;
		private int _pos;

		/// <summary>
		/// Creates a new sample counter
		/// </summary>
		/// <param name="count">Maximum number of measurements to keep</param>
		public SampleCounter (int count)
		{
			_measurements = new double[count];
		}

		/// <summary>
		/// Current number of measurements
		/// </summary>
		public int Count { get; private set; }

		/// <summary>
		/// Average of all the measurements
		/// </summary>
		public double Average
		{
			get
			{
				var avg = 0d;
				lock(_locker)
					for(var i = 0; i < Count; ++i)
						avg += _measurements[i] / Count;
				return avg;
			}
		}

		/// <summary>
		/// Maximum value of all measurements
		/// </summary>
		public double Maximum
		{
			get
			{
				lock(_locker)
					return _max;
			}
		}

		/// <summary>
		/// Minimum value of all measurements
		/// </summary>
		public double Minimum
		{
			get
			{
				lock(_locker)
					return _min;
			}
		}

		/// <summary>
		/// Adds a measurement to the counter
		/// </summary>
		/// <param name="value">Measurement value</param>
		public void AddMeasurement (double value)
		{
			lock(_locker)
			{
				_measurements[_pos++] = value;
				if(_pos >= _measurements.Length)
					_pos = 0; // Wrap around
				else if(Count < _measurements.Length)
					++Count; // Increment number of measurements

				// Update min/max
				if(value > _max)
					_max = value;
				else if(value < _min)
					_min = value;
			}
		}
	}
}
