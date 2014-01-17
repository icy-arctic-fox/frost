using System;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class Coordinate3DNodeValueControl : UserControl, INodeEditorControl
	{
		public Coordinate3DNodeValueControl ()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			float x, y, z;
			if(!Single.TryParse(xTextBox.Text, out x))
				x = 0f;
			if(!Single.TryParse(yTextBox.Text, out y))
				y = 0f;
			if(!Single.TryParse(zTextBox.Text, out z))
				z = 0f;
			return new Coordinate3DNode(x, y, z);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			var coord = (Coordinate3DNode)node;
			xTextBox.Text = coord.X.ToString();
			yTextBox.Text = coord.Y.ToString();
			zTextBox.Text = coord.Z.ToString();
		}
	}
}
