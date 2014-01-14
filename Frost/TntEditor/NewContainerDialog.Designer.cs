namespace Frost.TntEditor
{
	partial class NewContainerDialog
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewContainerDialog));
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.versionLabel = new System.Windows.Forms.Label();
			this.typeLabel = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.versionCombo = new System.Windows.Forms.ComboBox();
			this.typeCombo = new System.Windows.Forms.ComboBox();
			this.okButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 3;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.Controls.Add(this.versionLabel, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.typeLabel, 0, 1);
			this.tableLayoutPanel.Controls.Add(this.cancelButton, 1, 2);
			this.tableLayoutPanel.Controls.Add(this.versionCombo, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.typeCombo, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.okButton, 2, 2);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 3;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(222, 88);
			this.tableLayoutPanel.TabIndex = 0;
			// 
			// versionLabel
			// 
			this.versionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.versionLabel.AutoSize = true;
			this.versionLabel.Location = new System.Drawing.Point(3, 7);
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.Size = new System.Drawing.Size(45, 13);
			this.versionLabel.TabIndex = 0;
			this.versionLabel.Text = "Version:";
			// 
			// typeLabel
			// 
			this.typeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.typeLabel.AutoSize = true;
			this.typeLabel.Location = new System.Drawing.Point(3, 34);
			this.typeLabel.Name = "typeLabel";
			this.typeLabel.Size = new System.Drawing.Size(60, 13);
			this.typeLabel.TabIndex = 1;
			this.typeLabel.Text = "Root Type:";
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(69, 62);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(69, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// versionCombo
			// 
			this.versionCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel.SetColumnSpan(this.versionCombo, 2);
			this.versionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.versionCombo.Enabled = false;
			this.versionCombo.FormattingEnabled = true;
			this.versionCombo.Items.AddRange(new object[] {
            "Version 1"});
			this.versionCombo.Location = new System.Drawing.Point(69, 3);
			this.versionCombo.Name = "versionCombo";
			this.versionCombo.Size = new System.Drawing.Size(150, 21);
			this.versionCombo.TabIndex = 3;
			this.versionCombo.SelectedIndexChanged += new System.EventHandler(this.versionCombo_SelectedIndexChanged);
			// 
			// typeCombo
			// 
			this.typeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel.SetColumnSpan(this.typeCombo, 2);
			this.typeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.typeCombo.FormattingEnabled = true;
			this.typeCombo.Location = new System.Drawing.Point(69, 30);
			this.typeCombo.Name = "typeCombo";
			this.typeCombo.Size = new System.Drawing.Size(150, 21);
			this.typeCombo.TabIndex = 4;
			this.typeCombo.SelectedIndexChanged += new System.EventHandler(this.typeCombo_SelectedIndexChanged);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(144, 62);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 5;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// NewDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(222, 88);
			this.Controls.Add(this.tableLayoutPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Node Container";
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label versionLabel;
		private System.Windows.Forms.Label typeLabel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ComboBox versionCombo;
		private System.Windows.Forms.ComboBox typeCombo;
		private System.Windows.Forms.Button okButton;
	}
}