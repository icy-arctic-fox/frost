﻿namespace Frost.Tnt
{
	/// <summary>
	/// 8-bit unsigned integer node
	/// </summary>
	public class ByteNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Byte"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Byte; }
		}

		/// <summary>
		/// Value stored in the node
		/// </summary>
		public byte Value { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return Value.ToString(System.Globalization.CultureInfo.InvariantCulture); }
		}
		#endregion

		/// <summary>
		/// Creates a new byte node
		/// </summary>
		/// <param name="value">Value to store in the node</param>
		public ByteNode (byte value)
		{
			Value = value;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public ByteNode CloneNode ()
		{
			return new ByteNode(Value);
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
		/// Constructs a byte node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed byte node</returns>
		internal static ByteNode ReadPayload (System.IO.BinaryReader br)
		{
			var value = br.ReadByte();
			return new ByteNode(value);
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			bw.Write(Value);
		}
		#endregion
	}
}
