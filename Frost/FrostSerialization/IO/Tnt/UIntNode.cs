namespace Frost.IO.Tnt
{
	/// <summary>
	/// 32-bit unsigned integer node
	/// </summary>
	public class UIntNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.UInt"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.UInt; }
		}

		/// <summary>
		/// Value stored in the node
		/// </summary>
		public uint Value { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return Value.ToString(System.Globalization.CultureInfo.InvariantCulture); }
		}
		#endregion

		/// <summary>
		/// Creates a new unsigned integer node
		/// </summary>
		/// <param name="value">Value to store in the node</param>
		public UIntNode (uint value)
		{
			Value = value;
		}

		#region Serialization

		/// <summary>
		/// Constructs an unsigned integer node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed unsigned integer node</returns>
		internal static UIntNode ReadPayload (System.IO.BinaryReader br)
		{
			var value = br.ReadUInt32();
			return new UIntNode(value);
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
