using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class DateTimeNodeValueControl : UserControl, INodeEditorControl
	{
		public DateTimeNodeValueControl ()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			var dt = datePicker.Value.Date;
			dt    += timePicker.Value.TimeOfDay;
			return new DateTimeNode(dt);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			var dt = ((DateTimeNode)node).Value;
			datePicker.Value = dt;
			timePicker.Value = dt;
		}
	}
}
