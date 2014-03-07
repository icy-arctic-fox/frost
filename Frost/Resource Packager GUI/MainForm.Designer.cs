namespace Frost.ResourcePackagerGui
{
	partial class MainForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.resourcePackageExplorer1 = new Frost.ResourcePackagerGui.ResourcePackageExplorer();
			this.loadToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.Location = new System.Drawing.Point(0, 339);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(624, 22);
			this.statusStrip.TabIndex = 1;
			this.statusStrip.Text = "statusStrip1";
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 25);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.resourcePackageExplorer1);
			this.splitContainer.Size = new System.Drawing.Size(624, 314);
			this.splitContainer.SplitterDistance = 300;
			this.splitContainer.TabIndex = 2;
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripButton});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(624, 25);
			this.toolStrip.TabIndex = 3;
			// 
			// resourcePackageExplorer1
			// 
			this.resourcePackageExplorer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resourcePackageExplorer1.Location = new System.Drawing.Point(0, 0);
			this.resourcePackageExplorer1.Name = "resourcePackageExplorer1";
			this.resourcePackageExplorer1.Size = new System.Drawing.Size(320, 314);
			this.resourcePackageExplorer1.TabIndex = 0;
			// 
			// loadToolStripButton
			// 
			this.loadToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("loadToolStripButton.Image")));
			this.loadToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.loadToolStripButton.Name = "loadToolStripButton";
			this.loadToolStripButton.Size = new System.Drawing.Size(53, 22);
			this.loadToolStripButton.Text = "Load";
			this.loadToolStripButton.Click += new System.EventHandler(this.loadToolStripButton_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "*.frp";
			this.openFileDialog.Filter = "Resource Packages|*.frp|All files|*.*";
			this.openFileDialog.Title = "Open Resource Package";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 361);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.statusStrip);
			this.Name = "MainForm";
			this.Text = "Resource Packager";
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.ToolStrip toolStrip;
		private ResourcePackageExplorer resourcePackageExplorer1;
		private System.Windows.Forms.ToolStripButton loadToolStripButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
	}
}

