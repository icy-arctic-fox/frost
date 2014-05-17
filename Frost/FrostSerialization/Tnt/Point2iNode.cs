using System;

namespace Frost.Tnt
{
	/// <summary>
	/// X and Y node.
	/// Contains X and Y integer values for sizes or locations.
	/// </summary>
	public class Point2iNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Point2i"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Point2i; }
		}

		/// <summary>
		/// X position
		/// </summary>
		public int X { get; set; }

		/// <summary>
		/// Y position
		/// </summary>
		public int Y { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return String.Format("({0}, {1})", X, Y); }
		}
		#endregion

		/// <summary>
		/// Creates a new 2D point node
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		public Point2iNode (int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public Point2iNode CloneNode ()
		{
			return new Point2iNode(X, Y);
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
		/// Constructs a 2D point node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed 2D point node</returns>
		internal static Point2iNode ReadPayload (System.IO.BinaryReader br)
		{
			var x = br.ReadInt32();
			var y = br.ReadInt32();
			return new Point2iNode(x, y);
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			bw.Write(X);
			bw.Write(Y);
		}
		#endregion
	}
}
