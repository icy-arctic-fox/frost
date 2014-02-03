using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Resource_Packager_GUI
{
	public partial class FileExplorer : UserControl
	{
		public FileExplorer ()
		{
			InitializeComponent();
			initializeTreeView();
		}

		private void initializeTreeView (string basePath = null)
		{
			systemTreeView.Nodes.Clear();

			if(basePath == null)
			{// Use drives as root nodes
				foreach(var drive in Environment.GetLogicalDrives())
				{
					var driveNode = constructDriveNode(drive);
					systemTreeView.Nodes.Add(driveNode);
				}
			}

			else
			{// Use a directory as the base path
//				var dirNode = constructDirectoryNode(basePath);
//				systemTreeView.Nodes.Add(dirNode);
			}
		}

		/// <summary>
		/// Constructs a tree node for a drive
		/// </summary>
		/// <param name="drive">Name of the drive</param>
		/// <returns>A constructed tree node</returns>
		private static TreeNode constructDriveNode (string drive)
		{
			var text = drive.Substring(0, drive.Length - 1);
			return new TreeNode {
				Text = text,
				Tag  = drive
			};
		}

		private void populateDirectoryNode (TreeNode dirNode, string path)
		{
			// Populate with shell sub-directories to be expandable later
			foreach(var dir in Directory.EnumerateDirectories(path))
			{
				var subDirName = dir.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1);
				var subDirNode = new TreeNode {
					Text = subDirName,
					Tag  = dir + Path.DirectorySeparatorChar
				};
				dirNode.Nodes.Add(subDirNode);
			}

			// Populate with directory contents
			foreach(var file in Directory.EnumerateFiles(path))
			{
				var fileNode = constructFileNode(file);
				dirNode.Nodes.Add(fileNode);
			}
		}

		private TreeNode constructFileNode (string path)
		{
			var name = Path.GetFileNameWithoutExtension(path) ?? path;

			string extension;
			if(File.Exists(path))
			{
				extension = Path.GetExtension(name);
				if(!iconList.Images.Keys.Contains(extension))
				{// Don't know the icon for the extension, load it
					var icon = Icon.ExtractAssociatedIcon(path) ?? SystemIcons.Question;
					iconList.Images.Add(extension, icon);
				}
			}
			else
				extension = null;

			return new TreeNode {
				Text     = name,
				Tag      = path,
				ImageKey = extension
			};
		}

		private void systemTreeView_NodeMouseClick (object sender, TreeNodeMouseClickEventArgs e)
		{
			var node = e.Node;
			if(!node.IsExpanded)
			{
				var path = node.Tag as string;
				if(path != null && Directory.Exists(path))
				{// Populate the node's children with the contents of the directory
					populateDirectoryNode(node, path);
					systemTreeView.SelectedNode = node;
					node.Expand();
				}
			}
		}
	}
}
