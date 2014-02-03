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
			this.locationCombo = new System.Windows.Forms.ComboBox();
			this.systemTreeView = new System.Windows.Forms.TreeView();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1.SuspendLayout();
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
			this.systemTreeView.Location = new System.Drawing.Point(3, 30);
			this.systemTreeView.Name = "systemTreeView";
			this.systemTreeView.Size = new System.Drawing.Size(392, 427);
			this.systemTreeView.TabIndex = 1;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.locationCombo, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.systemTreeView, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(398, 460);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// FileExplorer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "FileExplorer";
			this.Size = new System.Drawing.Size(398, 460);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox locationCombo;
		private System.Windows.Forms.TreeView systemTreeView;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}
