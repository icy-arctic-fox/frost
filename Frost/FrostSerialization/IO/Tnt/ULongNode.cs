namespace Frost.IO.Tnt
{
	/// <summary>
	/// 64-bit unsigned integer node
	/// </summary>
	public class ULongNode : Node
	{
		#region Properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.ULong"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.ULong; }
		}

		/// <summary>
		/// Value stored in the node
		/// </summary>
		public ulong Value { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return Value.ToString(System.Globalization.CultureInfo.InvariantCulture); }
		}
		#endregion

		/// <summary>
		/// Creates a new unsigned long node
		/// </summary>
		/// <param name="value">Value to store in the node</param>
		public ULongNode (ulong value)
		{
			Value = value;
		}

		#region Serialization

		/// <summary>
		/// Constructs an unsigned long node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed unsigned long integer node</returns>
		internal static ULongNode ReadPayload (System.IO.BinaryReader br)
		{
			var value = br.ReadUInt64();
			return new ULongNode(value);
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
