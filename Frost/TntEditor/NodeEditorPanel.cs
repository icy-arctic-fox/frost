using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor
{
	public partial class NodeEditorPanel : UserControl
	{
		private NodeContainer _container;

		/// <summary>
		/// Container to display the nodes of in the tree view
		/// </summary>
		public NodeContainer NodeContainer
		{
			get { return _container; }
			set
			{
				_container = value;
				if(_container != null)
					reconstructTreeView();
				else
					treeView.Nodes.Clear();
			}
		}

		public NodeEditorPanel ()
		{
			InitializeComponent();
		}

		#region Tree construction

		/// <summary>
		/// Reconstructs the graphical node tree
		/// </summary>
		private void reconstructTreeView ()
		{
			treeView.Nodes.Clear();
			var rootTreeNode = constructContainerTreeNode(_container);

			treeView.Nodes.Add(rootTreeNode);
		}

		/// <summary>
		/// Constructs the top-level container tree node
		/// </summary>
		/// <param name="container">Container to construct a tree node for</param>
		/// <returns>A graphical tree node</returns>
		private static TreeNode constructContainerTreeNode (NodeContainer container)
		{
			var text = String.Format("Node container (version {0})", container.Version);
			const int index = (int)NodeType.End;

			return new TreeNode {
				Text       = text,
				ImageIndex = index,
				Tag        = container
			};
		}

		/// <summary>
		/// Constructs a tree node for a node and its children
		/// </summary>
		/// <param name="node">Node to construct a tree node for</param>
		/// <param name="parent">Information about the parent node</param>
		/// <param name="name">Name to give the node (prefix in the text label)</param>
		/// <returns>A graphical tree node</returns>
		private static TreeNode constructTreeNode (Node node, NodeInfo parent = null, string name = null)
		{
			var info  = new NodeInfo(node, parent);
			var value = node.StringValue;
			var text  = (name == null) ? value : String.Format("{0}: {1}", name, value);
			var type  = node.Type;
			var index = (int)type;

			var treeNode = new TreeNode {
				Text       = text,
				ImageIndex = index,
				Tag        = info
			};

			switch(type)
			{
			case NodeType.List:
				appendListTreeNode(treeNode, (ListNode)node, info);
				break;
			case NodeType.Complex:
				appendComplexTreeNode(treeNode, (ComplexNode)node, info);
				break;
			}

			return treeNode;
		}

		/// <summary>
		/// Appends child nodes of a list node to a tree node
		/// </summary>
		/// <param name="treeNode">Tree node to append the children to</param>
		/// <param name="nodeList">Collection of child nodes to append</param>
		/// <param name="info">Information about the parent (list) node</param>
		private static void appendListTreeNode (TreeNode treeNode, IEnumerable<Node> nodeList, NodeInfo info)
		{
			var i = 0;
			foreach(var childNode in nodeList)
			{
				var name = String.Format("[{0}]", i++);
				var childTreeNode = constructTreeNode(childNode, info, name);
				treeNode.Nodes.Add(childTreeNode);
			}
		}

		/// <summary>
		/// Appends child nodes of a complex node to a tree node
		/// </summary>
		/// <param name="treeNode">Tree node to append the children to</param>
		/// <param name="nodeList">Collection of child nodes to append</param>
		/// <param name="info">Information about the parent (complex) node</param>
		private static void appendComplexTreeNode (TreeNode treeNode, IEnumerable<KeyValuePair<string, Node>> nodeList, NodeInfo info)
		{
			foreach(var entry in nodeList)
			{
				var name      = entry.Key;
				var childNode = entry.Value;
				var childTreeNode = constructTreeNode(childNode, info, name);
				treeNode.Nodes.Add(childTreeNode);
			}
		}
		#endregion

		/// <summary>
		/// Information about the currently selected node
		/// </summary>
		/// <remarks>This can be null if the container node or no node is selected.</remarks>
		internal NodeInfo SelectedNode
		{
			get
			{
				var selected = treeView.SelectedNode;
				if(selected != null)
					return selected.Tag as NodeInfo;
				return null;
			}
		}

		/// <summary>
		/// Checks if the currently selected node can be moved up
		/// </summary>
		/// <remarks>A node can be moved up if it's in a list and not already at the top of the list.</remarks>
		public bool CanMoveSelectedNodeUp
		{
			get
			{
				var selected = treeView.SelectedNode;
				if(selected != null)
				{
					var info = selected.Tag as NodeInfo;
					if(info != null)
					{
						var node = info.Node;
						var parentNode = info.ParentNode;
						if(parentNode != null && parentNode.Type == NodeType.List)
							return ((ListNode)parentNode).IndexOf(node) > 0;
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Checks if the currently selected node can be moved down
		/// </summary>
		/// <remarks>A node can be moved down if it's in a list and not already at the bottom of the list.</remarks>
		public bool CanMoveSelectedNodeDown
		{
			get
			{
				var selected = treeView.SelectedNode;
				if(selected != null)
				{
					var info = selected.Tag as NodeInfo;
					if(info != null)
					{
						var node = info.Node;
						var parentNode = info.ParentNode;
						if(parentNode != null && parentNode.Type == NodeType.List)
						{
							var listNode = (ListNode)parentNode;
							return listNode.IndexOf(node) < listNode.Count - 1;
						}
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Checks if the currently selected node can be deleted
		/// </summary>
		/// <remarks>The node container and root node can't be deleted.</remarks>
		public bool CanDeleteSelectedNode
		{
			get
			{
				var selected = treeView.SelectedNode;
				if(selected != null)
				{
					var info = selected.Tag as NodeInfo;
					if(info != null)
						return !info.IsRootNode;
				}
				return false;
			}
		}

		#region Tree manipulation

		/// <summary>
		/// Moves the currently selected node up one slot
		/// </summary>
		public void MoveSelectedNodeUp ()
		{
			if(CanMoveSelectedNodeUp)
			{
				var info = SelectedNode;
				var node = info.Node;
				var parentNode = info.ParentNode;
				var listNode   = (ListNode)parentNode;

				// Reinsert at the previous index
				var index = listNode.IndexOf(node);
				listNode.RemoveAt(index);
				listNode.Insert(index - 1, node);

				// Move the visual node up
				var selected = treeView.SelectedNode;
				var parent   = selected.Parent;
				index = selected.Index;
				parent.Nodes.RemoveAt(index);
				parent.Nodes.Insert(index - 1, selected);
			}
		}

		/// <summary>
		/// Moves the currently selected node down one slot
		/// </summary>
		public void MoveSelectedNodeDown ()
		{
			if(CanMoveSelectedNodeDown)
			{
				var info = SelectedNode;
				var node = info.Node;
				var parentNode = info.ParentNode;
				var listNode   = (ListNode)parentNode;

				// Reinsert at the next index
				var index = listNode.IndexOf(node);
				listNode.RemoveAt(index);
				listNode.Insert(index + 1, node);

				// Move the visual node down
				var selected = treeView.SelectedNode;
				var parent   = selected.Parent;
				index = selected.Index;
				parent.Nodes.RemoveAt(index);
				parent.Nodes.Insert(index + 1, selected);
			}
		}

		/// <summary>
		/// Deletes the currently selected node
		/// </summary>
		public void DeleteSelectedNode ()
		{
			if(CanDeleteSelectedNode)
			{
				var info = SelectedNode;
				var node = info.Node;
				var parentNode = info.ParentNode;

				// Remove the node from the structure
				switch(parentNode.Type)
				{
				case NodeType.List:
					((ListNode)parentNode).Remove(node);
					break;
				case NodeType.Complex:
					((ComplexNode)parentNode).Remove(node);
					break;
				default:
					throw new InvalidCastException("Unexpected parent node type");
				}

				// Remove the visual node
				treeView.SelectedNode.Remove();
			}
		}
		#endregion
	}
}
