namespace Frost.IO.Tnt
{
	/// <summary>
	/// True or false value node
	/// </summary>
	public class BooleanNode : Node
	{
		#region Properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Boolean"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Boolean; }
		}

		/// <summary>
		/// Value stored in the node
		/// </summary>
		public bool Value { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return Value.ToString(System.Globalization.CultureInfo.InvariantCulture); }
		}
		#endregion

		/// <summary>
		/// Creates a new boolean node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="value">Value to store in the node</param>
		public BooleanNode(string name, bool value)
			: base(name)
		{
			Value = value;
		}

		#region Serialization

		/// <summary>
		/// Constructs a boolean node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <param name="name">Name to give the new node</param>
		/// <returns>A constructed boolean node</returns>
		internal static BooleanNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			var value = br.ReadBoolean();
			return new BooleanNode(name, value);
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
