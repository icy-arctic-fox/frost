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
	public partial class NodeInfo : UserControl
	{
		public NodeInfo ()
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
			typePicture.Image = MainForm.NodeTypeImageList.Images[(int)node.Type];
			valueBox.Text = node.StringValue;
		}

		/// <summary>
		/// Figures out what the path to the node is
		/// </summary>
		private void updatePath (TreeNode treeNode)
		{
			nameText.Text = (treeNode.Parent == null) ? String.Empty : getBaseName(treeNode.Parent.Tag as Node, treeNode.Tag as Node);
			pathText.Text = getBasePath(treeNode);
		}

		private static string getBaseName (Node parent, Node node)
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

		private static string getBasePath (TreeNode treeNode)
		{
			var node = treeNode.Tag as Node;
			if(node != null && treeNode.Parent != null)
			{
				var parent = treeNode.Parent;
				var name   = getBaseName(parent.Tag as Node, node);
				var path   = getBasePath(parent);
				return (path == null) ? name : String.Join("/", path, name);
			}
			return null;
		}
	}
}
