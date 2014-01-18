namespace Frost.TntEditor.NodeValueControls
{
	partial class ColorNodeValueControl
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
			this.colorLabel = new System.Windows.Forms.Label();
			this.alphaLabel = new System.Windows.Forms.Label();
			this.colorBox = new System.Windows.Forms.PictureBox();
			this.colorTextBox = new System.Windows.Forms.TextBox();
			this.selectButton = new System.Windows.Forms.Button();
			this.alphaNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.colorBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.alphaNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 4;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.Controls.Add(this.colorLabel, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.alphaLabel, 0, 1);
			this.tableLayoutPanel.Controls.Add(this.colorBox, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.colorTextBox, 2, 0);
			this.tableLayoutPanel.Controls.Add(this.selectButton, 3, 0);
			this.tableLayoutPanel.Controls.Add(this.alphaNumericUpDown, 1, 1);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 2;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(295, 53);
			this.tableLayoutPanel.TabIndex = 1;
			// 
			// colorLabel
			// 
			this.colorLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.colorLabel.AutoSize = true;
			this.colorLabel.Location = new System.Drawing.Point(3, 8);
			this.colorLabel.Name = "colorLabel";
			this.colorLabel.Size = new System.Drawing.Size(34, 13);
			this.colorLabel.TabIndex = 1;
			this.colorLabel.Text = "Color:";
			// 
			// alphaLabel
			// 
			this.alphaLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.alphaLabel.AutoSize = true;
			this.alphaLabel.Location = new System.Drawing.Point(3, 34);
			this.alphaLabel.Name = "alphaLabel";
			this.alphaLabel.Size = new System.Drawing.Size(37, 13);
			this.alphaLabel.TabIndex = 3;
			this.alphaLabel.Text = "Alpha:";
			// 
			// colorBox
			// 
			this.colorBox.BackColor = System.Drawing.Color.White;
			this.colorBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.colorBox.Location = new System.Drawing.Point(46, 3);
			this.colorBox.Name = "colorBox";
			this.colorBox.Size = new System.Drawing.Size(23, 23);
			this.colorBox.TabIndex = 4;
			this.colorBox.TabStop = false;
			// 
			// colorTextBox
			// 
			this.colorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.colorTextBox.Location = new System.Drawing.Point(75, 4);
			this.colorTextBox.MaxLength = 6;
			this.colorTextBox.Name = "colorTextBox";
			this.colorTextBox.Size = new System.Drawing.Size(136, 20);
			this.colorTextBox.TabIndex = 5;
			this.colorTextBox.TextChanged += new System.EventHandler(this.colorTextBox_TextChanged);
			// 
			// selectButton
			// 
			this.selectButton.Location = new System.Drawing.Point(217, 3);
			this.selectButton.Name = "selectButton";
			this.selectButton.Size = new System.Drawing.Size(75, 23);
			this.selectButton.TabIndex = 6;
			this.selectButton.Text = "Select...";
			this.selectButton.UseVisualStyleBackColor = true;
			this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
			// 
			// alphaNumericUpDown
			// 
			this.tableLayoutPanel.SetColumnSpan(this.alphaNumericUpDown, 3);
			this.alphaNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.alphaNumericUpDown.Location = new System.Drawing.Point(46, 32);
			this.alphaNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.alphaNumericUpDown.Name = "alphaNumericUpDown";
			this.alphaNumericUpDown.Size = new System.Drawing.Size(246, 20);
			this.alphaNumericUpDown.TabIndex = 7;
			// 
			// colorDialog
			// 
			this.colorDialog.AnyColor = true;
			this.colorDialog.FullOpen = true;
			// 
			// ColorNodeValueControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorNodeValueControl";
			this.Size = new System.Drawing.Size(295, 53);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.colorBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.alphaNumericUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label colorLabel;
		private System.Windows.Forms.Label alphaLabel;
		private System.Windows.Forms.PictureBox colorBox;
		private System.Windows.Forms.TextBox colorTextBox;
		private System.Windows.Forms.Button selectButton;
		private System.Windows.Forms.NumericUpDown alphaNumericUpDown;
		private System.Windows.Forms.ColorDialog colorDialog;
	}
}
