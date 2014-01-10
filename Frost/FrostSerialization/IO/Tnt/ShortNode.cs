namespace Frost.IO.Tnt
{
	/// <summary>
	/// 16-bit signed integer node
	/// </summary>
	public class ShortNode : Node
	{
		#region Properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Short"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Short; }
		}

		/// <summary>
		/// Value stored in the node
		/// </summary>
		public short Value { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return Value.ToString(System.Globalization.CultureInfo.InvariantCulture); }
		}
		#endregion

		/// <summary>
		/// Creates a new short node
		/// </summary>
		/// <param name="value">Value to store in the node</param>
		public ShortNode (short value)
		{
			Value = value;
		}

		#region Serialization

		/// <summary>
		/// Constructs a short node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed short node</returns>
		internal static ShortNode ReadPayload (System.IO.BinaryReader br)
		{
			var value = br.ReadInt16();
			return new ShortNode(value);
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
