namespace Frost.TntEditor.NodeValueControls
{
	partial class ListNodeValueControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListNodeValueControl));
			this.comboBox = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.typeLabel = new System.Windows.Forms.Label();
			this.nodeTypeImageList = new System.Windows.Forms.ImageList(this.components);
			this.typePicture = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.typePicture)).BeginInit();
			this.SuspendLayout();
			// 
			// comboBox
			// 
			this.comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox.FormattingEnabled = true;
			this.comboBox.Location = new System.Drawing.Point(106, 3);
			this.comboBox.Name = "comboBox";
			this.comboBox.Size = new System.Drawing.Size(186, 21);
			this.comboBox.TabIndex = 0;
			this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 3;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Controls.Add(this.typePicture, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.comboBox, 2, 0);
			this.tableLayoutPanel.Controls.Add(this.typeLabel, 0, 0);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 1;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(295, 28);
			this.tableLayoutPanel.TabIndex = 1;
			// 
			// typeLabel
			// 
			this.typeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.typeLabel.AutoSize = true;
			this.typeLabel.Location = new System.Drawing.Point(3, 7);
			this.typeLabel.Name = "typeLabel";
			this.typeLabel.Size = new System.Drawing.Size(75, 13);
			this.typeLabel.TabIndex = 1;
			this.typeLabel.Text = "Element Type:";
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
			// typePicture
			// 
			this.typePicture.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.typePicture.Location = new System.Drawing.Point(84, 6);
			this.typePicture.Name = "typePicture";
			this.typePicture.Size = new System.Drawing.Size(16, 16);
			this.typePicture.TabIndex = 6;
			this.typePicture.TabStop = false;
			// 
			// ListNodeValueControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ListNodeValueControl";
			this.Size = new System.Drawing.Size(295, 28);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.typePicture)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox comboBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label typeLabel;
		private System.Windows.Forms.ImageList nodeTypeImageList;
		private System.Windows.Forms.PictureBox typePicture;
	}
}
