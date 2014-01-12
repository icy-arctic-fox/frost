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
		}

		/// <summary>
		/// Sets the node that has information displayed about it
		/// </summary>
		/// <param name="treeNode">GUI node to display information for</param>
		public void SetDisplayNode (TreeNode treeNode)
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
			pathText.Text = getBasePath(treeNode);
		}

		private static string getBasePath (TreeNode treeNode)
		{
			var node = treeNode.Tag as Node;
			if(node != null && treeNode.Parent != null)
			{
				var parent = treeNode.Parent;
				var parentNode = parent.Tag as Node;
				if(parentNode != null)
				{
					string path;
					switch(parentNode.Type)
					{
					case NodeType.List:
						path = ((ListNode)parentNode).IndexOf(node).ToString(CultureInfo.InvariantCulture);
						break;
					case NodeType.Complex:
						var complex = (ComplexNode)parentNode;
						path = complex.Where(entry => entry.Value == node).Select(entry => entry.Key).First();
						break;
					default: // Shouldn't get here
						path = null;
						break;
					}
					var parentPath = getBasePath(parent);
					return (parentPath == null) ? path : String.Join("/", parentPath, path);
				}
			}
			return null;
		}
	}
}
