namespace Resource_Packager_GUI
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
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.packageTreeView = new System.Windows.Forms.TreeView();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.fileExplorer = new Resource_Packager_GUI.FileExplorer();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
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
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.fileExplorer);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.packageTreeView);
			this.splitContainer.Size = new System.Drawing.Size(624, 314);
			this.splitContainer.SplitterDistance = 300;
			this.splitContainer.TabIndex = 2;
			// 
			// packageTreeView
			// 
			this.packageTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.packageTreeView.Location = new System.Drawing.Point(0, 0);
			this.packageTreeView.Name = "packageTreeView";
			this.packageTreeView.Size = new System.Drawing.Size(320, 314);
			this.packageTreeView.TabIndex = 0;
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(624, 25);
			this.toolStrip.TabIndex = 3;
			// 
			// fileExplorer
			// 
			this.fileExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fileExplorer.Location = new System.Drawing.Point(0, 0);
			this.fileExplorer.Name = "fileExplorer";
			this.fileExplorer.Size = new System.Drawing.Size(300, 314);
			this.fileExplorer.TabIndex = 0;
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
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.TreeView packageTreeView;
		private System.Windows.Forms.ToolStrip toolStrip;
		private FileExplorer fileExplorer;
	}
}

