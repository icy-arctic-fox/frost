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
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Total number of integers available
		/// </summary>
		public int Available
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Number of integers remaining
		/// </summary>
		public int Remaining
		{
			get { throw new NotImplementedException(); }
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
		public FreeList (int max)
		{
			_max = max;
		}

		/// <summary>
		/// Retrieves the next available integer
		/// </summary>
		/// <returns>Next available integer or -1 if no integers are available</returns>
		public int GetNext ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Releases an integer so that it becomes usable again
		/// </summary>
		/// <param name="value">Integer value to release</param>
		public void Release (int value)
		{
			throw new NotImplementedException();
		}
	}
}
