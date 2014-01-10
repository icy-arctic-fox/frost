namespace Frost.IO.Tnt
{
	/// <summary>
	/// 64-bit signed integer node
	/// </summary>
	public class LongNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Long"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Long; }
		}

		/// <summary>
		/// Value stored in the node
		/// </summary>
		public long Value { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return Value.ToString(System.Globalization.CultureInfo.InvariantCulture); }
		}
		#endregion

		/// <summary>
		/// Creates a new long node
		/// </summary>
		/// <param name="value">Value to store in the node</param>
		public LongNode (long value)
		{
			Value = value;
		}

		#region Serialization

		/// <summary>
		/// Constructs a long node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed long integer node</returns>
		internal static LongNode ReadPayload (System.IO.BinaryReader br)
		{
			var value = br.ReadInt64();
			return new LongNode(value);
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
