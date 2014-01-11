using System;
using System.Collections;
using System.Collections.Generic;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Node that contains multiple nodes of the same type
	/// </summary>
	public class ListNode : Node, IList<Node>
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.List"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.List; }
		}

		private readonly NodeType _elementType;

		/// <summary>
		/// Node type for each of the contained nodes
		/// </summary>
		public NodeType ElementType
		{
			get { return _elementType; }
		}

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return String.Format("{0} items of type {1}", Count, _elementType); }
		}
		#endregion

		private readonly List<Node> _nodes = new List<Node>();

		/// <summary>
		/// Creates a new empty list node
		/// </summary>
		/// <param name="type">Type of each node that will be in the list</param>
		public ListNode (NodeType type)
		{
			_elementType = type;
		}

		/// <summary>
		/// Creates a new list node with initial contents
		/// </summary>
		/// <param name="type">Type of each node that will be in the list</param>
		/// <param name="nodes">Initial collection of nodes</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="nodes"/> is null.
		/// The initial collection of nodes can't be null.</exception>
		/// <exception cref="ArgumentException">Thrown if a node inside of <paramref name="nodes"/> is null.</exception>
		/// <exception cref="ArrayTypeMismatchException">Thrown if a node inside of <paramref name="nodes"/> does not match the type given by <paramref name="type"/></exception>
		public ListNode (NodeType type, IEnumerable<Node> nodes)
			: this(type)
		{
			if(nodes == null)
				throw new ArgumentNullException("nodes", "The initial collection of nodes can't be null.");
			foreach(var node in nodes)
			{
				if(node == null)
					throw new ArgumentException("None of the initial nodes can be null.", "nodes");
				if(node.Type != type)
					throw new ArrayTypeMismatchException();
				_nodes.Add(node);
			}
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public ListNode CloneNode ()
		{
			var list = new ListNode(_elementType);
			foreach(var node in _nodes)
				list._nodes.Add((Node)node.Clone());
			return list;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public override object Clone ()
		{
			return CloneNode();
		}

		#region Serialization

		/// <summary>
		/// Constructs a list node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed list node</returns>
		internal static ListNode ReadPayload (System.IO.BinaryReader br)
		{
			var type   = (NodeType)br.ReadByte();
			var count  = br.ReadInt32();
			var reader = GetPayloadReader(type);
			var list   = new ListNode(type);
			for(var i = 0; i < count; ++i)
			{
				var node = reader(br);
				list._nodes.Add(node);
			}
			return list;
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			bw.Write((byte)_elementType);
			bw.Write(_nodes.Count);
			foreach(var node in _nodes)
				node.WritePayload(bw);
		}
		#endregion

		/// <summary>
		/// Returns an enumerator that iterates through the nodes
		/// </summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the nodes</returns>
		public IEnumerator<Node> GetEnumerator ()
		{
			return _nodes.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the nodes
		/// </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the nodes</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Adds a node to the list
		/// </summary>
		/// <param name="item">Node to add to the list</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null.
		/// The node to add to the list can't be null.</exception>
		/// <exception cref="ArrayTypeMismatchException">Thrown if the type of node being added does not match the type of the existing nodes in the list</exception>
		public void Add (Node item)
		{
			if(item == null)
				throw new ArgumentNullException("item", "The node to add can't be null.");
			if(item.Type != _elementType)
				throw new ArrayTypeMismatchException();

			_nodes.Add(item);
		}

		/// <summary>
		/// Removes all nodes from the list
		/// </summary>
		public void Clear ()
		{
			_nodes.Clear();
		}

		/// <summary>
		/// Determines whether the list contains a node
		/// </summary>
		/// <param name="item">The node to locate in the list</param>
		/// <returns>True if <paramref name="item"/> is found in the list; otherwise, false</returns>
		public bool Contains (Node item)
		{
			return _nodes.Contains(item);
		}

		/// <summary>
		/// Copies the nodes in the list to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the nodes copied from the list.
		/// The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins</param>
		/// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="array"/> is null</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0</exception>
		/// <exception cref="T:System.ArgumentException">The number of elements in the source list is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/></exception>
		public void CopyTo (Node[] array, int arrayIndex)
		{
			_nodes.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Removes the first occurrence of a specific node from the list
		/// </summary>
		/// <param name="item">The node to remove from the list</param>
		/// <returns>True if <paramref name="item"/> was successfully removed from the list; otherwise, false.
		/// This method also returns false if <paramref name="item"/> is not found in the original list.</returns>
		public bool Remove (Node item)
		{
			return _nodes.Remove(item);
		}

		/// <summary>
		/// Number of nodes contained in the list
		/// </summary>
		public int Count
		{
			get { return _nodes.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether the node is read-only
		/// </summary>
		/// <remarks>This property is always false.</remarks>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Determines the index of a specific node in the list
		/// </summary>
		/// <param name="item">The node to locate in the list</param>
		/// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1</returns>
		public int IndexOf (Node item)
		{
			return _nodes.IndexOf(item);
		}

		/// <summary>
		/// Inserts a node into the list at the specified index
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted</param>
		/// <param name="item">The node to insert into the list</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is not a valid index in the node's list</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null.
		/// The node to insert can't be null.</exception>
		/// <exception cref="ArrayTypeMismatchException">Thrown if the type of node being added does not match the type of the existing nodes in the list</exception>
		public void Insert (int index, Node item)
		{
			if(item == null)
				throw new ArgumentNullException("item", "The new node being inserted can't be null.");
			if(item.Type != _elementType)
				throw new ArrayTypeMismatchException();
			_nodes.Insert(index, item);
		}

		/// <summary>
		/// Removes the node at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the node to remove</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the node's list</exception>
		public void RemoveAt (int index)
		{
			_nodes.RemoveAt(index);
		}

		/// <summary>
		/// Gets or sets the node at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the node to get or set</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is not a valid index in the node's list</exception>
		/// <exception cref="ArgumentNullException">Thrown when attempting to set an element to a null node</exception>
		/// <exception cref="ArrayTypeMismatchException">Thrown if the type of node being set does not match the type of the existing nodes in the list</exception>
		public Node this [int index]
		{
			get { return _nodes[index]; }
			set
			{
				if(value == null)
					throw new ArgumentNullException("value", "The new node being applied can't be null.");
				if(value.Type != _elementType)
					throw new ArrayTypeMismatchException();
				_nodes[index] = value;
			}
		}

		/// <summary>
		/// Gets or sets the node with the specified index
		/// </summary>
		/// <param name="key">The index of the node to get or set</param>
		/// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the index in <paramref name="key"/> is outside the bounds of the list</exception>
		/// <exception cref="ArgumentException">Thrown when attempting to set a node to null</exception>
		public Node this[string key]
		{
			get
			{
				if(key == null)
					throw new ArgumentNullException("key", "The index of the node can't be null");
				if(key.Contains(TraversalPathSeperator))
					return traverseGet(key);
				int index;
				if(Int32.TryParse(key, out index))
				{
					if(index < 0 || index >= Count)
						throw new ArgumentOutOfRangeException("key", "The index of the node to retrieve is out of range");
					return _nodes[index];
				}
				throw new FormatException("Expected a numerical key value");
			}
			set
			{
				if(key == null)
					throw new ArgumentNullException("key", "The index of the node can't be null");
				if(key.Contains(TraversalPathSeperator))
					traverseSet(key, value);
				else
				{
					if(value == null)
						throw new ArgumentException("The new node can't be null.", "value");
					int index;
					if(Int32.TryParse(key, out index))
					{
						if(index < 0 || index >= Count)
							throw new ArgumentOutOfRangeException("key", "The index of the node to set is out of range");
						_nodes[index] = value;
					}
					throw new FormatException("Expected a numerical key value");
				}
			}
		}

		private static readonly char[] _traversalPathSplit = TraversalPathSeperator.ToCharArray();

		/// <summary>
		/// Retrieves a child node from a traversal path
		/// </summary>
		/// <param name="path">Path to the child node</param>
		/// <returns>The child node</returns>
		private Node traverseGet (string path)
		{
			var parts   = path.Split(_traversalPathSplit, 2);
			var name    = parts[0];
			var subPath = parts[1];

			int index;
			if(Int32.TryParse(name, out index))
			{
				if(index < 0 || index >= Count)
					throw new IndexOutOfRangeException("The index of the node is out of range");
				var child = _nodes[index];
				switch(child.Type)
				{
				case NodeType.Complex:
					return ((ComplexNode)child)[subPath];
				case NodeType.List:
					return ((ListNode)child)[subPath];
				default:
					throw new InvalidCastException("The node to traverse must be a list or complex node at: " + name);
				}
			}
			throw new FormatException("Expected a numerical key value");
		}

		/// <summary>
		/// Sets a child node in a traversal path
		/// </summary>
		/// <param name="path">Path to the child node</param>
		/// <param name="node">The new node to set</param>
		private void traverseSet (string path, Node node)
		{
			var parts   = path.Split(_traversalPathSplit, 2);
			var name    = parts[0];
			var subPath = parts[1];

			int index;
			if(Int32.TryParse(name, out index))
			{
				if(index < 0 || index >= Count)
					throw new IndexOutOfRangeException("The index of the node is out of range");
				var child = _nodes[index];
				switch(child.Type)
				{
				case NodeType.Complex:
					((ComplexNode)child)[subPath] = node;
					break;
				case NodeType.List:
					((ListNode)child)[subPath] = node;
					break;
				default:
					throw new InvalidCastException("The node to traverse must be a list or complex node at: " + name);
				}
			}
			throw new FormatException("Expected a numerical key value");
		}

		/// <summary>
		/// Appends the contents of the list as a string.
		/// This method is recursive across node classes and is used to construct a string for complex node structures.
		/// </summary>
		/// <param name="sb">String builder to append to</param>
		/// <param name="depth">Current depth (starting at 0)</param>
		internal override void ToString (System.Text.StringBuilder sb, int depth)
		{
			sb.Append(IndentCharacter, depth);
			base.ToString(sb, depth);
			++depth;

			var digits = Count / 10;
			var format = "{0," + digits + "} ";
			var i = 0;
			foreach(var node in _nodes)
			{
				sb.Append(IndentCharacter, depth);
				sb.AppendFormat(format, i++);
				node.ToString(sb, depth);
				sb.Append('\n');
			}
		}
	}
}
