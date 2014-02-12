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
		private readonly LinkedListNode<T> _curNode;
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

		/// <summary>
		/// Creates a linked list enumerator
		/// </summary>
		/// <param name="start">Starting node</param>
		/// <param name="reverse">True to iterate backwards through nodes</param>
		public LinkedListEnumerator (LinkedListNode<T> start, bool reverse = false)
		{
			_curNode = start;
			_reverse = reverse;
		}

		#region List operations

		/// <summary>
		/// Adds an element to the list after the current position of the enumerator
		/// </summary>
		/// <param name="value">Value to insert</param>
		public void AddAfter (T value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds an element to the list before the current position of the enumerator
		/// </summary>
		/// <param name="value">Value to insert</param>
		public void AddBefore (T value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes the node at the current position of the enumerator
		/// </summary>
		public void Remove ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes the node before the current position of the enumerator
		/// </summary>
		public void RemovePrevious ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes the node after the current position of the enumerator
		/// </summary>
		public void RemoveNext ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes all nodes before the current position of the enumerator
		/// </summary>
		/// <remarks>All nodes will be removed if the enumerator is past the end of the list.
		/// If <see cref="Reverse"/> is true, this method removes previous nodes (nodes with a higher index).</remarks>
		public void RemovePrior ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes all nodes after the current position of the enumerator
		/// </summary>
		/// <remarks>All nodes will be removed if the enumerator is before the start of the list.
		/// If <see cref="Reverse"/> is true, this method removes upcoming nodes (nodes with a lower index).</remarks>
		public void RemoveFollowing ()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
