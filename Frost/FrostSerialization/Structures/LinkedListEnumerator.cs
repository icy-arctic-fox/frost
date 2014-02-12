using System;
using System.Collections;
using System.Collections.Generic;

namespace Frost.Structures
{
	/// <summary>
	/// Enumerates nodes in a linked list
	/// </summary>
	/// <typeparam name="T">Type of each node value</typeparam>
	/// <remarks>This enumerator provides methods for modifying nodes in the linked list as it traverses it.
	/// However, care should be taken that the list is not modified unexpectedly elsewhere.</remarks>
	public class LinkedListEnumerator<T> : IEnumerator<T>
	{
		private readonly LinkedList<T> _list;

		/// <summary>
		/// List being enumerated through
		/// </summary>
		public LinkedList<T> List
		{
			get { return _list; }
		}

		private readonly bool _reverse;

		/// <summary>
		/// Indicates whether the enumerator moves backwards through the list from end to start
		/// </summary>
		public bool Reverse
		{
			get { return _reverse; }
		}

		/// <summary>
		/// Advances the enumerator to the next element of the list
		/// </summary>
		/// <returns>True if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the list</returns>
		public bool MoveNext ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Sets the enumerator to its initial position, which is before the first element or after the last element in the collection
		/// </summary>
		public void Reset ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the element in the list at the current position of the enumerator
		/// </summary>
		public T Current
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the current element in the list
		/// </summary>
		object IEnumerator.Current
		{
			get { return Current; }
		}

		/// <summary>
		/// Releases resources held by the enumerator
		/// </summary>
		public void Dispose ()
		{
			// ...
		}
	}
}
