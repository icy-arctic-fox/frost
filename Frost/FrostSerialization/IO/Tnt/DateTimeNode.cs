using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Date and time node
	/// </summary>
	public class DateTimeNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.DateTime"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.DateTime; }
		}

		/// <summary>
		/// Value stored in the node
		/// </summary>
		public DateTime Value { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return Value.ToString(System.Globalization.CultureInfo.InvariantCulture); }
		}
		#endregion

		/// <summary>
		/// Creates a new date and time node
		/// </summary>
		/// <param name="value">Value to store in the node</param>
		public DateTimeNode (DateTime value)
		{
			Value = value;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public DateTimeNode CloneNode ()
		{
			return new DateTimeNode(Value);
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
		/// Constructs a date and time node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed date and time node</returns>
		internal static DateTimeNode ReadPayload (System.IO.BinaryReader br)
		{
			var data = br.ReadInt64();
			var dt   = DateTime.FromBinary(data);
			return new DateTimeNode(dt);
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			bw.Write(Value.ToBinary());
		}
		#endregion
	}
}
