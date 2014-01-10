using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Node that contains multiple nodes of the same type
	/// </summary>
	public class ListNode : Node
	{
		#region Properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.List"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.List; }
		}

		/// <summary>
		/// Node type for each of the contained nodes
		/// </summary>
		public NodeType ElementType
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { throw new NotImplementedException(); }
		}
		#endregion

		/// <summary>
		/// Creates a new list node
		/// </summary>
		/// <param name="name">Name of the node</param>
		public ListNode (string name)
			: base(name)
		{
			throw new NotImplementedException();
		}

		#region Serialization

		/// <summary>
		/// Constructs a list node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <param name="name">Name to give the new node</param>
		/// <returns>A constructed list node</returns>
		internal static ListNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
