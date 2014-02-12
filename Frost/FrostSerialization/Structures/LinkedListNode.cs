using System;

namespace Frost.Structures
{
	/// <summary>
	/// Node in a doubly-linked list
	/// </summary>
	/// <typeparam name="T">Type of element</typeparam>
	public sealed class LinkedListNode<T>
	{
		/// <summary>
		/// Node prior to this one in the list
		/// </summary>
		public LinkedListNode<T> Previous { get; internal set; }

		/// <summary>
		/// Node following this one in the list
		/// </summary>
		public LinkedListNode<T> Next { get; internal set; }

		private readonly LinkedList<T> _list;

		/// <summary>
		/// List that the node belongs to
		/// </summary>
		public LinkedList<T> List
		{
			get { return _list; }
		}

		/// <summary>
		/// Indicates whether there are any nodes prior to this one in the list
		/// </summary>
		public bool HasPrevious
		{
			get { return Previous != null; }
		}

		/// <summary>
		/// Indicates whether there are any nodes after this one in the list
		/// </summary>
		public bool HasNext
		{
			get { return Next != null; }
		}

		/// <summary>
		/// Value of the node
		/// </summary>
		public T Value { get; set; }

		/// <summary>
		/// Creates a linked list node with the default value
		/// </summary>
		/// <param name="list">List that the node belongs to</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> is null</exception>
		internal LinkedListNode (LinkedList<T> list)
		{
#if DEBUG
			if(list == null)
				throw new ArgumentNullException("list", "The parent linked list can't be null.");
#endif
			_list = list;
			Value = default(T);
		}

		/// <summary>
		/// Creates a linked list node containing an initial value
		/// </summary>
		/// <param name="list">List that the node belongs to</param>
		/// <param name="value">Value of the node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> is null</exception>
		internal LinkedListNode (LinkedList<T> list, T value)
		{
#if DEBUG
			if(list == null)
				throw new ArgumentNullException("list", "The parent linked list can't be null.");
#endif
			_list = list;
			Value = value;
		}

		/// <summary>
		/// Creates a linked list node with links to the next and previous nodes
		/// </summary>
		/// <param name="list">List that the node belongs to</param>
		/// <param name="value">Value of the node</param>
		/// <param name="prev">Previous node in the list</param>
		/// <param name="next">Next node in the list</param>
		/// <remarks><paramref name="prev"/> and <paramref name="next"/> can't be part of an existing list.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> is null</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="prev"/> has a following node or <paramref name="next"/> has a previous node</exception>
		internal LinkedListNode (LinkedList<T> list, T value, LinkedListNode<T> prev, LinkedListNode<T> next)
			: this(list, value)
		{
#if DEBUG
			if(prev != null && (prev.HasNext || prev._list != list))
				throw new ArgumentException("The previous node is part of a different list.", "prev");
			if(next != null && (next.HasPrevious || next._list != list))
				throw new ArgumentException("The next node is part of a different list.", "next");
#endif

			Previous = prev;
			Next     = next;
			if(prev != null)
				prev.Next = this;
			if(next != null)
				next.Previous = this;
		}
	}
}
