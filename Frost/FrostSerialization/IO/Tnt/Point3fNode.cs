using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// 3D point node with floating-point values
	/// </summary>
	public class Point3fNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Point3f"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Point3f; }
		}

		/// <summary>
		/// X-coordinate
		/// </summary>
		public float X { get; set; }

		/// <summary>
		/// Y-coordinate
		/// </summary>
		public float Y { get; set; }

		/// <summary>
		/// Z-coordinate
		/// </summary>
		public float Z { get; set; }

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
		/// <param name="x">X-coordinate</param>
		/// <param name="y">Y-coordinate</param>
		/// <param name="z">Z-coordinate</param>
		public Point3fNode (float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public Point3fNode CloneNode ()
		{
			return new Point3fNode(X, Y, Z);
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
		internal static Point3fNode ReadPayload (System.IO.BinaryReader br)
		{
			var x = br.ReadSingle();
			var y = br.ReadSingle();
			var z = br.ReadSingle();
			return new Point3fNode(x, y, z);
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
