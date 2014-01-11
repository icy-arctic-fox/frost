using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Contains multiple nodes of different types and addresses them by name
	/// </summary>
	public class ComplexNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Complex"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Complex; }
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
		/// Creates a new empty complex node
		/// </summary>
		public ComplexNode ()
		{
			// ...
		}

		#region Serialization

		/// <summary>
		/// Constructs a complex node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed complex node</returns>
		internal static ComplexNode ReadPayload (System.IO.BinaryReader br)
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
