namespace Frost.Utility
{
	/// <summary>
	/// Calculates the minimum, maximum, and average of multiple values.
	/// This counter tracks a maximum number of entries and wraps around - creating a running average.
	/// </summary>
	public class SampleCounter
	{
		private readonly double[] _measurements;
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
				lock(_measurements)
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
				var max = _measurements[0];
				lock(_measurements)
					for(var i = 1; i < Count; ++i)
						if(_measurements[i] > max)
							max = _measurements[i];
				return max;
			}
		}

		/// <summary>
		/// Minimum value of all measurements
		/// </summary>
		public double Minimum
		{
			get
			{
				var min = _measurements[0];
				lock(_measurements)
					for(var i = 1; i < Count; ++i)
						if(_measurements[i] < min)
							min = _measurements[i];
				return min;
			}
		}

		/// <summary>
		/// Adds a measurement to the counter
		/// </summary>
		/// <param name="measurement">Measurement value</param>
		public void AddMeasurement (double measurement)
		{
			lock(_measurements)
			{
				_measurements[_pos++] = measurement;
				if(_pos >= _measurements.Length)
					_pos = 0; // Wrap around
				else if(Count < _measurements.Length)
					++Count; // Increment number of measurements
			}
		}
	}
}
