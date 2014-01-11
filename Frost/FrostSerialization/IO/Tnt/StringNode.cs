using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// String of text node
	/// </summary>
	public class StringNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.String"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.String; }
		}

		private string _value;

		/// <summary>
		/// Value stored in the node
		/// </summary>
		/// <remarks>Null strings are automatically converted to empty strings.</remarks>
		public string Value
		{
			get { return _value; }
			set { _value = value ?? String.Empty; }
		}

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return String.Format("\"{0}\"", _value); }
		}
		#endregion

		/// <summary>
		/// Creates a new string node
		/// </summary>
		/// <param name="value">Value to store in the node</param>
		/// <remarks>A null string for <paramref name="value"/> will be converted to an empty string.</remarks>
		public StringNode (string value)
		{
			Value = value;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public StringNode CloneNode ()
		{
			throw new NotImplementedException();
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
		/// Constructs a string node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed string node</returns>
		internal static StringNode ReadPayload (System.IO.BinaryReader br)
		{
			var value = br.ReadString();
			return new StringNode(value);
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
