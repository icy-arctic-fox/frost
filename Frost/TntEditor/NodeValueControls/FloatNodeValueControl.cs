using System;
using System.Windows.Forms;
using Frost.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class FloatNodeValueControl : UserControl, INodeEditorControl
	{
		public FloatNodeValueControl ()
		{
			InitializeComponent();
		}

		private NodeType _type;

		/// <summary>
		/// Type of node that will be produced by <see cref="AsNode"/>
		/// </summary>
		public NodeType NodeType
		{
			get { return _type; }
			set { _type = value; }
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			switch(_type)
			{
			case NodeType.Float:
				float f;
				return new FloatNode(Single.TryParse(textBox.Text, out f) ? f : 0f);
			case NodeType.Double:
				double d;
				return new DoubleNode(Double.TryParse(textBox.Text, out d) ? d : 0d);
			}
			return null;
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			textBox.Text = node.StringValue;
			_type        = node.Type;
		}
	}
}
