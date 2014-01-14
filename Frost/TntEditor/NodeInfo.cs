using System;
using System.Globalization;
using Frost.IO.Tnt;

namespace Frost.TntEditor
{
	/// <summary>
	/// Additional information about a node relative to other nodes
	/// </summary>
	class NodeInfo
	{
		private readonly Node _node;
		
		/// <summary>
		/// Actual node
		/// </summary>
		public Node Node
		{
			get { return _node; }
		}

		private readonly NodeInfo _parent;

		/// <summary>
		/// Parent node reference (can be null)
		/// </summary>
		public NodeInfo Parent
		{
			get { return _parent; }
		}

		/// <summary>
		/// Parent's node (will be null if there's no parent)
		/// </summary>
		public Node ParentNode
		{
			get { return (_parent == null) ? null : _parent.Node; }
		}

		/// <summary>
		/// Checks if the node has a parent
		/// </summary>
		public bool HasParent
		{
			get { return _parent != null; }
		}

		/// <summary>
		/// Checks if the node is the root node
		/// </summary>
		public bool IsRootNode
		{
			get { return _parent == null; }
		}

		/// <summary>
		/// Name or index of the node as a string
		/// </summary>
		public string Name
		{
			get { return getNodeName(); }
		}

		/// <summary>
		/// Path to the node delimited by slashes
		/// </summary>
		public string Path
		{
			get { return getNodePath(); }
		}
		
		/// <summary>
		/// Creates new node information
		/// </summary>
		/// <param name="node">Node to refer to</param>
		/// <param name="parent">Parent node (if any)</param>
		public NodeInfo (Node node, NodeInfo parent = null)
		{
			if(node == null)
				throw new ArgumentNullException("node", "The node to refer to can't be null.");
			_node   = node;
			_parent = parent;
		}

		/// <summary>
		/// Figures out what the node's name is by looking at the parent
		/// </summary>
		/// <returns>The node's name or null if it doesn't have one</returns>
		private string getNodeName ()
		{
			if(HasParent)
			{
				var parentNode = ParentNode;
				switch(parentNode.Type)
				{
				case NodeType.List: // Looking for an index
					var listNode = (ListNode)parentNode;
					var index    = listNode.IndexOf(_node);
					if(index >= 0) // Found it
						return index.ToString(CultureInfo.InvariantCulture);
					break;

				case NodeType.Complex: // Looking for a node in Values that equals ours
					var complexNode = (ComplexNode)parentNode;
					foreach(var entry in complexNode)
						if(entry.Value == _node)
							return entry.Key; // Found our node
					break;
				default:
					throw new InvalidCastException("Unexpected parent node type");
				}
			}
			return null;
		}

		/// <summary>
		/// Constructs the path to the node (including its name) by following the trail of parents
		/// </summary>
		/// <returns>The node's path or null if the node is the root node</returns>
		private string getNodePath ()
		{
			if(IsRootNode)
				return null;
			var parentPath = _parent.getNodePath();
			var name = getNodeName();
			if(name == null)
				return null;
			var path = (parentPath == null) ? name : String.Join("/", parentPath, name);
			return path;
		}
	}
}
