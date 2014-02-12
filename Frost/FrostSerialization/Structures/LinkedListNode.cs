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
		public LinkedListNode<T> PreviousNode { get; internal set; }

		/// <summary>
		/// Node following this one in the list
		/// </summary>
		public LinkedListNode<T> NextNode { get; internal set; }

		/// <summary>
		/// Indicates whether there are any nodes prior to this one in the list
		/// </summary>
		public bool HasPrevious
		{
			get { return PreviousNode != null; }
		}

		/// <summary>
		/// Indicates whether there are any nodes after this one in the list
		/// </summary>
		public bool HasNext
		{
			get { return NextNode != null; }
		}

		/// <summary>
		/// Value of the node
		/// </summary>
		public T Value { get; set; }

		/// <summary>
		/// Creates a linked list node with the default value
		/// </summary>
		public LinkedListNode ()
		{
			Value = default(T);
		}

		/// <summary>
		/// Creates a linked list node containing an initial value
		/// </summary>
		/// <param name="value">Value of the node</param>
		public LinkedListNode (T value)
		{
			Value = value;
		}

		/// <summary>
		/// Creates a linked list node with links to the next and previous nodes
		/// </summary>
		/// <param name="value">Value of the node</param>
		/// <param name="prev">Previous node in the list</param>
		/// <param name="next">Next node in the list</param>
		/// <remarks><paramref name="prev"/> and <paramref name="next"/> can't be part of an existing list.</remarks>
		/// <exception cref="ArgumentException">Thrown if <paramref name="prev"/> has a following node or <paramref name="next"/> has a previous node</exception>
		public LinkedListNode (T value, LinkedListNode<T> prev, LinkedListNode<T> next)
		{
#if DEBUG
			if(prev != null && prev.HasNext)
				throw new ArgumentException("The previous node is part of an existing list.", "prev");
			if(next != null && next.HasPrevious)
				throw new ArgumentException("The next node is part of an existing list.", "next");
#endif

			Value        = value;
			PreviousNode = prev;
			NextNode     = next;
			if(prev != null)
				prev.NextNode = this;
			if(next != null)
				next.PreviousNode = this;
		}
	}
}
