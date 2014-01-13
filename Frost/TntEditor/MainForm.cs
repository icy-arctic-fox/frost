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
			constructNewNodeMenu();

			treeView.ImageList = _nodeTypeImageList;
			displaySampleContainer();
		}
		
		private void constructNewNodeMenu ()
		{
			newNodeContextMenuStrip.ImageList = _nodeTypeImageList;
			for(var type = NodeType.Boolean; type <= NodeType.Complex; ++type)
			{
				var index  = (int)type;
				var button = new ToolStripButton {
					Tag        = type,
					Text       = type.ToString(),
					ImageIndex = index
				};
				newNodeContextMenuStrip.Items.Add(button);
			}
		}

		private void displaySampleContainer ()
		{
			var container = constructSampleContainer();
			DisplayContainer(container);
		}

		private static NodeContainer constructSampleContainer ()
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
				var list = new ListNode(NodeType.Guid) {
					new GuidNode(Guid.NewGuid()),
					new GuidNode(Guid.NewGuid()),
					new GuidNode(Guid.NewGuid())
				};
				complex.Add("IDs", list);
				root.Add(complex);
			}
			return new NodeContainer(root);
		}

		#region Node type image list

		/// <summary>
		/// Constructs the node type image list
		/// </summary>
		static MainForm ()
		{
			constructImageList();
		}

		private static readonly ImageList _nodeTypeImageList = new ImageList();

		/// <summary>
		/// Collection of images for each node type
		/// </summary>
		public static ImageList NodeTypeImageList
		{
			get { return _nodeTypeImageList; }
		}

		private static void constructImageList ()
		{
			_nodeTypeImageList.ImageSize  = new Size(16, 16);
			_nodeTypeImageList.ColorDepth = ColorDepth.Depth32Bit;
			for(var type = NodeType.End; type <= NodeType.Complex; ++type)
			{
				var filename = "Images/type" + (int)type + ".png";
				var typeName = type.ToString();
				var image    = Image.FromFile(filename);
				_nodeTypeImageList.Images.Add(typeName, image);
			}
		}
		#endregion

		/// <summary>
		/// Displays a node container in the tree pane
		/// </summary>
		/// <param name="container">Container to display</param>
		public void DisplayContainer (NodeContainer container)
		{
			if(container == null)
				throw new ArgumentNullException("container", "The node container to display can't be null.");

			var treeRoot = constructTreeNode(container);
			treeView.Nodes.Clear();
			treeView.Nodes.Add(treeRoot);
		}

		/// <summary>
		/// Refreshes the numerical indices on the nodes.
		/// This should be called after adding or deleting a node in a list node
		/// </summary>
		/// <param name="treeNode">Node that refers to the list node</param>
		private void refreshListNumbers (TreeNode treeNode)
		{
			var list = treeNode.Tag as ListNode;
			if(list != null)
			{
				var value   = list.StringValue;
				var parent  = treeNode.Parent;
				string name = null;
				if(parent != null)
					name = NodeInfo.GetNodeName(parent.Tag as Node, list);
				var text = (name == null) ? value : String.Format("{0}: {1}", name, value);
				treeNode.Text = text;

				var i = 0;
				foreach(TreeNode child in treeNode.Nodes)
					child.Text = String.Format("[{0}]: {1}", i++, ((Node)child.Tag).StringValue);
			}
		}

		#region Search

		/// <summary>
		/// Finds the next node that matches some text
		/// </summary>
		/// <param name="text">Text to look for</param>
		/// <returns>A tree node of the found node, or null if nothing was found</returns>
		private TreeNode findNextNode (string text)
		{
			var start = treeView.SelectedNode ?? treeView.Nodes[0];
			return searchBranch(start, text);
		}

		/// <summary>
		/// Searches a branch for text
		/// </summary>
		/// <param name="treeNode">Starting point of the branch</param>
		/// <param name="text">Text to look for</param>
		/// <returns>A node that contains the text or null if the text wasn't found</returns>
		private static TreeNode searchBranch (TreeNode treeNode, string text)
		{
			TreeNode result;
			var curNode = treeNode;
			while(curNode != null)
			{
				if((result = nodeMatches(curNode, text)) != null)
					return result;
				curNode = curNode.NextNode;
			}
			foreach(TreeNode child in treeNode.Nodes)
				if((result = searchBranch(child, text)) != null)
					return result;
			return null;
		}

		/// <summary>
		/// Checks if the current node matches the text
		/// </summary>
		/// <param name="treeNode">Node to check</param>
		/// <param name="text">Text to look for</param>
		/// <returns>The matching node or null if the text wasn't found</returns>
		private static TreeNode nodeMatches (TreeNode treeNode, string text)
		{
			var node = treeNode.Tag as Node;
			if(node != null)
			{
				switch(node.Type)
				{
				case NodeType.Complex:
					var complex = (ComplexNode)node;
					foreach(var entry in complex)
					{
						var name = entry.Key;
						if(name.Contains(text))
							foreach(TreeNode child in treeNode.Nodes)
								if(child.Tag == entry.Value)
									return child;
					}
					break;
				default:
					if(node.StringValue.Contains(text))
						return treeNode;
					break;
				}
			}
			return null;
		}
		#endregion

		#region Tree view construction

		/// <summary>
		/// Creates the top-level container GUI tree node
		/// </summary>
		/// <param name="container">Node container to pull information from</param>
		/// <returns>A GUI tree node</returns>
		private TreeNode constructTreeNode (NodeContainer container)
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
		private TreeNode constructTreeNode (Node node, string name = null)
		{
			if(node == null)
				throw new ArgumentNullException("node", "The node to create a tree node from can't be null.");

			// Construct the node itself
			var type  = node.Type;
			var index = (int)type;
			var value = node.StringValue;
			var text  = (name == null) ? value : String.Format("{0}: {1}", name, value);

			var treeNode = new TreeNode(text, index, index) {
				ContextMenuStrip = nodeContextMenuStrip,
				ToolTipText      = type.ToString(),
				Tag              = node
			};

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

			return treeNode;
		}

		/// <summary>
		/// Appends elements of a list node to a GUI tree node
		/// </summary>
		/// <param name="baseNode">Tree node to append the list items to</param>
		/// <param name="list">TNT list node to pull information from</param>
		private void constructListTree (TreeNode baseNode, IEnumerable<Node> list)
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
		private void constructComplexTree (TreeNode baseNode, IEnumerable<KeyValuePair<string, Node>> complex)
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

		private void enableListNodeOptions(bool flag = true)
		{
			deleteToolStripButton.Enabled   = flag;
			deleteToolStripMenuItem.Enabled = flag;
			enableNodeOptions(flag);
		}

		private void enableNodeOptions (bool flag = true)
		{
			copyToolStripButton.Enabled    = flag;
			copyToolStripMenuItem.Enabled  = flag;
			pasteToolStripButton.Enabled   = flag;
			pasteToolStripMenuItem.Enabled = flag;
		}

		#region Event listeners
		private void treeView_NodeMouseClick (object sender, TreeNodeMouseClickEventArgs e)
		{
			var node = e.Node.Tag as Node;
			if(node != null)
			{
				nodeInfoPanel.SetDisplayNode(e.Node);
				if(e.Node.Parent != null && e.Node.Parent.Tag is Node)
					enableListNodeOptions();
				else
					enableNodeOptions();
			}
			else
				enableListNodeOptions(false);
		}

		private void addNodeToolStripButton_Click (object sender, EventArgs e)
		{
			((ToolStripSplitButton)sender).ShowDropDown();
		}

		private void searchToolStripTextBox_Click (object sender, EventArgs e)
		{
			var textBox = (ToolStripTextBox)sender;
			if(textBox.ForeColor == SystemColors.GrayText)
			{// Search text
				textBox.Text      = String.Empty;
				textBox.ForeColor = SystemColors.ControlText;
			}
		}

		private void searchToolStripTextBox_Leave (object sender, EventArgs e)
		{
			var textBox = (ToolStripTextBox)sender;
			if(String.IsNullOrWhiteSpace(textBox.Text))
			{// Display search text
				textBox.ForeColor = SystemColors.GrayText;
				textBox.Text      = "Search";
			}
		}

		private void searchToolStripButton_Click(object sender, EventArgs e)
		{
			if(searchToolStripTextBox.ForeColor != SystemColors.GrayText)
			{
				var text  = searchToolStripTextBox.Text;
				var found = findNextNode(text);
				if(found != null)
				{
					treeView.SelectedNode = found;
					nodeInfoPanel.SetDisplayNode(found);
				}
				else
					System.Media.SystemSounds.Beep.Play();
			}
		}

		private void searchToolStripTextBox_KeyDown (object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				e.SuppressKeyPress = true;
				searchToolStripButton_Click(sender, e);
			}
		}

		private void deleteButton_Click (object sender, EventArgs e)
		{
			var treeNode = treeView.SelectedNode;
			if(treeNode != null)
			{
				var node = treeNode.Tag as Node;
				if(node != null)
				{
					var parent = treeNode.Parent;
					if(parent != null)
					{
						var parentNode = parent.Tag as Node;
						if(parentNode != null)
						{
							switch(parentNode.Type)
							{
							case NodeType.List:
								((ListNode)parentNode).Remove(node);
								break;
							case NodeType.Complex:
								((ComplexNode)parentNode).Remove(node);
								break;
							default:
								throw new ApplicationException("Unexpected parent node type");
							}
							treeNode.Remove();
							if(parentNode.Type == NodeType.List)
								refreshListNumbers(parent);
							nodeInfoPanel.SetDisplayNode(treeView.SelectedNode);
						}
					}
				}
			}
		}
		#endregion
	}
}
