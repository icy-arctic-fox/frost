using System;

namespace Frost.Tnt
{
	/// <summary>
	/// X, Y, and Z node.
	/// Contains X, Y, and Z integer values for sizes or locations.
	/// </summary>
	public class Point3iNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Point3i"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Point3i; }
		}

		/// <summary>
		/// X position or size
		/// </summary>
		public int X { get; set; }

		/// <summary>
		/// Y position or size
		/// </summary>
		public int Y { get; set; }

		/// <summary>
		/// Z position or size
		/// </summary>
		public int Z { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return String.Format("({0}, {1}, {2})", X, Y, Z); }
		}
		#endregion

		/// <summary>
		/// Creates a new 3D point node
		/// </summary>
		/// <param name="x">X position or size</param>
		/// <param name="y">Y position or size</param>
		/// <param name="z">Z position or size</param>
		public Point3iNode (int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public Point3iNode CloneNode ()
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
		/// Constructs a 3D point node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed 3D point node</returns>
		internal static Point3iNode ReadPayload (System.IO.BinaryReader br)
		{
			var x = br.ReadInt32();
			var y = br.ReadInt32();
			var z = br.ReadInt32();
			return new Point3iNode(x, y, z);
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			bw.Write(X);
			bw.Write(Y);
			bw.Write(Z);
		}
		#endregion
	}
}
