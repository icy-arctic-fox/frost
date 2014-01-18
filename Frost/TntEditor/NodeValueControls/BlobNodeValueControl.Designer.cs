namespace Frost.TntEditor.NodeValueControls
{
	partial class BlobNodeValueControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.sizeLabel = new System.Windows.Forms.Label();
			this.bytesLabel = new System.Windows.Forms.Label();
			this.importButton = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.exportButton = new System.Windows.Forms.Button();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 4;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.Controls.Add(this.sizeLabel, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.bytesLabel, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.importButton, 2, 0);
			this.tableLayoutPanel.Controls.Add(this.exportButton, 3, 0);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 1;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(295, 28);
			this.tableLayoutPanel.TabIndex = 1;
			// 
			// sizeLabel
			// 
			this.sizeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.sizeLabel.AutoSize = true;
			this.sizeLabel.Location = new System.Drawing.Point(3, 7);
			this.sizeLabel.Name = "sizeLabel";
			this.sizeLabel.Size = new System.Drawing.Size(30, 13);
			this.sizeLabel.TabIndex = 1;
			this.sizeLabel.Text = "Size:";
			// 
			// bytesLabel
			// 
			this.bytesLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.bytesLabel.AutoSize = true;
			this.bytesLabel.Location = new System.Drawing.Point(39, 7);
			this.bytesLabel.Name = "bytesLabel";
			this.bytesLabel.Size = new System.Drawing.Size(48, 13);
			this.bytesLabel.TabIndex = 2;
			this.bytesLabel.Text = "__ Bytes";
			// 
			// importButton
			// 
			this.importButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.importButton.Location = new System.Drawing.Point(136, 3);
			this.importButton.Name = "importButton";
			this.importButton.Size = new System.Drawing.Size(75, 22);
			this.importButton.TabIndex = 3;
			this.importButton.Text = "Import...";
			this.importButton.UseVisualStyleBackColor = true;
			this.importButton.Click += new System.EventHandler(this.importButton_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Title = "Import File";
			// 
			// exportButton
			// 
			this.exportButton.Location = new System.Drawing.Point(217, 3);
			this.exportButton.Name = "exportButton";
			this.exportButton.Size = new System.Drawing.Size(75, 22);
			this.exportButton.TabIndex = 4;
			this.exportButton.Text = "Export...";
			this.exportButton.UseVisualStyleBackColor = true;
			this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.AddExtension = false;
			this.saveFileDialog.Title = "Export Blob";
			// 
			// BlobNodeValueControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "BlobNodeValueControl";
			this.Size = new System.Drawing.Size(295, 28);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label sizeLabel;
		private System.Windows.Forms.Label bytesLabel;
		private System.Windows.Forms.Button importButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button exportButton;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
	}
}
