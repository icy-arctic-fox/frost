using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class StringNodeValueControl : UserControl, INodeEditorControl
	{
		public StringNodeValueControl ()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			return new StringNode(textBox.Text);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			textBox.Text = ((StringNode)node).Value;
		}
	}
}
