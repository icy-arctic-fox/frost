using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor
{
	public partial class NodeInfoPanel : UserControl
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
		/// <param name="treeNode">GUI node to display information for</param>
		public void SetDisplayNode(TreeNode treeNode)
		{
			var node = treeNode.Tag as Node;
			if(node != null)
			{
				updatePath(treeNode);
				updateControls(node);
			}
			// TODO: else - set controls to blank
		}

		/// <summary>
		/// Updates the contents of the displayed controls
		/// </summary>
		private void updateControls (Node node)
		{
			var type  = node.Type;
			var index = (int)type;
			typeCombo.SelectedIndex = index;
			valueBox.Text = node.StringValue;
		}

		#region Node path
		/// <summary>
		/// Figures out what the path and name of a node is
		/// </summary>
		private void updatePath (TreeNode treeNode)
		{
			nameText.Text = (treeNode.Parent == null) ? String.Empty : GetNodeName(treeNode.Parent.Tag as Node, treeNode.Tag as Node);
			pathText.Text = getNodePath(treeNode);
		}

		/// <summary>
		/// Gets the name or index of a node
		/// </summary>
		/// <param name="parent">Parent node (if any)</param>
		/// <param name="node">Node to look for</param>
		/// <returns>The node's name or index, or null if it doesn't have either</returns>
		public static string GetNodeName (Node parent, Node node)
		{
			if(parent != null)
			{
				string path;
				switch(parent.Type)
				{
				case NodeType.List:
					path = ((ListNode)parent).IndexOf(node).ToString(CultureInfo.InvariantCulture);
					break;
				case NodeType.Complex:
					var complex = (ComplexNode)parent;
					path = complex.Where(entry => entry.Value == node).Select(entry => entry.Key).First();
					break;
				default: // Shouldn't get here
					path = null;
					break;
				}
				return path;
			}
			return null;
		}

		/// <summary>
		/// Gets the 
		/// </summary>
		/// <param name="treeNode">GUI tree node that has the node to generate the path for</param>
		/// <returns>The node's path, or null if it's a top-level node</returns>
		private static string getNodePath (TreeNode treeNode)
		{
			var node = treeNode.Tag as Node;
			if(node != null && treeNode.Parent != null)
			{
				var parent = treeNode.Parent;
				var name   = GetNodeName(parent.Tag as Node, node);
				var path   = getNodePath(parent);
				return (path == null) ? name : String.Join("/", path, name);
			}
			return null;
		}
		#endregion

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
				img = MainForm.NodeTypeImageList.Images[index];
				applyButton.Enabled = true;
			}
			typePicture.Image = img;
		}
	}
}
