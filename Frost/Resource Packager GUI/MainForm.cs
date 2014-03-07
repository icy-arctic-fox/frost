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
			var tryAgain = false;
			do
			{
				try
				{
					package = new ResourcePackageReader(filename, password);
				}
				catch(CryptographicException)
				{// Password is incorrect
					var passwordPrompt = new PasswordPromptDialog();
					if(passwordPrompt.ShowDialog() == DialogResult.OK)
					{
						password = passwordPrompt.Password;
						tryAgain = true;
						continue; // Try with new password
					}
					const string errorText = "The resource package requires a package to open it.";
					tryAgain = (MessageBox.Show(errorText, "Incorrect Password", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) ==
								DialogResult.Retry);
				}
				catch(Exception e)
				{
					var errorText = String.Format("An error was encountered while attempting to load the resource package.\n\n{0}", e.Message);
					tryAgain = (MessageBox.Show(errorText, "Load Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) ==
								DialogResult.Retry);
				}
			} while(package == null && tryAgain);
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
