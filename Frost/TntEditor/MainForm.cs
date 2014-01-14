﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor
{
	public partial class MainForm : Form
	{
		private const string DefaultTitle = "TNT Editor";

		private NodeContainer _activeContainer;
		private string _activeFilepath;
		private bool _activeContainerCompressed;

		public MainForm ()
		{
			InitializeComponent();
			constructNewNodeMenu();

			displaySampleContainer();
		}
		
		private void constructNewNodeMenu ()
		{
			newNodeContextMenuStrip.ImageList = _nodeTypeImageList;
			for(var type = NodeType.Boolean; type <= NodeType.Complex; ++type)
			{
				var index = (int)type;
				var item  = new ToolStripButton();
				item.Tag        = type;
				item.Text       = type.ToString();
				item.ImageIndex = index;
				newNodeContextMenuStrip.Items.Add(item);
			}
		}

		private void displaySampleContainer ()
		{
			var container = constructSampleContainer();
			_activeContainer = container;
			DisplayContainer(container);
		}

		private static NodeContainer constructSampleContainer ()
		{
			var root = new ListNode(NodeType.Complex);
			for(var i = 0; i < 20; ++i)
			{
				var complex = new ComplexNode {
					{"foo",    new IntNode(5 * i)},
					{"bar",    new BlobNode(new byte[i])},
					{"sushi",  new ColorNode(5 * i)},
					{"wasabi", new XyNode(2 * i, 7 * i)}
				};
				var list = new ListNode(NodeType.Guid) {
					new GuidNode(Guid.NewGuid()),
					new GuidNode(Guid.NewGuid()),
					new GuidNode(Guid.NewGuid())
				};
				complex.Add("IDs", list);
				root.Add(complex);
			}
			var all = new ComplexNode();
			for(var type = NodeType.Boolean; type <= NodeType.Complex; ++type)
				all.Add(type.ToString(), Node.CreateDefaultNode(type));
			root.Add(all);
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
			nodeEditorPanel.NodeContainer = container;
		}

		/// <summary>
		/// Refreshes the numerical indices on the nodes.
		/// This should be called after adding or deleting a node in a list node
		/// </summary>
		/// <param name="treeNode">Node that refers to the list node</param>
		private void refreshListNumbers (TreeNode treeNode)
		{
			var info = treeNode.Tag as NodeInfo;
			if(info != null)
			{
				var list = info.Node as ListNode;
				if(list != null)
				{
					var value      = list.StringValue;
					var parentInfo = info.Parent;
					string name    = null;
					if(parentInfo != null)
						name = parentInfo.Name;
					var text = (name == null) ? value : String.Format("{0}: {1}", name, value);
					treeNode.Text = text;

					var i = 0;
					foreach(TreeNode child in treeNode.Nodes)
						child.Text = String.Format("[{0}]: {1}", i++, ((Node)child.Tag).StringValue);
				}
			}
		}

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

		private void saveActiveContainer (string filepath, bool compress)
		{
			using(var fs = new FileStream(filepath, FileMode.Create))
			{
				if(compress)
					using(var ds = new Ionic.Zlib.DeflateStream(fs, Ionic.Zlib.CompressionMode.Compress))
						_activeContainer.WriteToStream(ds);
				else
					_activeContainer.WriteToStream(fs);
			}
			_activeContainerCompressed = compress;
		}

		private void loadContainer (string filepath, bool compress)
		{
			using(var fs = new FileStream(filepath, FileMode.Open))
			{
				if(compress)
					using(var ds = new Ionic.Zlib.DeflateStream(fs, Ionic.Zlib.CompressionMode.Decompress))
						_activeContainer = NodeContainer.ReadFromStream(ds);
				else
					_activeContainer = NodeContainer.ReadFromStream(fs);
			}
			_activeContainerCompressed = compress;
		}

		#region Event listeners
		private void treeView_AfterSelect (object sender, TreeViewEventArgs e)
		{
			var info = e.Node.Tag as NodeInfo;
			if(info != null)
			{
				var node = info.Node;
				if(node != null)
				{
					nodeInfoPanel.SetDisplayNode(info);
					if(e.Node.Parent != null && e.Node.Parent.Tag is NodeInfo)
						enableListNodeOptions();
					else
						enableNodeOptions();
					switch(node.Type)
					{
					case NodeType.List:
						var list = (ListNode)node;
						var elementType = list.ElementType;
						addNodeToolStripButton.Image = _nodeTypeImageList.Images[(int)elementType];
						addNodeListToolStripButton.Visible = false;
						addNodeToolStripButton.Enabled = true;
						addNodeToolStripButton.Visible = true;
						break;
					case NodeType.Complex:
						addNodeListToolStripButton.Enabled = true;
						addNodeListToolStripButton.Visible = true;
						addNodeToolStripButton.Visible = false;
						break;
					default:
						addNodeToolStripButton.Image = addNodeListToolStripButton.Image;
						addNodeListToolStripButton.Visible = false;
						addNodeToolStripButton.Enabled = false;
						addNodeToolStripButton.Visible = true;
						break;
					}
				}
				else
				{
					enableListNodeOptions(false);
					addNodeToolStripButton.Image = addNodeListToolStripButton.Image;
					addNodeListToolStripButton.Visible = false;
					addNodeToolStripButton.Enabled = false;
					addNodeToolStripButton.Visible = true;
				}
			}
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		#region File menu
		private void newToolStripMenuItem_Click (object sender, EventArgs e)
		{
			using(var newDialog = new NewContainerDialog())
				if(newDialog.ShowDialog() == DialogResult.OK)
				{
					var type = newDialog.RootNodeType;
					var root = Node.CreateDefaultNode(type);
					_activeContainer = new NodeContainer(root);
					Text = DefaultTitle;
					DisplayContainer(_activeContainer);
				}
		}

		private void openToolStripMenuItem_Click (object sender, EventArgs args)
		{
			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				var filename   = openFileDialog.SafeFileName;
				var filepath   = openFileDialog.FileName;
				var compressed = openFileDialog.FilterIndex == 2;

				try
				{
					loadContainer(filepath, compressed);
					Text = String.Join(" - ", DefaultTitle, filename);
					DisplayContainer(_activeContainer);
				}
				catch(Exception e)
				{
					MessageBox.Show("Failed to load TNT file:" + Environment.NewLine + e.Message, "Load failed", MessageBoxButtons.OK,
									MessageBoxIcon.Error);
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (null == _activeFilepath)
				saveAstoolStripMenuItem_Click(sender, e);
			else
				saveActiveContainer(_activeFilepath, _activeContainerCompressed);
		}

		private void saveAstoolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				var filepath   = saveFileDialog.FileName;
				var filename   = Path.GetFileName(filepath);
				var compressed = saveFileDialog.FilterIndex == 2;

				Text = String.Join(" - ", DefaultTitle, filename);
				saveActiveContainer(filepath, compressed);
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
		#endregion
		#endregion
	}
}
