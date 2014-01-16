using System;
using System.Drawing;
using System.Windows.Forms;
using Frost.IO.Tnt;
using Frost.TntEditor.NodeValueControls;

namespace Frost.TntEditor
{
	internal partial class NodeInfoPanel : UserControl
	{
		public NodeInfoPanel ()
		{
			InitializeComponent();
			typeCombo.Items.Add(String.Empty);
			for(var type = NodeType.Boolean; type <= NodeType.Complex; ++type)
				typeCombo.Items.Add(type);
		}

		/// <summary>
		/// Sets the node that has information displayed about it
		/// </summary>
		/// <param name="info">Information about the node to display</param>
		internal void SetDisplayNode (NodeInfo info)
		{
			if(info != null)
				updateControls(info);
			else
				blankControls();
		}

		/// <summary>
		/// Updates the contents of the displayed controls
		/// </summary>
		private void updateControls (NodeInfo info)
		{
			nameText.Text = info.Name;
			pathText.Text = info.Path;
			var type      = info.Node.Type;
			var index     = (int)type;
			typeCombo.SelectedIndex = index;

			var parentNode    = info.ParentNode;
			nameText.ReadOnly = (parentNode == null || parentNode.Type != NodeType.Complex);
			typeCombo.Enabled = (parentNode == null || parentNode.Type != NodeType.List);

			var node = info.Node;
			if(_nodeEditorControl != null)
				_nodeEditorControl.FromNode(node);
		}

		/// <summary>
		/// Blanks out the contents of the displayed controls
		/// </summary>
		private void blankControls ()
		{
			nameText.Text = String.Empty;
			pathText.Text = String.Empty;
			typeCombo.SelectedIndex = 0;
			typeCombo.Enabled = false;

			nameText.ReadOnly = true;
		}

		private INodeEditorControl _nodeEditorControl;

		/// <summary>
		/// Changes the control used to display and edit the node's value
		/// </summary>
		/// <param name="type">Node type to display a control for</param>
		private void changeValueEditor (NodeType type)
		{
			if(_nodeEditorControl != null)
				nodeInfoLayoutPanel.Controls.Remove((Control)_nodeEditorControl);

			switch(type)
			{
			case NodeType.Boolean:
				_nodeEditorControl = new BooleanNodeValueControl();
				break;
			case NodeType.Byte:
			case NodeType.SByte:
			case NodeType.Short:
			case NodeType.UShort:
			case NodeType.Int:
			case NodeType.UInt:
			case NodeType.Long:
			case NodeType.ULong:
				_nodeEditorControl = new IntegerNodeValueControl();
				break;
			default:
				_nodeEditorControl = null;
				break;
			}

			if(_nodeEditorControl != null)
			{
				var control = (Control)_nodeEditorControl;
				nodeInfoLayoutPanel.Controls.Add(control, 0, 3);
				nodeInfoLayoutPanel.SetColumnSpan(control, 3);
			}
		}

		/// <summary>
		/// Triggered when a node has been modified
		/// </summary>
		public event EventHandler<NodeUpdateEventArgs> NodeModified;

		#region Event listeners
		private void typeCombo_SelectedIndexChanged (object sender, EventArgs e)
		{
			var combo = (ComboBox)sender;
			var index = combo.SelectedIndex;
			Image img;
			if(index == 0)
			{// No type
				img = null;
				applyButton.Enabled = false;
			}
			else
			{// Type specified
				img = nodeTypeImageList.Images[index];
				applyButton.Enabled = true;
			}
			typePicture.Image = img;
			changeValueEditor((NodeType)index);
		}

		private void applyButton_Click (object sender, EventArgs e)
		{
			if(NodeModified != null)
			{
				var name = nameText.Text;
				var node = _nodeEditorControl.AsNode();
				var args = new NodeUpdateEventArgs(name, node);
				NodeModified(this, args);
			}
		}
		#endregion

		/// <summary>
		/// Information about a node change
		/// </summary>
		internal class NodeUpdateEventArgs : EventArgs
		{
			private readonly string _name;

			/// <summary>
			/// New node name (for nodes nested under complex nodes)
			/// </summary>
			public string Name
			{
				get { return _name; }
			}

			private readonly Node _node;

			/// <summary>
			/// New node values
			/// </summary>
			public Node Node
			{
				get { return _node; }
			}

			public NodeUpdateEventArgs (string name, Node node)
			{
				_name = name;
				_node = node;
			}
		}
	}
}
