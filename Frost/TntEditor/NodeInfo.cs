using System;
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// Constructs the path to the node (including its name) by following the trail of parents
		/// </summary>
		/// <returns>The node's path or null if the node is the root node</returns>
		private string getNodePath ()
		{
			throw new NotImplementedException();
		}
	}
}
