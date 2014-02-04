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

		/// <summary>
		/// Populates an existing directory node with the directory's contents
		/// </summary>
		/// <param name="dirNode">Node to populate</param>
		/// <param name="path">Path to the directory to pull contents from</param>
		private void populateDirectoryNode (TreeNode dirNode, string path)
		{
			dirNode.Nodes.Clear();

			try
			{
				// Populate with shell sub-directories to be expandable later
				foreach(var dir in Directory.EnumerateDirectories(path))
				{
					var attr = File.GetAttributes(dir);
					if((attr & FileAttributes.System) == FileAttributes.System)
						continue; // Skip system files and folders
					var subDirName = dir.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1);
					var subDirNode = new TreeNode {
						Text = subDirName,
						Tag = dir + Path.DirectorySeparatorChar
					};
					dirNode.Nodes.Add(subDirNode);
				}

				// Populate with directory contents
				foreach(var file in Directory.EnumerateFiles(path))
				{
					var attr = File.GetAttributes(file);
					if((attr & FileAttributes.System) == FileAttributes.System)
						continue; // Skip system files and folders
					var fileNode = constructFileNode(file);
					dirNode.Nodes.Add(fileNode);
				}
			}
			catch(IOException e)
			{
				MessageBox.Show(e.Message, "IO Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch(UnauthorizedAccessException e)
			{
				MessageBox.Show(e.Message, "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Creates a tree node for a file
		/// </summary>
		/// <param name="path">Path to the file</param>
		/// <returns>Constructed tree node</returns>
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

		private void systemTreeView_BeforeSelect (object sender, TreeViewCancelEventArgs e)
		{
			var node = e.Node;
			if(node.Nodes.Count <= 0)
			{// Attempt to populate the node's children
				var path = node.Tag as string;
				if(path != null && Directory.Exists(path))
					populateDirectoryNode(node, path);
			}
		}

		private void systemTreeView_AfterCollapse (object sender, TreeViewEventArgs e)
		{
			e.Node.Nodes.Clear();
		}
	}
}
