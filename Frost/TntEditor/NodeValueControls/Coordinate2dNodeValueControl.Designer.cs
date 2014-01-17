namespace Frost.TntEditor.NodeValueControls
{
	partial class Coordinate2DNodeValueControl
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
			this.xLabel = new System.Windows.Forms.Label();
			this.xTextBox = new System.Windows.Forms.TextBox();
			this.yLabel = new System.Windows.Forms.Label();
			this.yTextBox = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Controls.Add(this.yTextBox, 0, 1);
			this.tableLayoutPanel.Controls.Add(this.xLabel, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.xTextBox, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.yLabel, 0, 1);
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
			// xLabel
			// 
			this.xLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.xLabel.AutoSize = true;
			this.xLabel.Location = new System.Drawing.Point(3, 6);
			this.xLabel.Name = "xLabel";
			this.xLabel.Size = new System.Drawing.Size(17, 13);
			this.xLabel.TabIndex = 1;
			this.xLabel.Text = "X:";
			// 
			// xTextBox
			// 
			this.xTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xTextBox.Location = new System.Drawing.Point(26, 3);
			this.xTextBox.Name = "xTextBox";
			this.xTextBox.Size = new System.Drawing.Size(266, 20);
			this.xTextBox.TabIndex = 2;
			// 
			// yLabel
			// 
			this.yLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.yLabel.AutoSize = true;
			this.yLabel.Location = new System.Drawing.Point(3, 33);
			this.yLabel.Name = "yLabel";
			this.yLabel.Size = new System.Drawing.Size(17, 13);
			this.yLabel.TabIndex = 3;
			this.yLabel.Text = "Y:";
			// 
			// yTextBox
			// 
			this.yTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.yTextBox.Location = new System.Drawing.Point(26, 29);
			this.yTextBox.Name = "yTextBox";
			this.yTextBox.Size = new System.Drawing.Size(266, 20);
			this.yTextBox.TabIndex = 4;
			// 
			// Coordinate2DNodeValueControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "Coordinate2DNodeValueControl";
			this.Size = new System.Drawing.Size(295, 53);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label xLabel;
		private System.Windows.Forms.TextBox xTextBox;
		private System.Windows.Forms.TextBox yTextBox;
		private System.Windows.Forms.Label yLabel;
	}
}
