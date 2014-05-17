using System.Windows.Forms;
using Frost.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class ListNodeValueControl : UserControl, INodeEditorControl
	{
		public ListNodeValueControl ()
		{
			InitializeComponent();
			for(var type = NodeType.Boolean; type <= NodeType.Complex; ++type)
				comboBox.Items.Add(type);
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			var type = (NodeType)(comboBox.SelectedIndex + 1);
			return new ListNode(type);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			var list = (ListNode)node;
			comboBox.SelectedIndex = (int)list.ElementType - 1;
		}

		private void comboBox_SelectedIndexChanged (object sender, System.EventArgs e)
		{
			var index = ((ComboBox)sender).SelectedIndex + 1;
			typePicture.Image = nodeTypeImageList.Images[index];
		}
	}
}
