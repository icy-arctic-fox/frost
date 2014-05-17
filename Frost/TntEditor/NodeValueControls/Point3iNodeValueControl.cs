using System;
using System.Windows.Forms;
using Frost.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class Point3iNodeValueControl : UserControl, INodeEditorControl
	{
		public Point3iNodeValueControl ()
		{
			InitializeComponent();
			xNumericUpDown.Minimum = Int32.MinValue;
			xNumericUpDown.Maximum = Int32.MaxValue;
			yNumericUpDown.Minimum = Int32.MinValue;
			yNumericUpDown.Maximum = Int32.MaxValue;
			zNumericUpDown.Minimum = Int32.MinValue;
			zNumericUpDown.Maximum = Int32.MaxValue;
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			var x = (int)xNumericUpDown.Value;
			var y = (int)yNumericUpDown.Value;
			var z = (int)zNumericUpDown.Value;
			return new Point3iNode(x, y, z);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			var xyz = (Point3iNode)node;
			xNumericUpDown.Value = xyz.X;
			yNumericUpDown.Value = xyz.Y;
			zNumericUpDown.Value = xyz.Z;
		}
	}
}
