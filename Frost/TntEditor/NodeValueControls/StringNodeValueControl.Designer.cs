namespace Frost.TntEditor.NodeValueControls
{
	partial class StringNodeValueControl
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
			this.valueLabel = new System.Windows.Forms.Label();
			this.textBox = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Controls.Add(this.textBox, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.valueLabel, 0, 0);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 2;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(295, 68);
			this.tableLayoutPanel.TabIndex = 1;
			// 
			// valueLabel
			// 
			this.valueLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.valueLabel.AutoSize = true;
			this.valueLabel.Location = new System.Drawing.Point(3, 7);
			this.valueLabel.Name = "valueLabel";
			this.valueLabel.Size = new System.Drawing.Size(37, 13);
			this.valueLabel.TabIndex = 1;
			this.valueLabel.Text = "Value:";
			// 
			// textBox
			// 
			this.textBox.AcceptsReturn = true;
			this.textBox.AcceptsTab = true;
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.Location = new System.Drawing.Point(46, 3);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.tableLayoutPanel.SetRowSpan(this.textBox, 2);
			this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox.Size = new System.Drawing.Size(246, 62);
			this.textBox.TabIndex = 2;
			// 
			// StringNodeValueControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.MinimumSize = new System.Drawing.Size(100, 28);
			this.Name = "StringNodeValueControl";
			this.Size = new System.Drawing.Size(295, 68);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label valueLabel;
		private System.Windows.Forms.TextBox textBox;
	}
}
