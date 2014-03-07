using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using Frost.IO.Resources;

namespace Frost.ResourcePackagerGui
{
	public partial class MainForm : Form
	{
		private ResourcePackage _activePackage;

		public MainForm ()
		{
			InitializeComponent();
		}

		#region Save and load

		private static ResourcePackage loadPackage (string filename)
		{
			ResourcePackage package = null;
			string password = null;
			var loaded = false;
			var errorText = String.Empty;
			do
			{
				try
				{
					package = new ResourcePackageReader(filename, password);
					loaded = true;
				}
				catch(CryptographicException)
				{// Password is incorrect
					// TODO: Prompt for password
				}
				catch(Exception e)
				{
					errorText = String.Format("An error was encountered while attempting to load the resource package.\n\n{0}", e.Message);
				}
			} while(!loaded && MessageBox.Show(errorText, "Load Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry);
			return package;
		}
		#endregion

		#region Listeners

		private void loadToolStripButton_Click (object sender, EventArgs args)
		{
			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				var package = loadPackage(openFileDialog.FileName);
				if(package != null)
				{
					_activePackage = package;
					resourcePackageExplorer1.DisplayPackageContents(package);
				}
			}
		}
		#endregion
	}
}
