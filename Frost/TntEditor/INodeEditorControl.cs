using Frost.Tnt;

namespace Frost.TntEditor
{
	/// <summary>
	/// A GUI control that displays the value(s) of a node
	/// </summary>
	public interface INodeEditorControl
	{
		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns></returns>
		Node AsNode ();

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		void FromNode (Node node);
	}
}
