using System;
using System.Drawing;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor
{
	internal partial class NodeInfoPanel : UserControl
	{
		public NodeInfoPanel ()
		{
			InitializeComponent();
			typeCombo.Items.Add(String.Empty);
			for(var type = NodeType.Boolean; type <= NodeType.Complex; ++type)
				typeCombo.Items.Add(type);
		}

		/// <summary>
		/// Sets the node that has information displayed about it
		/// </summary>
		/// <param name="info">Information about the node to display</param>
		internal void SetDisplayNode (NodeInfo info)
		{
			if(info != null)
				updateControls(info);
			else
				blankControls();
		}

		/// <summary>
		/// Updates the contents of the displayed controls
		/// </summary>
		private void updateControls (NodeInfo info)
		{
			nameText.Text = info.Name;
			pathText.Text = info.Path;
			var type      = info.Node.Type;
			var index     = (int)type;
			typeCombo.SelectedIndex = index;
			valueBox.Text = info.Node.StringValue;
		}

		/// <summary>
		/// Blanks out the contents of the displayed controls
		/// </summary>
		private void blankControls ()
		{
			nameText.Text = String.Empty;
			pathText.Text = String.Empty;
			typeCombo.SelectedIndex = 0;
			valueBox.Text = String.Empty;
		}

		#region Event listeners
		private void typeCombo_SelectedIndexChanged (object sender, EventArgs e)
		{
			var combo = (ComboBox)sender;
			var index = combo.SelectedIndex;
			Image img;
			if(index == 0)
			{// No type
				img = null;
				applyButton.Enabled = false;
			}
			else
			{// Type specified
				img = nodeTypeImageList.Images[index];
				applyButton.Enabled = true;
			}
			typePicture.Image = img;
		}
		#endregion
	}
}
