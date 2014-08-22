using System;
using System.Windows.Forms;
using Frost.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class GuidNodeValueControl : UserControl, INodeEditorControl
	{
		public GuidNodeValueControl()
		{
			InitializeComponent();

			// Links for advancing to the next box
			textBox1.Tag = textBox2;
			textBox2.Tag = textBox3;
			textBox3.Tag = textBox4;
			textBox4.Tag = textBox5;
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			// Force append zeroes to incomplete segments
			var curTextBox = textBox1;
			while(curTextBox != null)
			{
				while(curTextBox.Text.Length < curTextBox.MaxLength)
					curTextBox.Text += '0';
				curTextBox = curTextBox.Tag as TextBox;
			}

			var guidString = String.Join("-", textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);
			var guid       = new Guid(guidString);
			return new GuidNode(guid);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			var segments  = node.StringValue.Split('-');
			textBox1.Text = segments[0];
			textBox2.Text = segments[1];
			textBox3.Text = segments[2];
			textBox4.Text = segments[3];
			textBox5.Text = segments[4];
		}

		private void textBox_TextChanged (object sender, EventArgs e)
		{
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

			if(text.Length == textBox.MaxLength)
			{// Move to next text box
				var nextTextBox = textBox.Tag as TextBox;
				if(nextTextBox != null)
					nextTextBox.Focus();
			}
		}
	}
}
