namespace Frost.ResourcePackagerGui
{
	partial class ResourcePackageExplorer
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.treeView = new System.Windows.Forms.TreeView();
			this.pathTextBox = new System.Windows.Forms.TextBox();
			this.typeIcon = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.typeIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.treeView, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.pathTextBox, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.typeIcon, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(338, 406);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// treeView
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.treeView, 2);
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.Location = new System.Drawing.Point(3, 29);
			this.treeView.Name = "treeView";
			this.treeView.PathSeparator = "/";
			this.treeView.Size = new System.Drawing.Size(332, 374);
			this.treeView.TabIndex = 1;
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			// 
			// pathTextBox
			// 
			this.pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.pathTextBox.Location = new System.Drawing.Point(25, 3);
			this.pathTextBox.Name = "pathTextBox";
			this.pathTextBox.ReadOnly = true;
			this.pathTextBox.Size = new System.Drawing.Size(310, 20);
			this.pathTextBox.TabIndex = 2;
			// 
			// typeIcon
			// 
			this.typeIcon.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.typeIcon.Location = new System.Drawing.Point(3, 5);
			this.typeIcon.Name = "typeIcon";
			this.typeIcon.Size = new System.Drawing.Size(16, 16);
			this.typeIcon.TabIndex = 3;
			this.typeIcon.TabStop = false;
			// 
			// ResourcePackageExplorer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ResourcePackageExplorer";
			this.Size = new System.Drawing.Size(338, 406);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.typeIcon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.TextBox pathTextBox;
		private System.Windows.Forms.PictureBox typeIcon;
	}
}
