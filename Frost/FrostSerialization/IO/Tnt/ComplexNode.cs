using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Contains multiple nodes of different types and addresses them by name
	/// </summary>
	public class ComplexNode : Node, IDictionary<string, Node>
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Complex"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Complex; }
		}

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return String.Format("{0} entries", Count); }
		}
		#endregion

		/// <summary>
		/// Checks if a string is valid for a node name
		/// </summary>
		/// <param name="name">String to check</param>
		/// <returns>True if the string is valid for a node name</returns>
		/// <remarks>Valid node names are not null, empty, contain only whitespace, or forward slashes.</remarks>
		private static bool isValidNodeName (string name)
		{
			return !(String.IsNullOrWhiteSpace(name) || name.Contains("/"));
		}

		private readonly Dictionary<string, Node> _nodes = new Dictionary<string, Node>();

		/// <summary>
		/// Creates a new empty complex node
		/// </summary>
		public ComplexNode ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new complex node with initial contents
		/// </summary>
		/// <param name="nodes">Initial nodes to add</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="nodes"/> is null.
		/// The initial collection of nodes can't be null.</exception>
		/// <exception cref="ArgumentException">Thrown if one of the nodes in <paramref name="nodes"/> is null or has an invalid name</exception>
		public ComplexNode (IEnumerable<KeyValuePair<string, Node>> nodes)
		{
			if(nodes == null)
				throw new ArgumentNullException("nodes", "The collection of initial nodes can't be null.");
			foreach(var entry in nodes)
			{
				var name = entry.Key;
				var node = entry.Value;
				if(!isValidNodeName(name))
					throw new ArgumentException("Invalid node name - " + (name ?? "null"), "nodes");
				if(node == null)
					throw new ArgumentException("Cannot add a null node", "nodes");
				_nodes.Add(name, node);
			}
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public ComplexNode CloneNode ()
		{
			var complex = new ComplexNode();
			foreach(var entry in _nodes)
				complex._nodes.Add(entry.Key, (Node)entry.Value.Clone());
			return complex;
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
		/// Constructs a complex node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed complex node</returns>
		internal static ComplexNode ReadPayload (System.IO.BinaryReader br)
		{
			var complex = new ComplexNode();
			while(true)
			{
				var node = ReadFromStream(br);
				if(node == null)
					break; // End marker
				var name = br.ReadString();
				complex._nodes.Add(name, node);
			}
			return complex;
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			foreach(var entry in _nodes)
			{
				var name = entry.Key;
				var node = entry.Value;
				node.WriteToStream(bw);
				bw.Write(name);
			}
			bw.Write((byte)NodeType.End);
		}
		#endregion

		/// <summary>
		/// Returns an enumerator that iterates through the nodes
		/// </summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the nodes</returns>
		public IEnumerator<KeyValuePair<string, Node>> GetEnumerator ()
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
		/// Adds a node to the collection
		/// </summary>
		/// <param name="item">The node to add to the collection</param>
		/// <exception cref="ArgumentException">Thrown if the name of the node is null</exception>
		/// <exception cref="ArgumentNullException">Thrown if the node to add is null</exception>
		public void Add (KeyValuePair<string, Node> item)
		{
			Add(item.Key, item.Value);
		}

		/// <summary>
		/// Removes all nodes from the collection
		/// </summary>
		public void Clear ()
		{
			_nodes.Clear();
		}

		/// <summary>
		/// Determines whether the collection contains a specific node
		/// </summary>
		/// <returns>True if <paramref name="item"/> is found in the collection; otherwise, false</returns>
		/// <param name="item">The node to locate in the collection</param>
		/// <exception cref="ArgumentException">Thrown if the name of the node is null</exception>
		/// <exception cref="ArgumentNullException">Thrown if the node to add is null</exception>
		public bool Contains (KeyValuePair<string, Node> item)
		{
			var name = item.Key;
			var node = item.Value;
			if(name == null)
				throw new ArgumentException("The name of the node can't be null.", "item");
			if(node == null)
				throw new ArgumentNullException("item", "The node to add can't be null.");
			return _nodes.ContainsKey(name) && _nodes[name] == node;
		}

		/// <summary>
		/// Copies the nodes to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the nodes copied from the collection.
		/// The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins</param>
		/// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="array"/> is null</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Thrown if <paramref name="arrayIndex"/> is less than 0</exception>
		/// <exception cref="T:System.ArgumentException">The number of nodes in the source collection is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/></exception>
		public void CopyTo (KeyValuePair<string, Node>[] array, int arrayIndex)
		{
			if(array == null)
				throw new ArgumentNullException("array", "The array to copy nodes to can't be null.");
			if(arrayIndex < 0)
				throw new ArgumentOutOfRangeException("arrayIndex", "The index to start at in the array can't be less than 0.");
			if(array.Length - arrayIndex < Count)
				throw new ArgumentException("Not enough elements in the destination array.", "array");

			var i = 0;
			foreach(var entry in _nodes)
				array[arrayIndex + i++] = entry;
		}

		/// <summary>
		/// Removes the first occurrence of a specific node from the collection
		/// </summary>
		/// <returns>True if <paramref name="item"/> was successfully removed from the collection; otherwise, false.
		/// This method also returns false if <paramref name="item"/> is not found in the original collection.</returns>
		/// <param name="item">The node to remove from the collection</param>
		public bool Remove (KeyValuePair<string, Node> item)
		{
			var name = item.Key;
			var node = item.Value;
			if(name == null)
				throw new ArgumentException("The name of the node can't be null.", "item");
			if(node == null)
				throw new ArgumentNullException("item", "The node to remove can't be null.");
			if(_nodes.ContainsKey(name) && node == _nodes[name])
				return _nodes.Remove(name);
			return false;
		}

		/// <summary>
		/// Attempts to find and remove a node from the collection
		/// </summary>
		/// <returns>True if <paramref name="item"/> was successfully removed from the collection; otherwise, false.
		/// This method also returns false if <paramref name="item"/> is not found in the original collection.</returns>
		/// <param name="item">The node to remove from the collection</param>
		public bool Remove (Node item)
		{
			if(item == null)
				throw new ArgumentNullException("item", "The node to remove can't be null.");
			var name = (from entry in _nodes where entry.Value == item select entry.Key).FirstOrDefault();
			if(name != null)
			{// Found the node
				_nodes.Remove(name);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the number of nodes contained in the collection
		/// </summary>
		public int Count
		{
			get { return _nodes.Count; }
		}

		/// <summary>
		/// Indicates whether the node is read-only
		/// </summary>
		/// <remarks>This property always returns false.</remarks>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Determines whether the collection contains a node with the specified name
		/// </summary>
		/// <returns>True if the collection contains a node with the name; otherwise, false</returns>
		/// <param name="key">The name to locate in the collection</param>
		/// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="key"/> is null</exception>
		public bool ContainsKey (string key)
		{
			if(key == null)
				throw new ArgumentException("The name of the node can't be null.", "key");
			return _nodes.ContainsKey(key);
		}

		/// <summary>
		/// Adds a node with the provided name to the collection
		/// </summary>
		/// <param name="key">The name to use for the node to add</param>
		/// <param name="value">The node to add</param>
		/// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="key"/> or <paramref name="value"/> is null.</exception>
		/// <exception cref="T:System.ArgumentException">A node with the same name already exists in the collection</exception>
		public void Add (string key, Node value)
		{
			if(key == null)
				throw new ArgumentException("The name of the node can't be null.", "key");
			if(value == null)
				throw new ArgumentNullException("value", "The node to add can't be null.");
			_nodes.Add(key, value);
		}

		/// <summary>
		/// Removes the node with the specified name from the collection
		/// </summary>
		/// <returns>True if the node is successfully removed; otherwise, false.
		/// This method also returns false if <paramref name="key"/> was not found in the original collection.
		/// </returns>
		/// <param name="key">The name of the node to remove</param>
		/// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="key"/> is null</exception>
		public bool Remove (string key)
		{
			if(key == null)
				throw new ArgumentException("The name of the node can't be null.", "key");
			return _nodes.Remove(key);
		}

		/// <summary>
		/// Gets the node associated with the specified name
		/// </summary>
		/// <returns>True if the collection contains a node with the specified name; otherwise, false</returns>
		/// <param name="key">The name of the node to get</param>
		/// <param name="value">When this method returns, the node associated with the specified name, if the name is found; otherwise, null</param>
		/// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="key"/> is null</exception>
		public bool TryGetValue (string key, out Node value)
		{
			return _nodes.TryGetValue(key, out value);
		}

		/// <summary>
		/// Gets or sets the node with the specified name
		/// </summary>
		/// <param name="key">The name of the node to get or set</param>
		/// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
		/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found.</exception>
		/// <exception cref="ArgumentException">Thrown when attempting to set a node to null</exception>
		public Node this[string key]
		{
			get
			{
				if(key == null)
					throw new ArgumentNullException("key", "The name of the node can't be null.");
				return key.Contains(TraversalPathSeperator) ? traverseGet(key) : _nodes[key];
			}
			set
			{
				if(key == null)
					throw new ArgumentNullException("key", "The name of the node can't be null.");
				if(key.Contains(TraversalPathSeperator))
					traverseSet(key, value);
				else
				{
					if(value == null)
						throw new ArgumentException("The new node can't be null.", "value");
					_nodes[key] = value;
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
			if(_nodes.ContainsKey(name))
			{
				var child = _nodes[name];
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
			throw new KeyNotFoundException("No child node with the name " + name + " exists");
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
			if(_nodes.ContainsKey(parts[0]))
			{
				var child = _nodes[name];
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
			throw new KeyNotFoundException("No child node with the name " + name + " exists");
		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the names of the nodes in the collection
		/// </summary>
		/// <returns>A collection containing the names of the nodes</returns>
		public ICollection<string> Keys
		{
			get { return _nodes.Keys; }
		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the nodes in the collection
		/// </summary>
		/// <returns>A collection containing the nodes</returns>
		public ICollection<Node> Values
		{
			get { return _nodes.Values; }
		}

		/// <summary>
		/// Appends the contents of the collection as a string.
		/// This method is recursive across node classes and is used to construct a string for complex node structures.
		/// </summary>
		/// <param name="sb">String builder to append to</param>
		/// <param name="depth">Current depth (starting at 0)</param>
		internal override void ToString (System.Text.StringBuilder sb, int depth)
		{
			sb.Append(IndentCharacter, depth);
			base.ToString(sb, depth);
			++depth;

			var spacing = _nodes.Keys.Select(name => name.Length).Max();
			var format  = "{0," + spacing + "} ";
			foreach(var entry in _nodes)
			{
				var name = entry.Key;
				var node = entry.Value;
				sb.Append(IndentCharacter, depth);
				sb.AppendFormat(format, name);
				node.ToString(sb, depth);
				sb.Append('\n');
			}
		}
	}
}
