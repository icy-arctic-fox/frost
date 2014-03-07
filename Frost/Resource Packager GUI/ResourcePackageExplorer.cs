using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Frost.IO.Resources;

namespace Frost.ResourcePackagerGui
{
	public partial class ResourcePackageExplorer : UserControl
	{
		public ResourcePackageExplorer ()
		{
			InitializeComponent();
		}

		public ResourcePackageExplorer (ResourcePackage package)
			: this()
		{
			DisplayPackageContents(package);
		}

		public void DisplayPackageContents (ResourcePackage package)
		{
			var root = constructTree(package.Resources, treeView.PathSeparator[0]);
			treeView.Nodes.Add(root);
		}

		#region Tree construction

		private static TreeNode constructTree (IEnumerable<ResourcePackageEntry> resources, char separator)
		{
			var root = new TreeNode();
			foreach(var resource in resources)
			{
				var node = constructNode(resource, separator);
				insertNodeIntoTree(root, node, separator);
			}
			return root;
		}

		private static TreeNode constructNode (ResourcePackageEntry entry, char separator)
		{
			var index = entry.Name.LastIndexOf(separator);
			var text  = (index < 0) ? entry.Name : entry.Name.Substring(index + 1);
			var node  = new TreeNode {
				Name = entry.Name,
				Text = text,
				Tag  = entry
			};
			return node;
		}

		private static void insertNodeIntoTree (TreeNode root, TreeNode node, char separator)
		{
			var entry = (ResourcePackageEntry)node.Tag;
			var levels = entry.Name.Split(separator);
			var name = String.Empty;
			var curNode = root;
			for(var i = 1; i < levels.Length; ++i)
			{
				name += levels[i];
				var parent = findNodeByName(curNode.Nodes, name);
				if(parent == null)
				{// Create the container node
					var container = new TreeNode {Name = name};
					curNode.Nodes.Add(container);
				}
				curNode = parent;
			}
			curNode.Nodes.Add(node);
		}

		private static TreeNode findNodeByName (IEnumerable nodes, string name)
		{
			return nodes.Cast<TreeNode>().FirstOrDefault(node => node.Name == name);
		}
		#endregion
	}
}
