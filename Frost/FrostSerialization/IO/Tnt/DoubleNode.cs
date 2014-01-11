namespace Frost.IO.Tnt
{
	/// <summary>
	/// 64-bit double-precision floating-point node
	/// </summary>
	public class DoubleNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Double"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Double; }
		}

		/// <summary>
		/// Value stored in the node
		/// </summary>
		public double Value { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return Value.ToString(System.Globalization.CultureInfo.InvariantCulture); }
		}
		#endregion

		/// <summary>
		/// Creates a new double-precision floating-point node
		/// </summary>
		/// <param name="value">Value to store in the node</param>
		public DoubleNode (double value)
		{
			Value = value;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public DoubleNode CloneNode ()
		{
			return new DoubleNode(Value);
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
		/// Constructs a double-precision floating-point node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed double-precision floating-point node</returns>
		internal static DoubleNode ReadPayload (System.IO.BinaryReader br)
		{
			var value = br.ReadDouble();
			return new DoubleNode(value);
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
