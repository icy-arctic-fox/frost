using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Length of time node
	/// </summary>
	public class TimeSpanNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.TimeSpan"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.TimeSpan; }
		}

		/// <summary>
		/// Value stored in the node
		/// </summary>
		public TimeSpan Value { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return Value.ToString(); }
		}
		#endregion

		/// <summary>
		/// Creates a new time span node
		/// </summary>
		/// <param name="value">Value to store in the node</param>
		public TimeSpanNode (TimeSpan value)
		{
			Value = value;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public TimeSpanNode CloneNode ()
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
		/// Constructs a time span node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed time span node</returns>
		internal static TimeSpanNode ReadPayload (System.IO.BinaryReader br)
		{
			var ticks = br.ReadInt64();
			var span  = new TimeSpan(ticks);
			return new TimeSpanNode(span);
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			bw.Write(Value.Ticks);
		}
		#endregion
	}
}
