namespace Frost.TntEditor
{
	internal partial class NodeInfoPanel
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
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
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NodeInfoPanel));
			this.nodeInfoLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.applyButton = new System.Windows.Forms.Button();
			this.nameLabel = new System.Windows.Forms.Label();
			this.pathLabel = new System.Windows.Forms.Label();
			this.typeLabel = new System.Windows.Forms.Label();
			this.typePicture = new System.Windows.Forms.PictureBox();
			this.pathText = new System.Windows.Forms.TextBox();
			this.nameText = new System.Windows.Forms.TextBox();
			this.typeCombo = new System.Windows.Forms.ComboBox();
			this.nodeTypeImageList = new System.Windows.Forms.ImageList(this.components);
			this.nodeInfoLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.typePicture)).BeginInit();
			this.SuspendLayout();
			// 
			// nodeInfoLayoutPanel
			// 
			this.nodeInfoLayoutPanel.ColumnCount = 3;
			this.nodeInfoLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.nodeInfoLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.nodeInfoLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.nodeInfoLayoutPanel.Controls.Add(this.applyButton, 2, 5);
			this.nodeInfoLayoutPanel.Controls.Add(this.nameLabel, 0, 0);
			this.nodeInfoLayoutPanel.Controls.Add(this.pathLabel, 0, 1);
			this.nodeInfoLayoutPanel.Controls.Add(this.typeLabel, 0, 2);
			this.nodeInfoLayoutPanel.Controls.Add(this.typePicture, 1, 2);
			this.nodeInfoLayoutPanel.Controls.Add(this.pathText, 1, 1);
			this.nodeInfoLayoutPanel.Controls.Add(this.nameText, 1, 0);
			this.nodeInfoLayoutPanel.Controls.Add(this.typeCombo, 2, 2);
			this.nodeInfoLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nodeInfoLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.nodeInfoLayoutPanel.Name = "nodeInfoLayoutPanel";
			this.nodeInfoLayoutPanel.RowCount = 6;
			this.nodeInfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.nodeInfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.nodeInfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.nodeInfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.nodeInfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.nodeInfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.nodeInfoLayoutPanel.Size = new System.Drawing.Size(170, 163);
			this.nodeInfoLayoutPanel.TabIndex = 1;
			// 
			// applyButton
			// 
			this.applyButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.applyButton.Enabled = false;
			this.applyButton.Location = new System.Drawing.Point(92, 137);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(75, 23);
			this.applyButton.TabIndex = 9;
			this.applyButton.Text = "Apply";
			this.applyButton.UseVisualStyleBackColor = true;
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// nameLabel
			// 
			this.nameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(3, 6);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(38, 13);
			this.nameLabel.TabIndex = 1;
			this.nameLabel.Text = "Name:";
			// 
			// pathLabel
			// 
			this.pathLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.pathLabel.AutoSize = true;
			this.pathLabel.Location = new System.Drawing.Point(3, 32);
			this.pathLabel.Name = "pathLabel";
			this.pathLabel.Size = new System.Drawing.Size(32, 13);
			this.pathLabel.TabIndex = 2;
			this.pathLabel.Text = "Path:";
			// 
			// typeLabel
			// 
			this.typeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.typeLabel.AutoSize = true;
			this.typeLabel.Location = new System.Drawing.Point(3, 59);
			this.typeLabel.Name = "typeLabel";
			this.typeLabel.Size = new System.Drawing.Size(34, 13);
			this.typeLabel.TabIndex = 3;
			this.typeLabel.Text = "Type:";
			// 
			// typePicture
			// 
			this.typePicture.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.typePicture.Location = new System.Drawing.Point(48, 57);
			this.typePicture.Name = "typePicture";
			this.typePicture.Size = new System.Drawing.Size(16, 16);
			this.typePicture.TabIndex = 5;
			this.typePicture.TabStop = false;
			// 
			// pathText
			// 
			this.pathText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.nodeInfoLayoutPanel.SetColumnSpan(this.pathText, 2);
			this.pathText.Location = new System.Drawing.Point(47, 29);
			this.pathText.Name = "pathText";
			this.pathText.ReadOnly = true;
			this.pathText.Size = new System.Drawing.Size(120, 20);
			this.pathText.TabIndex = 6;
			// 
			// nameText
			// 
			this.nameText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.nodeInfoLayoutPanel.SetColumnSpan(this.nameText, 2);
			this.nameText.Location = new System.Drawing.Point(47, 3);
			this.nameText.Name = "nameText";
			this.nameText.Size = new System.Drawing.Size(120, 20);
			this.nameText.TabIndex = 7;
			this.nameText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameText_KeyDown);
			// 
			// typeCombo
			// 
			this.typeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.typeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.typeCombo.FormattingEnabled = true;
			this.typeCombo.Location = new System.Drawing.Point(71, 55);
			this.typeCombo.Name = "typeCombo";
			this.typeCombo.Size = new System.Drawing.Size(96, 21);
			this.typeCombo.TabIndex = 8;
			this.typeCombo.SelectedIndexChanged += new System.EventHandler(this.typeCombo_SelectedIndexChanged);
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
			// NodeInfoPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.nodeInfoLayoutPanel);
			this.Name = "NodeInfoPanel";
			this.Size = new System.Drawing.Size(170, 163);
			this.nodeInfoLayoutPanel.ResumeLayout(false);
			this.nodeInfoLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.typePicture)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel nodeInfoLayoutPanel;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Label pathLabel;
		private System.Windows.Forms.Label typeLabel;
		private System.Windows.Forms.PictureBox typePicture;
		private System.Windows.Forms.TextBox pathText;
		private System.Windows.Forms.TextBox nameText;
		private System.Windows.Forms.ComboBox typeCombo;
		private System.Windows.Forms.ImageList nodeTypeImageList;
	}
}
