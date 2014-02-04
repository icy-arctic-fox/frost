namespace Resource_Packager_GUI
{
	partial class FileExplorer
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
			this.components = new System.ComponentModel.Container();
			this.locationCombo = new System.Windows.Forms.ComboBox();
			this.systemTreeView = new System.Windows.Forms.TreeView();
			this.iconList = new System.Windows.Forms.ImageList(this.components);
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// locationCombo
			// 
			this.locationCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locationCombo.FormattingEnabled = true;
			this.locationCombo.Location = new System.Drawing.Point(3, 3);
			this.locationCombo.Name = "locationCombo";
			this.locationCombo.Size = new System.Drawing.Size(392, 21);
			this.locationCombo.TabIndex = 0;
			// 
			// systemTreeView
			// 
			this.systemTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.systemTreeView.ImageIndex = 0;
			this.systemTreeView.ImageList = this.iconList;
			this.systemTreeView.Location = new System.Drawing.Point(3, 30);
			this.systemTreeView.Name = "systemTreeView";
			this.systemTreeView.SelectedImageIndex = 0;
			this.systemTreeView.Size = new System.Drawing.Size(392, 427);
			this.systemTreeView.TabIndex = 1;
			this.systemTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.systemTreeView_AfterCollapse);
			this.systemTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.systemTreeView_BeforeSelect);
			// 
			// iconList
			// 
			this.iconList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.iconList.ImageSize = new System.Drawing.Size(16, 16);
			this.iconList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 1;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Controls.Add(this.locationCombo, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.systemTreeView, 0, 1);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 2;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(398, 460);
			this.tableLayoutPanel.TabIndex = 2;
			// 
			// FileExplorer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Name = "FileExplorer";
			this.Size = new System.Drawing.Size(398, 460);
			this.tableLayoutPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox locationCombo;
		private System.Windows.Forms.TreeView systemTreeView;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.ImageList iconList;
	}
}
