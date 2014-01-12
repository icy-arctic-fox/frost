namespace Frost.TntEditor
{
	partial class NodeInfo
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
			this.nodeInfoLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.applyButton = new System.Windows.Forms.Button();
			this.nameLabel = new System.Windows.Forms.Label();
			this.pathLabel = new System.Windows.Forms.Label();
			this.typeLabel = new System.Windows.Forms.Label();
			this.valueLabel = new System.Windows.Forms.Label();
			this.pathText = new System.Windows.Forms.TextBox();
			this.nameText = new System.Windows.Forms.TextBox();
			this.typeCombo = new System.Windows.Forms.ComboBox();
			this.valueBox = new System.Windows.Forms.TextBox();
			this.typePicture = new System.Windows.Forms.PictureBox();
			this.nodeInfoLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.typePicture)).BeginInit();
			this.SuspendLayout();
			// 
			// nodeInfoLayoutPanel
			// 
			this.nodeInfoLayoutPanel.ColumnCount = 3;
			this.nodeInfoLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.nodeInfoLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.nodeInfoLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.nodeInfoLayoutPanel.Controls.Add(this.applyButton, 2, 5);
			this.nodeInfoLayoutPanel.Controls.Add(this.nameLabel, 0, 0);
			this.nodeInfoLayoutPanel.Controls.Add(this.pathLabel, 0, 1);
			this.nodeInfoLayoutPanel.Controls.Add(this.typeLabel, 0, 2);
			this.nodeInfoLayoutPanel.Controls.Add(this.valueLabel, 0, 3);
			this.nodeInfoLayoutPanel.Controls.Add(this.typePicture, 1, 2);
			this.nodeInfoLayoutPanel.Controls.Add(this.pathText, 1, 1);
			this.nodeInfoLayoutPanel.Controls.Add(this.nameText, 1, 0);
			this.nodeInfoLayoutPanel.Controls.Add(this.typeCombo, 2, 2);
			this.nodeInfoLayoutPanel.Controls.Add(this.valueBox, 1, 3);
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
			this.nodeInfoLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.nodeInfoLayoutPanel.Size = new System.Drawing.Size(306, 161);
			this.nodeInfoLayoutPanel.TabIndex = 1;
			// 
			// applyButton
			// 
			this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.applyButton.Location = new System.Drawing.Point(228, 135);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(75, 23);
			this.applyButton.TabIndex = 9;
			this.applyButton.Text = "Apply";
			this.applyButton.UseVisualStyleBackColor = true;
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
			// valueLabel
			// 
			this.valueLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.valueLabel.AutoSize = true;
			this.valueLabel.Location = new System.Drawing.Point(3, 85);
			this.valueLabel.Name = "valueLabel";
			this.valueLabel.Size = new System.Drawing.Size(37, 13);
			this.valueLabel.TabIndex = 7;
			this.valueLabel.Text = "Value:";
			// 
			// pathText
			// 
			this.nodeInfoLayoutPanel.SetColumnSpan(this.pathText, 2);
			this.pathText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pathText.Location = new System.Drawing.Point(47, 29);
			this.pathText.Name = "pathText";
			this.pathText.ReadOnly = true;
			this.pathText.Size = new System.Drawing.Size(256, 20);
			this.pathText.TabIndex = 6;
			// 
			// nameText
			// 
			this.nodeInfoLayoutPanel.SetColumnSpan(this.nameText, 2);
			this.nameText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nameText.Location = new System.Drawing.Point(47, 3);
			this.nameText.Name = "nameText";
			this.nameText.Size = new System.Drawing.Size(256, 20);
			this.nameText.TabIndex = 7;
			// 
			// typeCombo
			// 
			this.typeCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.typeCombo.FormattingEnabled = true;
			this.typeCombo.Location = new System.Drawing.Point(71, 55);
			this.typeCombo.Name = "typeCombo";
			this.typeCombo.Size = new System.Drawing.Size(232, 21);
			this.typeCombo.TabIndex = 8;
			// 
			// valueBox
			// 
			this.nodeInfoLayoutPanel.SetColumnSpan(this.valueBox, 2);
			this.valueBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.valueBox.Location = new System.Drawing.Point(47, 82);
			this.valueBox.Name = "valueBox";
			this.valueBox.Size = new System.Drawing.Size(256, 20);
			this.valueBox.TabIndex = 8;
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
			// NodeInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.nodeInfoLayoutPanel);
			this.Name = "NodeInfo";
			this.Size = new System.Drawing.Size(306, 161);
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
		private System.Windows.Forms.Label valueLabel;
		private System.Windows.Forms.PictureBox typePicture;
		private System.Windows.Forms.TextBox pathText;
		private System.Windows.Forms.TextBox nameText;
		private System.Windows.Forms.ComboBox typeCombo;
		private System.Windows.Forms.TextBox valueBox;
	}
}
