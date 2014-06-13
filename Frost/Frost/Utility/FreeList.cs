using System;
using System.Collections.Generic;

namespace Frost.Utility
{
	/// <summary>
	/// Tracks integer values that are in use.
	/// Used to get and reuse integer values.
	/// Released integers are reused first before acquiring new values.
	/// </summary>
	public class FreeList
	{
		/// <summary>
		/// Released integers available for reuse
		/// </summary>
		private readonly Queue<int> _released = new Queue<int>();

		/// <summary>
		/// Maximum allowed integer
		/// </summary>
		private readonly int _max;

		/// <summary>
		/// Maximum integer acquired
		/// </summary>
		private volatile int _top;

		/// <summary>
		/// Number of integers acquired
		/// </summary>
		public int Acquired
		{
			get { return _top - _released.Count; }
		}

		/// <summary>
		/// Total number of integers available
		/// </summary>
		public int Total
		{
			get { return _max; }
		}

		/// <summary>
		/// Number of integers remaining
		/// </summary>
		public int Remaining
		{
			get { return _max - _top + _released.Count; }
		}

		/// <summary>
		/// Creates a free list using all positive integers
		/// </summary>
		public FreeList ()
		{
			_max = Int32.MaxValue;
		}

		/// <summary>
		/// Creates a free list with a maximum upper bound
		/// </summary>
		/// <param name="max">Maximum number of integers allowed</param>
		/// <exception cref="ArgumentOutOfRangeException">The maximum number of integers must be zero or more.</exception>
		public FreeList (int max)
		{
			if(max < 0)
				throw new ArgumentOutOfRangeException("max");

			_max = max;
		}

		/// <summary>
		/// Retrieves the next available integer
		/// </summary>
		/// <returns>Next available integer or -1 if no integers are available</returns>
		public int GetNext ()
		{
			if(_released.Count > 0)
				return _released.Dequeue(); // Reuse a released value

			if(_top >= _max)
				return -1; // No values remaining

			return _top++;
		}

		/// <summary>
		/// Releases an integer so that it becomes usable again
		/// </summary>
		/// <param name="value">Integer value to release</param>
		public void Release (int value)
		{
			if(value > _top || _released.Contains(value))
				return; // Invalid value to be released

			_released.Enqueue(value);
		}
	}
}
