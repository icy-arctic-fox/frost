using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class BooleanNodeValueControl : UserControl, INodeEditorControl
	{
		public BooleanNodeValueControl ()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Boolean value of the control
		/// </summary>
		public bool Value
		{
			get { return comboBox.SelectedIndex > 0; }
			set { comboBox.SelectedIndex = value ? 1 : 0; }
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			return new BooleanNode(Value);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			Value = ((BooleanNode)node).Value;
		}
	}
}
