using System;
using System.Collections;
using System.Collections.Generic;

namespace Frost.Structures
{
	/// <summary>
	/// Doubly-linked list generic structure.
	/// Uses nodes with links to store data.
	/// Accessing elements by index is an O(n) operation,
	/// but inserting, removing, and iterating elements are O(1) operations.
	/// </summary>
	/// <typeparam name="T">Type of each element</typeparam>
	/// <remarks>The purpose of creating this class instead of using .NET's <see cref="T:System.Collections.Generic.LinkedList`1"/>
	/// is because it was limited in some areas.
	/// This class has the same functionality, but expands on a linked list's possibilities.</remarks>
	public class LinkedList<T> : IList<T>, ICloneable
	{
		/// <summary>
		/// Creates an enumerator that iterates through nodes in the list
		/// </summary>
		/// <returns>An enumerator used iterate though node values</returns>
		public IEnumerator<T> GetEnumerator ()
		{
			return GetEnumerator(false);
		}

		/// <summary>
		/// Creates an enumerator that iterates through nodes in the list
		/// </summary>
		/// <param name="reverse">When true, the enumerator will start at the end and iterate backwards</param>
		/// <returns>An enumerator used to iterate through node values</returns>
		public LinkedListEnumerator<T> GetEnumerator (bool reverse)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates an enumerator that iterates through nodes in the list
		/// </summary>
		/// <returns>An enumerator used to iterate through node values</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Adds an item to the end of the list
		/// </summary>
		/// <param name="item">The object to add to the list</param>
		/// <exception cref="T:System.NotSupportedException">The linked list is read-only</exception>
		public void Add (T item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes all items from the list
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The linked list is read-only</exception>
		public void Clear ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether the list contains a specific value
		/// </summary>
		/// <returns>True if <paramref name="item"/> is found in the linked list; otherwise, false</returns>
		/// <param name="item">The object to locate in the list</param>
		/// <remarks>This operation takes O(n) time.</remarks>
		public bool Contains (T item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Copies the elements of the list to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0</exception>
		/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/></exception>
		public void CopyTo (T[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <returns>True if <paramref name="item"/> was successfully removed from the linked list; otherwise, false.
		/// This method also returns false if <paramref name="item"/> is not found in the original list.</returns>
		/// <param name="item">The object to remove from the list</param>
		/// <exception cref="T:System.NotSupportedException">The linked list is read-only</exception>
		/// <remarks>This operation takes O(n) time.
		/// If the node containing the value is known, <see cref="RemoveNode"/> is more ideal.</remarks>
		public bool Remove (T item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Number of elements contained in the list
		/// </summary>
		/// <remarks>This value is tracked so that the operation takes O(1) time.</remarks>
		public int Count
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Indicates whether the list is read-only
		/// </summary>
		public bool IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1</returns>
		/// <param name="item">The object to locate in the list</param>
		/// <remarks>This operation takes O(n) time.</remarks>
		public int IndexOf (T item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Inserts an item into the list at the specified index
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted</param>
		/// <param name="item">The object to insert into the list</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the list</exception>
		/// <exception cref="T:System.NotSupportedException">The list is read-only</exception>
		/// <remarks>This operation takes O(n) time.
		/// If the node containing the value is known, methods like <see cref="InsertBefore"/> and <see cref="InsertAfter"/> will be more beneficial.</remarks>
		public void Insert (int index, T item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the list</exception>
		/// <exception cref="T:System.NotSupportedException">The list is read-only</exception>
		/// <remarks>This operation takes O(n) time.
		/// If the node containing the value is known, methods like <see cref="RemoveNode"/>, <see cref="RemoveBefore"/>, and <see cref="RemoveAfter"/> will be more beneficial.</remarks>
		public void RemoveAt (int index)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets or sets the element at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the list</exception>
		/// <exception cref="T:System.NotSupportedException">The property is set and the list is read-only.</exception>
		/// <remarks>This operation takes O(n) time.</remarks>
		public T this [int index]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// First node in the list
		/// </summary>
		public LinkedListNode<T> First
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Last node in the list
		/// </summary>
		public LinkedListNode<T> Last
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Clones the elements in the list
		/// </summary>
		/// <returns>A cloned linked list</returns>
		/// <remarks>Only a shallow copy is performed, however the nodes are copied.
		/// Copying the nodes prevents two lists from conflicting with each other.</remarks>
		public LinkedList<T> CloneList ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Clones the elements in the list
		/// </summary>
		/// <returns>A cloned linked list</returns>
		/// <remarks>Only a shallow copy is performed, however the nodes are copied.
		/// Copying the nodes prevents two lists from conflicting with each other.</remarks>
		public object Clone ()
		{
			return CloneList();
		}

		/// <summary>
		/// Creates a copy of the linked list that is read-only
		/// </summary>
		/// <returns>A copy of the linked list with read-only status</returns>
		/// <remarks>A shallow copy is performed and the nodes are not cloned.
		/// This allows the original list that isn't read-only to be updated, which in turn updates all read-only copies.</remarks>
		public LinkedList<T> CreateReadOnly ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Inserts a new node before an existing one
		/// </summary>
		/// <param name="node">Node to insert before</param>
		/// <param name="value">Value to insert</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <remarks>This operation takes O(1) time.</remarks>
		public void InsertBefore (LinkedListNode<T> node, T value)
		{
			// TODO: How do we know that [node] is in this list?
			throw new NotImplementedException();
		}

		/// <summary>
		/// Inserts a new node after an existing one
		/// </summary>
		/// <param name="node">Node to insert after</param>
		/// <param name="value">Value to insert</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <remarks>This operation takes O(1) time.</remarks>
		public void InsertAfter (LinkedListNode<T> node, T value)
		{
			// TODO: How do we know that [node] is in this list?
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Inserts a new node at the start of the list
		/// </summary>
		/// <param name="value">Value to insert</param>
		/// <remarks>This operation takes O(1) time.</remarks>
		public void AddFirst (T value)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Inserts a new node at the end of the list
		/// </summary>
		/// <param name="value">Value to insert</param>
		/// <remarks>This operation takes O(1) time.</remarks>
		public void AddLast (T value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a node from the list
		/// </summary>
		/// <param name="node">Node to remove</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <remarks>This operation takes O(1) time.</remarks>
		public void RemoveNode (LinkedListNode<T> node)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Removes a node prior to <see cref="node"/>
		/// </summary>
		/// <param name="node">Node to remove before</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <remarks>This operation takes O(1) time.</remarks>
		public void RemoveBefore (LinkedListNode<T> node)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a node following <see cref="node"/>
		/// </summary>
		/// <param name="node">Node to remove after</param>
		/// <remarks>This operation takes O(1) time.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		public void RemoveAfter (LinkedListNode<T> node)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes all nodes following, but not including <see cref="node"/>
		/// </summary>
		/// <param name="node">Node to remove after</param>
		/// <remarks>This operation takes O(1) time.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		public void RemoveFollowing (LinkedListNode<T> node)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes all nodes prior to, but not including <see cref="node"/>
		/// </summary>
		/// <param name="node">Node to remove before</param>
		/// <remarks>This operation takes O(1) time.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		public void RemovePrior (LinkedListNode<T> node)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a node from the list at a given index
		/// </summary>
		/// <param name="index">Index of the node to retrieve</param>
		/// <returns>A linked list node</returns>
		/// <exception cref="IndexOutOfRangeException">Thrown if <paramref name="index"/> is outside the bounds of the list</exception>
		public LinkedListNode<T> GetNodeAt (int index)
		{
			throw new NotImplementedException();
		}
	}
}
