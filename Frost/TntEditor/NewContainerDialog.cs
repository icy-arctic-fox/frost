using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor
{
	public partial class NewContainerDialog : Form
	{
		/// <summary>
		/// Selected container version
		/// </summary>
		public byte ContainerVersion { get; private set; }

		/// <summary>
		/// Node type for the root node
		/// </summary>
		public NodeType RootNodeType { get; private set; }

		public NewContainerDialog()
		{
			// Initial values
			ContainerVersion = 1;
			RootNodeType     = NodeType.Complex;

			InitializeComponent();
			for(var type = NodeType.Boolean; type <= NodeType.Complex; ++type)
				typeCombo.Items.Add(type);

			versionCombo.SelectedIndex = ContainerVersion - 1;
			typeCombo.SelectedIndex    = (int)(RootNodeType - 1);
		}

		#region Event listeners
		private void versionCombo_SelectedIndexChanged (object sender, EventArgs e)
		{
			var combo = (ComboBox)sender;
			ContainerVersion = (byte)(combo.SelectedIndex + 1);
		}

		private void typeCombo_SelectedIndexChanged (object sender, EventArgs e)
		{
			var combo = (ComboBox)sender;
			RootNodeType = (NodeType)(combo.SelectedIndex + 1);
		}
		#endregion
	}
}
