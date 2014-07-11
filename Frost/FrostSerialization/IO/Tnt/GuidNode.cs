using System;
using Frost.Utility;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Globally unique identifier node
	/// </summary>
	public class GuidNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Guid"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Guid; }
		}

		/// <summary>
		/// Value stored in the node
		/// </summary>
		public Guid Value { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return Value.ToString(); }
		}
		#endregion

		/// <summary>
		/// Creates a new Guid node
		/// </summary>
		/// <param name="value">Value to store in the node</param>
		public GuidNode (Guid value)
		{
			Value = value;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public GuidNode CloneNode ()
		{
			return new GuidNode(Value);
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public override object Clone ()
		{
			return CloneNode();
		}

		#region Serialization

		/// <summary>
		/// Number of bytes that make up a Guid
		/// </summary>
		private const int GuidSize = 16;

		/// <summary>
		/// Constructs a Guid node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed Guid node</returns>
		internal static GuidNode ReadPayload (System.IO.BinaryReader br)
		{
			var bytes = br.ReadBytes(GuidSize);
			adjustByteOrdering(bytes);
			var guid  = new Guid(bytes);
			return new GuidNode(guid);
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			var bytes = Value.ToByteArray();
			adjustByteOrdering(bytes);
			bw.Write(bytes);
		}

		/// <summary>
		/// Fixes the byte ordering to switch between the .NET GUID format and the standard UUID format
		/// </summary>
		/// <param name="bytes">Array of bytes containing the GUID/UUID</param>
		private static void adjustByteOrdering (byte[] bytes)
		{
			bytes.Reverse(0, 4);
			bytes.Reverse(4, 2);
			bytes.Reverse(6, 2);
		}
		#endregion
	}
}
