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
using Frost.Utility;

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
			treeView.Nodes.Clear();
			var root = constructTree(package, treeView.PathSeparator[0]);
			treeView.Nodes.Add(root);
		}

		public event EventHandler<ResourceSelectedEventArgs> ResourceSelected;

		#region Tree construction

		private static TreeNode constructTree (ResourcePackage package, char separator)
		{
			var root = constructRootNode(package);
			foreach(var resource in package.Resources)
			{
				var node = constructNode(resource, separator);
				insertNodeIntoTree(root, node, separator);
			}
			return root;
		}

		private static TreeNode constructRootNode (ResourcePackage package)
		{
			return new TreeNode {Text = package.ToString()};
		}

		private static TreeNode constructNode (ResourcePackageEntry entry, char separator)
		{
			var index = entry.Name.LastIndexOf(separator);
			var text  = (index < 0) ? entry.Name : entry.Name.Substring(index + 1);
			return new TreeNode {
				Name = entry.Name,
				Text = text,
				Tag  = entry
			};
		}

		private static void insertNodeIntoTree (TreeNode root, TreeNode node, char separator)
		{
			var entry = (ResourcePackageEntry)node.Tag;
			var levels = entry.Name.Split(separator);
			var name = String.Empty;
			var curNode = root;
			for(var i = 0; i < levels.Length - 1; ++i)
			{
				if(name != String.Empty)
					name += separator;
				name += levels[i];
				var parent = findNodeByName(curNode.Nodes, name);
				if(parent == null)
				{// Create the container node
					parent = new TreeNode {
						Name = name,
						Text = levels[i]
					};
					curNode.Nodes.Add(parent);
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

		private void treeView_AfterSelect (object sender, TreeViewEventArgs e)
		{
			var node = e.Node;
			pathTextBox.Text = node.Name;
			var entry = node.Tag as ResourcePackageEntry;
			if(entry != null)
			{
				var args = new ResourceSelectedEventArgs(entry);
				ResourceSelected.NotifyThreadedSubscribers(this, args);
			}
		}
	}
}
