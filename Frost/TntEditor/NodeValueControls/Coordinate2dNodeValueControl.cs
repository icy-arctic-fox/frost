using System;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class Coordinate2DNodeValueControl : UserControl, INodeEditorControl
	{
		public Coordinate2DNodeValueControl ()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			float x, y;
			if(!Single.TryParse(xTextBox.Text, out x))
				x = 0f;
			if(!Single.TryParse(yTextBox.Text, out y))
				y = 0f;
			return new Coordinate2DNode(x, y);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			var coord = (Coordinate2DNode)node;
			xTextBox.Text = coord.X.ToString();
			yTextBox.Text = coord.Y.ToString();
		}
	}
}
