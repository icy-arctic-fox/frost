using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor
{
	public partial class MainForm : Form
	{
		private const string DefaultTitle = "TNT Editor";

		private string _activeFilepath;
		private bool _activeContainerCompressed;

		public MainForm ()
		{
			InitializeComponent();
			constructNewNodeMenu();
			nodeEditorPanel.ContextMenuStrip = nodeContextMenuStrip;
			nodeEditorPanel.SelectedNodeChanged += nodeEditorPanel_SelectedNodeChanged;
			displaySampleContainer();
		}
		
		private void constructNewNodeMenu ()
		{
			newNodeContextMenuStrip.ImageList    = nodeTypeImageList;
			newNodeSubContextMenuStrip.ImageList = nodeTypeImageList;

			for(var type = NodeType.Boolean; type <= NodeType.Complex; ++type)
			{
				var index = (int)type;
				var item  = new ToolStripMenuItem {
					Tag        = type,
					Text       = type.ToString(),
					ImageIndex = index
				};
				item.Click += addNodeToComplexNode_Click;
				newNodeContextMenuStrip.Items.Add(item);

				item = new ToolStripMenuItem {
					Tag        = type,
					Text       = type.ToString(),
					ImageIndex = index
				};
				item.Click += addListNodeToComplexNode_Click;
				newNodeSubContextMenuStrip.Items.Add(item);
			}

			// Add sub-menu under the List node type
			((ToolStripMenuItem)newNodeContextMenuStrip.Items[(int)(NodeType.List) - 1]).DropDown = newNodeSubContextMenuStrip;
		}

		private void displaySampleContainer ()
		{
			var container = constructSampleContainer();
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

		/// <summary>
		/// Displays a node container in the tree pane
		/// </summary>
		/// <param name="container">Container to display</param>
		public void DisplayContainer (NodeContainer container)
		{
			nodeEditorPanel.NodeContainer = container;
		}

		private void saveActiveContainer (string filepath, bool compress)
		{
			saveFileDialog.FileName = filepath;
			openFileDialog.FileName = filepath;
			using(var fs = new FileStream(filepath, FileMode.Create))
			{
				if(compress)
					using(var ds = new Ionic.Zlib.DeflateStream(fs, Ionic.Zlib.CompressionMode.Compress))
						nodeEditorPanel.NodeContainer.WriteToStream(ds);
				else
					nodeEditorPanel.NodeContainer.WriteToStream(fs);
			}
			_activeFilepath = filepath;
			_activeContainerCompressed = compress;
		}

		private void loadContainer (string filepath, bool compress)
		{
			saveFileDialog.FileName = filepath;
			openFileDialog.FileName = filepath;
			using(var fs = new FileStream(filepath, FileMode.Open))
			{
				NodeContainer container;
				if(compress)
					using(var ds = new Ionic.Zlib.DeflateStream(fs, Ionic.Zlib.CompressionMode.Decompress))
						container = NodeContainer.ReadFromStream(ds);
				else
					container = NodeContainer.ReadFromStream(fs);
				nodeEditorPanel.NodeContainer = container;
			}
			_activeFilepath = filepath;
			_activeContainerCompressed = compress;
		}

		#region Event listeners

		void nodeEditorPanel_SelectedNodeChanged (object sender, TreeViewEventArgs e)
		{
			nodeInfoPanel.SetDisplayNode(nodeEditorPanel.SelectedNode);

			var canDelete = nodeEditorPanel.CanDeleteSelectedNode;
			deleteToolStripButton.Enabled   = canDelete;
			deleteToolStripMenuItem.Enabled = canDelete;

			var canMoveUp = nodeEditorPanel.CanMoveSelectedNodeUp;
			moveUpToolStripButton.Enabled   = canMoveUp;
			moveUpToolStripMenuItem.Enabled = canMoveUp;

			var canMoveDown = nodeEditorPanel.CanMoveSelectedNodeDown;
			moveDownToolStripButton.Enabled   = canMoveDown;
			moveDownToolStripMenuItem.Enabled = canMoveDown;

			var canAddToComplex = nodeEditorPanel.CanAddToComplexNode;
			if(canAddToComplex)
			{
				addNodeMultiToolStripButton.Visible   = true;
				addNodeMultiToolStripButton.Enabled   = true;
				addNodeMultiToolStripMenuItem.Visible = true;
				addNodeMultiToolStripMenuItem.Enabled = true;
				addNodeToolStripButton.Visible        = false;
				addNodeToolStripButton.Enabled        = false;
				addNodeToolStripMenuItem.Visible      = false;
				addNodeToolStripMenuItem.Enabled      = false;
			}
			else
			{
				var canAddToList = nodeEditorPanel.CanAddToListNode;
				var img = canAddToList ? nodeTypeImageList.Images[(int)nodeEditorPanel.SelectedListElementType] : addNodeMultiToolStripButton.Image;

				addNodeMultiToolStripButton.Visible   = false;
				addNodeMultiToolStripButton.Enabled   = false;
				addNodeMultiToolStripMenuItem.Visible = false;
				addNodeMultiToolStripMenuItem.Enabled = false;
				addNodeToolStripButton.Visible        = true;
				addNodeToolStripButton.Enabled        = canAddToList;
				addNodeToolStripButton.Image          = img;
				addNodeToolStripMenuItem.Visible      = true;
				addNodeToolStripMenuItem.Enabled      = canAddToList;
				addNodeToolStripMenuItem.Image        = img;
			}
		}

		private static string findUntakenName (ComplexNode complex)
		{
			var name = "New Node";
			if(complex.ContainsKey(name))
			{// Find a name that isn't taken
				var i = 2;
				for(; complex.ContainsKey(String.Format("{0} {1}", name, i)); ++i) { }
				name = String.Format("{0} {1}", name, i);
			}
			return name;
		}

		private void addNodeToComplexNode_Click (object sender, EventArgs e)
		{
			var item     = (ToolStripMenuItem)sender;
			var type     = (NodeType)item.Tag;
			var node     = Node.CreateDefaultNode(type);
			var selected = nodeEditorPanel.SelectedNode;
			var complex  = (ComplexNode)((selected.Node.Type == NodeType.Complex) ? selected.Node : selected.Parent.Node);
			var name     = findUntakenName(complex);
			nodeEditorPanel.AddToComplexNode(name, node);
		}

		private void addListNodeToComplexNode_Click (object sender, EventArgs e)
		{
			var item     = (ToolStripMenuItem)sender;
			var type     = (NodeType)item.Tag;
			var node     = new ListNode(type);
			var selected = nodeEditorPanel.SelectedNode;
			var complex  = (ComplexNode)((selected.Node.Type == NodeType.Complex) ? selected.Node : selected.Parent.Node);
			var name     = findUntakenName(complex);
			nodeEditorPanel.AddToComplexNode(name, node);
		}

		private void addNodeButton_Click (object sender, EventArgs e)
		{
			var type = nodeEditorPanel.SelectedListElementType;
			var node = Node.CreateDefaultNode(type);
			nodeEditorPanel.AddToListNode(node);
		}

		private void moveUpButton_Click (object sender, EventArgs e)
		{
			nodeEditorPanel.MoveSelectedNodeUp();
		}

		private void moveDownButton_Click (object sender, EventArgs e)
		{
			nodeEditorPanel.MoveSelectedNodeDown();
		}

		private void deleteButton_Click (object sender, EventArgs e)
		{
			nodeEditorPanel.DeleteSelectedNode();
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

		private void searchToolStripButton_Click (object sender, EventArgs e)
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

		#region File menu
		private void newToolStripMenuItem_Click (object sender, EventArgs e)
		{
			using(var newDialog = new NewContainerDialog())
				if(newDialog.ShowDialog() == DialogResult.OK)
				{
					var type = newDialog.RootNodeType;
					var root = Node.CreateDefaultNode(type);
					var container = new NodeContainer(root);
					Text = DefaultTitle;
					DisplayContainer(container);
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
			if(null == _activeFilepath)
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

		private void nodeInfoPanel_NodeModified (object sender, NodeInfoPanel.NodeUpdateEventArgs e)
		{
			nodeEditorPanel.UpdateSelectedNode(e.Name, e.Node);
		}
		#endregion
	}
}
