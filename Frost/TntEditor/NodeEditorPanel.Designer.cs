namespace Frost.TntEditor
{
	partial class NodeEditorPanel
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NodeEditorPanel));
			this.treeView = new System.Windows.Forms.TreeView();
			this.nodeTypeImageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.ImageIndex = 0;
			this.treeView.ImageList = this.nodeTypeImageList;
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 0;
			this.treeView.Size = new System.Drawing.Size(318, 247);
			this.treeView.TabIndex = 0;
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			// 
			// nodeTypeImageList
			// 
			this.nodeTypeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("nodeTypeImageList.ImageStream")));
			this.nodeTypeImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.nodeTypeImageList.Images.SetKeyName(0, "type0.png");
			this.nodeTypeImageList.Images.SetKeyName(1, "type1.png");
			this.nodeTypeImageList.Images.SetKeyName(2, "type2.png");
			this.nodeTypeImageList.Images.SetKeyName(3, "type3.png");
			this.nodeTypeImageList.Images.SetKeyName(4, "type4.png");
			this.nodeTypeImageList.Images.SetKeyName(5, "type5.png");
			this.nodeTypeImageList.Images.SetKeyName(6, "type6.png");
			this.nodeTypeImageList.Images.SetKeyName(7, "type7.png");
			this.nodeTypeImageList.Images.SetKeyName(8, "type8.png");
			this.nodeTypeImageList.Images.SetKeyName(9, "type9.png");
			this.nodeTypeImageList.Images.SetKeyName(10, "type10.png");
			this.nodeTypeImageList.Images.SetKeyName(11, "type11.png");
			this.nodeTypeImageList.Images.SetKeyName(12, "type12.png");
			this.nodeTypeImageList.Images.SetKeyName(13, "type13.png");
			this.nodeTypeImageList.Images.SetKeyName(14, "type14.png");
			this.nodeTypeImageList.Images.SetKeyName(15, "type15.png");
			this.nodeTypeImageList.Images.SetKeyName(16, "type16.png");
			this.nodeTypeImageList.Images.SetKeyName(17, "type17.png");
			this.nodeTypeImageList.Images.SetKeyName(18, "type18.png");
			this.nodeTypeImageList.Images.SetKeyName(19, "type19.png");
			this.nodeTypeImageList.Images.SetKeyName(20, "type20.png");
			this.nodeTypeImageList.Images.SetKeyName(21, "type21.png");
			this.nodeTypeImageList.Images.SetKeyName(22, "type22.png");
			this.nodeTypeImageList.Images.SetKeyName(23, "type23.png");
			// 
			// NodeEditorPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.treeView);
			this.Name = "NodeEditorPanel";
			this.Size = new System.Drawing.Size(318, 247);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.ImageList nodeTypeImageList;
	}
}
