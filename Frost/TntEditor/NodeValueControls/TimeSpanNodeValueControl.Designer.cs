namespace Frost.TntEditor.NodeValueControls
{
	partial class TimeSpanNodeValueControl
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
			this.ticksNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.ticksLabel = new System.Windows.Forms.Label();
			this.msNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.msLabel = new System.Windows.Forms.Label();
			this.secondsNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.minutesNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.hoursNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.daysLabel = new System.Windows.Forms.Label();
			this.hoursLabel = new System.Windows.Forms.Label();
			this.minutesLabel = new System.Windows.Forms.Label();
			this.secondsLabel = new System.Windows.Forms.Label();
			this.daysNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.negativeCheckBox = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ticksNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.msNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.secondsNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.minutesNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.hoursNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.daysNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Controls.Add(this.ticksNumericUpDown, 1, 6);
			this.tableLayoutPanel.Controls.Add(this.ticksLabel, 0, 6);
			this.tableLayoutPanel.Controls.Add(this.msNumericUpDown, 1, 5);
			this.tableLayoutPanel.Controls.Add(this.msLabel, 0, 5);
			this.tableLayoutPanel.Controls.Add(this.secondsNumericUpDown, 1, 4);
			this.tableLayoutPanel.Controls.Add(this.minutesNumericUpDown, 1, 3);
			this.tableLayoutPanel.Controls.Add(this.hoursNumericUpDown, 1, 2);
			this.tableLayoutPanel.Controls.Add(this.daysLabel, 0, 1);
			this.tableLayoutPanel.Controls.Add(this.hoursLabel, 0, 2);
			this.tableLayoutPanel.Controls.Add(this.minutesLabel, 0, 3);
			this.tableLayoutPanel.Controls.Add(this.secondsLabel, 0, 4);
			this.tableLayoutPanel.Controls.Add(this.daysNumericUpDown, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.negativeCheckBox, 0, 0);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 8;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(295, 189);
			this.tableLayoutPanel.TabIndex = 1;
			// 
			// ticksNumericUpDown
			// 
			this.ticksNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ticksNumericUpDown.Location = new System.Drawing.Point(76, 156);
			this.ticksNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.ticksNumericUpDown.Name = "ticksNumericUpDown";
			this.ticksNumericUpDown.Size = new System.Drawing.Size(216, 20);
			this.ticksNumericUpDown.TabIndex = 15;
			this.ticksNumericUpDown.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
			// 
			// ticksLabel
			// 
			this.ticksLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.ticksLabel.AutoSize = true;
			this.ticksLabel.Location = new System.Drawing.Point(3, 159);
			this.ticksLabel.Name = "ticksLabel";
			this.ticksLabel.Size = new System.Drawing.Size(36, 13);
			this.ticksLabel.TabIndex = 14;
			this.ticksLabel.Text = "Ticks:";
			// 
			// msNumericUpDown
			// 
			this.msNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.msNumericUpDown.Location = new System.Drawing.Point(76, 130);
			this.msNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.msNumericUpDown.Name = "msNumericUpDown";
			this.msNumericUpDown.Size = new System.Drawing.Size(216, 20);
			this.msNumericUpDown.TabIndex = 13;
			this.msNumericUpDown.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
			// 
			// msLabel
			// 
			this.msLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.msLabel.AutoSize = true;
			this.msLabel.Location = new System.Drawing.Point(3, 133);
			this.msLabel.Name = "msLabel";
			this.msLabel.Size = new System.Drawing.Size(67, 13);
			this.msLabel.TabIndex = 12;
			this.msLabel.Text = "Milliseconds:";
			// 
			// secondsNumericUpDown
			// 
			this.secondsNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.secondsNumericUpDown.Location = new System.Drawing.Point(76, 104);
			this.secondsNumericUpDown.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.secondsNumericUpDown.Name = "secondsNumericUpDown";
			this.secondsNumericUpDown.Size = new System.Drawing.Size(216, 20);
			this.secondsNumericUpDown.TabIndex = 11;
			this.secondsNumericUpDown.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
			// 
			// minutesNumericUpDown
			// 
			this.minutesNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.minutesNumericUpDown.Location = new System.Drawing.Point(76, 78);
			this.minutesNumericUpDown.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.minutesNumericUpDown.Name = "minutesNumericUpDown";
			this.minutesNumericUpDown.Size = new System.Drawing.Size(216, 20);
			this.minutesNumericUpDown.TabIndex = 10;
			this.minutesNumericUpDown.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
			// 
			// hoursNumericUpDown
			// 
			this.hoursNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hoursNumericUpDown.Location = new System.Drawing.Point(76, 52);
			this.hoursNumericUpDown.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
			this.hoursNumericUpDown.Name = "hoursNumericUpDown";
			this.hoursNumericUpDown.Size = new System.Drawing.Size(216, 20);
			this.hoursNumericUpDown.TabIndex = 9;
			this.hoursNumericUpDown.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
			// 
			// daysLabel
			// 
			this.daysLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.daysLabel.AutoSize = true;
			this.daysLabel.Location = new System.Drawing.Point(3, 29);
			this.daysLabel.Name = "daysLabel";
			this.daysLabel.Size = new System.Drawing.Size(34, 13);
			this.daysLabel.TabIndex = 1;
			this.daysLabel.Text = "Days:";
			// 
			// hoursLabel
			// 
			this.hoursLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.hoursLabel.AutoSize = true;
			this.hoursLabel.Location = new System.Drawing.Point(3, 55);
			this.hoursLabel.Name = "hoursLabel";
			this.hoursLabel.Size = new System.Drawing.Size(38, 13);
			this.hoursLabel.TabIndex = 3;
			this.hoursLabel.Text = "Hours:";
			// 
			// minutesLabel
			// 
			this.minutesLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.minutesLabel.AutoSize = true;
			this.minutesLabel.Location = new System.Drawing.Point(3, 81);
			this.minutesLabel.Name = "minutesLabel";
			this.minutesLabel.Size = new System.Drawing.Size(47, 13);
			this.minutesLabel.TabIndex = 5;
			this.minutesLabel.Text = "Minutes:";
			// 
			// secondsLabel
			// 
			this.secondsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.secondsLabel.AutoSize = true;
			this.secondsLabel.Location = new System.Drawing.Point(3, 107);
			this.secondsLabel.Name = "secondsLabel";
			this.secondsLabel.Size = new System.Drawing.Size(52, 13);
			this.secondsLabel.TabIndex = 7;
			this.secondsLabel.Text = "Seconds:";
			// 
			// daysNumericUpDown
			// 
			this.daysNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.daysNumericUpDown.Location = new System.Drawing.Point(76, 26);
			this.daysNumericUpDown.Name = "daysNumericUpDown";
			this.daysNumericUpDown.Size = new System.Drawing.Size(216, 20);
			this.daysNumericUpDown.TabIndex = 8;
			// 
			// negativeCheckBox
			// 
			this.negativeCheckBox.AutoSize = true;
			this.tableLayoutPanel.SetColumnSpan(this.negativeCheckBox, 2);
			this.negativeCheckBox.Location = new System.Drawing.Point(3, 3);
			this.negativeCheckBox.Name = "negativeCheckBox";
			this.negativeCheckBox.Size = new System.Drawing.Size(123, 17);
			this.negativeCheckBox.TabIndex = 16;
			this.negativeCheckBox.Text = "Past (negative span)";
			this.negativeCheckBox.UseVisualStyleBackColor = true;
			// 
			// TimeSpanNodeValueControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "TimeSpanNodeValueControl";
			this.Size = new System.Drawing.Size(295, 189);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ticksNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.msNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.secondsNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.minutesNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.hoursNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.daysNumericUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label daysLabel;
		private System.Windows.Forms.Label hoursLabel;
		private System.Windows.Forms.Label minutesLabel;
		private System.Windows.Forms.Label secondsLabel;
		private System.Windows.Forms.NumericUpDown ticksNumericUpDown;
		private System.Windows.Forms.Label ticksLabel;
		private System.Windows.Forms.NumericUpDown msNumericUpDown;
		private System.Windows.Forms.Label msLabel;
		private System.Windows.Forms.NumericUpDown secondsNumericUpDown;
		private System.Windows.Forms.NumericUpDown minutesNumericUpDown;
		private System.Windows.Forms.NumericUpDown hoursNumericUpDown;
		private System.Windows.Forms.NumericUpDown daysNumericUpDown;
		private System.Windows.Forms.CheckBox negativeCheckBox;
	}
}
