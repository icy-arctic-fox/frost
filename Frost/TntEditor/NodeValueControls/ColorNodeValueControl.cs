using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Frost.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class ColorNodeValueControl : UserControl, INodeEditorControl
	{
		public ColorNodeValueControl ()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			int rgb;
			if(!Int32.TryParse(colorTextBox.Text, NumberStyles.AllowHexSpecifier, null, out rgb))
				rgb = 0; // Default to black
			var alpha = (byte)alphaNumericUpDown.Value;
			var argb  = (alpha << 24) | rgb;
			return new ColorNode(argb);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			var color = (ColorNode)node;
			colorBox.BackColor = Color.FromArgb(color.Red, color.Green, color.Blue);
			colorTextBox.Text  = String.Format("{0:X6}", color.Argb & 0x00ffffff);
			alphaNumericUpDown.Value = color.Alpha;
		}

		private void colorTextBox_TextChanged (object sender, EventArgs e)
		{
			// Prevent anything other than 0-9, a-f, or A-F being typed
			var textBox = (TextBox)sender;
			var text    = textBox.Text;
			for(var i = 0; i < text.Length; ++i)
			{
				var c = text[i];
				var valid = (c >= '0' && c <= '9') ||
							(c >= 'a' && c <= 'f') ||
							(c >= 'A' && c <= 'F');
				if(!valid)
				{
					text = text.Remove(i, 1);
					--i;
				}
			}
			textBox.Text = text;
			textBox.SelectionStart = text.Length;

			// Update the color preview
			int rgb;
			if(!Int32.TryParse(textBox.Text, NumberStyles.AllowHexSpecifier, null, out rgb))
				rgb = 0; // Default to black
			var argb = (Byte.MaxValue << 24) | rgb;
			colorBox.BackColor = Color.FromArgb(argb);
		}

		private void selectButton_Click (object sender, EventArgs e)
		{
			int rgb;
			if(!Int32.TryParse(colorTextBox.Text, NumberStyles.AllowHexSpecifier, null, out rgb))
				rgb = 0; // Default to black
			var argb = (Byte.MaxValue << 24) | rgb;
			colorDialog.Color = Color.FromArgb(argb);

			if(colorDialog.ShowDialog() == DialogResult.OK)
			{
				argb = colorDialog.Color.ToArgb();
				colorTextBox.Text = String.Format("{0:X6}", argb & 0x00ffffff);
				alphaNumericUpDown.Value = (argb >> 24) & 0x000000ff;
			}
		}
	}
}
