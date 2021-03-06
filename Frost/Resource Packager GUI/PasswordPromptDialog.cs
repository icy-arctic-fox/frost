﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Frost.ResourcePackagerGui
{
	public partial class PasswordPromptDialog : Form
	{
		public string Password
		{
			get { return passwordTextBox.Text; }
		}

		public PasswordPromptDialog ()
		{
			InitializeComponent();
		}

		private void okButton_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void cancelButton_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
