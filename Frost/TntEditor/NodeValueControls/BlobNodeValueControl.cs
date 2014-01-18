using System;
using System.IO;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class BlobNodeValueControl : UserControl, INodeEditorControl
	{
		public BlobNodeValueControl ()
		{
			InitializeComponent();
		}

		private byte[] _data;

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			return new BlobNode(_data);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			var blob = (BlobNode)node;
			_data = blob.Data;
			updateSizeLabel();
		}

		private readonly string[] _unitLabels = new[] {"bytes", "KB", "MB", "GB", "TB", "PB"};

		private void updateSizeLabel ()
		{
			var bytes = (double)_data.LongLength;
			var units = 0;
			while(bytes > 1000d)
			{
				bytes /= 1024d;
				++units;
			}
			var unitLabel   = _unitLabels[units];
			var text        = String.Format(units > 0 ? "{0:F} {1}" : "{0} {1}", bytes, unitLabel);
			bytesLabel.Text = text;
		}

		private void importButton_Click (object sender, EventArgs args)
		{
			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					_data = File.ReadAllBytes(openFileDialog.FileName);
				}
				catch(Exception e)
				{
					MessageBox.Show("An error occurred while reading the file." + Environment.NewLine + e.Message, "File Import Error",
					                MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				updateSizeLabel();
			}
		}

		private void exportButton_Click (object sender, EventArgs args)
		{
			if(saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					File.WriteAllBytes(saveFileDialog.FileName, _data);
				}
				catch(Exception e)
				{
					MessageBox.Show("An error occurred while writing the file." + Environment.NewLine + e.Message, "File Export Error",
					                MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}
