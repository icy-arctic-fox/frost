using System.Windows.Forms;
using Frost.IO.Resources;

namespace Frost.ResourcePackagerGui
{
	public partial class MainForm : Form
	{
		public MainForm ()
		{
			InitializeComponent();
		}

		private void loadToolStripButton_Click (object sender, System.EventArgs e)
		{
			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				var package = new ResourcePackageReader(openFileDialog.FileName);
				resourcePackageExplorer1.DisplayPackageContents(package);
			}
		}
	}
}
