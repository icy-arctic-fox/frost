using System;
using System.Windows.Forms;
using Frost.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class Point2iNodeValueControl : UserControl, INodeEditorControl
	{
		public Point2iNodeValueControl ()
		{
			InitializeComponent();
			xNumericUpDown.Minimum = Int32.MinValue;
			xNumericUpDown.Maximum = Int32.MaxValue;
			yNumericUpDown.Minimum = Int32.MinValue;
			yNumericUpDown.Maximum = Int32.MaxValue;
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			var x = (int)xNumericUpDown.Value;
			var y = (int)yNumericUpDown.Value;
			return new Point2iNode(x, y);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			var point = (Point2iNode)node;
			xNumericUpDown.Value = point.X;
			yNumericUpDown.Value = point.Y;
		}
	}
}
