namespace Frost.TntEditor.NodeValueControls
{
	partial class DateTimeNodeValueControl
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
			this.timeLabel = new System.Windows.Forms.Label();
			this.dateLabel = new System.Windows.Forms.Label();
			this.datePicker = new System.Windows.Forms.DateTimePicker();
			this.timePicker = new System.Windows.Forms.DateTimePicker();
			this.tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Controls.Add(this.timeLabel, 0, 1);
			this.tableLayoutPanel.Controls.Add(this.dateLabel, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.datePicker, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.timePicker, 1, 1);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 2;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(246, 54);
			this.tableLayoutPanel.TabIndex = 1;
			// 
			// timeLabel
			// 
			this.timeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.timeLabel.AutoSize = true;
			this.timeLabel.Location = new System.Drawing.Point(3, 33);
			this.timeLabel.Name = "timeLabel";
			this.timeLabel.Size = new System.Drawing.Size(33, 13);
			this.timeLabel.TabIndex = 4;
			this.timeLabel.Text = "Time:";
			// 
			// dateLabel
			// 
			this.dateLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.dateLabel.AutoSize = true;
			this.dateLabel.Location = new System.Drawing.Point(3, 6);
			this.dateLabel.Name = "dateLabel";
			this.dateLabel.Size = new System.Drawing.Size(33, 13);
			this.dateLabel.TabIndex = 1;
			this.dateLabel.Text = "Date:";
			// 
			// datePicker
			// 
			this.datePicker.Dock = System.Windows.Forms.DockStyle.Fill;
			this.datePicker.Location = new System.Drawing.Point(42, 3);
			this.datePicker.Name = "datePicker";
			this.datePicker.Size = new System.Drawing.Size(201, 20);
			this.datePicker.TabIndex = 2;
			// 
			// timePicker
			// 
			this.timePicker.Dock = System.Windows.Forms.DockStyle.Fill;
			this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.timePicker.Location = new System.Drawing.Point(42, 29);
			this.timePicker.Name = "timePicker";
			this.timePicker.ShowUpDown = true;
			this.timePicker.Size = new System.Drawing.Size(201, 20);
			this.timePicker.TabIndex = 3;
			// 
			// DateTimeNodeValueControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "DateTimeNodeValueControl";
			this.Size = new System.Drawing.Size(246, 54);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label dateLabel;
		private System.Windows.Forms.DateTimePicker datePicker;
		private System.Windows.Forms.Label timeLabel;
		private System.Windows.Forms.DateTimePicker timePicker;
	}
}
