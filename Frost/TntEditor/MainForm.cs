using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor
{
	public partial class MainForm : Form
	{
		public MainForm ()
		{
			InitializeComponent();
			constructImageList();

			treeView.ImageList = _nodeTypeImageList;
			displaySampleContainer();
		}

		private void displaySampleContainer ()
		{
			var root = new ListNode(NodeType.Complex);
			for (var i = 0; i < 20; ++i)
			{
				var complex = new ComplexNode {
					{"foo "    + i, new IntNode(5 * i)},
					{"bar "    + i, new BlobNode(new byte[i])},
					{"sushi "  + i, new ColorNode(5 * i)},
					{"wasabi " + i, new XyNode(2 * i, 7 * i)}
				};
				root.Add(complex);
			}
			var container = new NodeContainer(root);
			DisplayContainer(container);
		}

		private readonly ImageList _nodeTypeImageList = new ImageList();

		private void constructImageList ()
		{
			_nodeTypeImageList.ImageSize = new Size(16, 16);
			for(var type = NodeType.End; type <= NodeType.Complex; ++type)
			{
				var filename = "Images/type" + (int)type + ".png";
				var typeName = type.ToString();
				var image    = Image.FromFile(filename);
				_nodeTypeImageList.Images.Add(typeName, image);
			}
		}

		/// <summary>
		/// Displays a node container in the tree pane
		/// </summary>
		/// <param name="container">Container to display</param>
		public void DisplayContainer(NodeContainer container)
		{
			if(container == null)
				throw new ArgumentNullException("container", "The node container to display can't be null.");

			var treeRoot = constructTreeNode(container);
			treeView.Nodes.Clear();
			treeView.Nodes.Add(treeRoot);
		}

		#region Tree view construction

		/// <summary>
		/// Creates the top-level container GUI tree node
		/// </summary>
		/// <param name="container">Node container to pull information from</param>
		/// <returns>A GUI tree node</returns>
		private static TreeNode constructTreeNode (NodeContainer container)
		{
			if(container == null)
				throw new ArgumentNullException("container", "The node container to construct a tree from can't be null.");
			
			var text     = String.Format("Node container (version {0})", container.Version);
			var treeNode = new TreeNode(text, 0, 0) {Tag = container};
			var rootNode = constructTreeNode(container.Root);
			treeNode.Nodes.Add(rootNode);
			return treeNode;
		}

		/// <summary>
		/// Creates a GUI tree node from a TNT node
		/// </summary>
		/// <param name="node">Node to pull information from</param>
		/// <param name="name">Optional name to give the node</param>
		/// <returns>A GUI tree node</returns>
		private static TreeNode constructTreeNode (Node node, string name = null)
		{
			if(node == null)
				throw new ArgumentNullException("node", "The node to create a tree node from can't be null.");

			// Construct the node itself
			var type  = node.Type;
			var index = (int)type;
			var value = node.StringValue;
			var text  = (name == null) ? String.Format("({0}): {1}", type, value)
				: String.Format("{0} ({1}): {2}", name, type, value);
			var treeNode = new TreeNode(text, index, index);

			// Handle any children
			switch(type)
			{
			case NodeType.List:
				constructListTree(treeNode, (ListNode)node);
				break;
			case NodeType.Complex:
				constructComplexTree(treeNode, (ComplexNode)node);
				break;
			}

			treeNode.Tag = node;
			return treeNode;
		}

		/// <summary>
		/// Appends elements of a list node to a GUI tree node
		/// </summary>
		/// <param name="baseNode">Tree node to append the list items to</param>
		/// <param name="list">TNT list node to pull information from</param>
		private static void constructListTree (TreeNode baseNode, IEnumerable<Node> list)
		{
			var i = 0;
			foreach(var node in list)
			{
				var name     = String.Format("[{0}]", i++);
				var treeNode = constructTreeNode(node, name);
				baseNode.Nodes.Add(treeNode);
			}
		}

		/// <summary>
		/// Appends elements of a complex node to a GUI tree node
		/// </summary>
		/// <param name="baseNode">Tree node to append the list items to</param>
		/// <param name="complex">TNT complex node to pull information from</param>
		private static void constructComplexTree (TreeNode baseNode, IEnumerable<KeyValuePair<string, Node>> complex)
		{
			foreach(var entry in complex)
			{
				var name     = entry.Key;
				var node     = entry.Value;
				var treeNode = constructTreeNode(node, name);
				baseNode.Nodes.Add(treeNode);
			}
		}
		#endregion
	}
}
