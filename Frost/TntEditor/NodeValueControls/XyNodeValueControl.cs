using System;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class XyNodeValueControl : UserControl, INodeEditorControl
	{
		public XyNodeValueControl ()
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
			return new XyNode(x, y);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			var xy = (XyNode)node;
			xNumericUpDown.Value = xy.X;
			yNumericUpDown.Value = xy.Y;
		}
	}
}
