﻿namespace Frost.TntEditor.NodeValueControls
{
	partial class Point3iNodeValueControl
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
			this.yLabel = new System.Windows.Forms.Label();
			this.xLabel = new System.Windows.Forms.Label();
			this.xNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.yNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.zLabel = new System.Windows.Forms.Label();
			this.zNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.xNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.yNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.zNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Controls.Add(this.yLabel, 0, 1);
			this.tableLayoutPanel.Controls.Add(this.xLabel, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.xNumericUpDown, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.yNumericUpDown, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.zNumericUpDown, 1, 2);
			this.tableLayoutPanel.Controls.Add(this.zLabel, 0, 2);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 3;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(246, 78);
			this.tableLayoutPanel.TabIndex = 1;
			// 
			// yLabel
			// 
			this.yLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.yLabel.AutoSize = true;
			this.yLabel.Location = new System.Drawing.Point(3, 32);
			this.yLabel.Name = "yLabel";
			this.yLabel.Size = new System.Drawing.Size(17, 13);
			this.yLabel.TabIndex = 4;
			this.yLabel.Text = "Y:";
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
			// xNumericUpDown
			// 
			this.xNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xNumericUpDown.Location = new System.Drawing.Point(26, 3);
			this.xNumericUpDown.Name = "xNumericUpDown";
			this.xNumericUpDown.Size = new System.Drawing.Size(217, 20);
			this.xNumericUpDown.TabIndex = 5;
			// 
			// yNumericUpDown
			// 
			this.yNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.yNumericUpDown.Location = new System.Drawing.Point(26, 29);
			this.yNumericUpDown.Name = "yNumericUpDown";
			this.yNumericUpDown.Size = new System.Drawing.Size(217, 20);
			this.yNumericUpDown.TabIndex = 6;
			// 
			// zLabel
			// 
			this.zLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.zLabel.AutoSize = true;
			this.zLabel.Location = new System.Drawing.Point(3, 58);
			this.zLabel.Name = "zLabel";
			this.zLabel.Size = new System.Drawing.Size(17, 13);
			this.zLabel.TabIndex = 7;
			this.zLabel.Text = "Z:";
			// 
			// zNumericUpDown
			// 
			this.zNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.zNumericUpDown.Location = new System.Drawing.Point(26, 55);
			this.zNumericUpDown.Name = "zNumericUpDown";
			this.zNumericUpDown.Size = new System.Drawing.Size(217, 20);
			this.zNumericUpDown.TabIndex = 8;
			// 
			// XyzNodeValueControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "Point3iNodeValueControl";
			this.Size = new System.Drawing.Size(246, 78);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.xNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.yNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.zNumericUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label xLabel;
		private System.Windows.Forms.Label yLabel;
		private System.Windows.Forms.NumericUpDown xNumericUpDown;
		private System.Windows.Forms.NumericUpDown yNumericUpDown;
		private System.Windows.Forms.NumericUpDown zNumericUpDown;
		private System.Windows.Forms.Label zLabel;
	}
}
