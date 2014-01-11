using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// 3D coordinate node
	/// </summary>
	public class Coordinate3DNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Coordinate3D"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Coordinate3D; }
		}

		/// <summary>
		/// X value stored in the node
		/// </summary>
		public float X { get; set; }

		/// <summary>
		/// Y value stored in the node
		/// </summary>
		public float Y { get; set; }

		/// <summary>
		/// Z value stored in the node
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
		/// Creates a new 3D coordinate node
		/// </summary>
		/// <param name="x">X value to store in the node</param>
		/// <param name="y">Y value to store in the node</param>
		/// <param name="z">Z value to store in the node</param>
		public Coordinate3DNode (float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public Coordinate3DNode CloneNode ()
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
		/// Constructs a 3D coordinate node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed 3D coordinate node</returns>
		internal static Coordinate3DNode ReadPayload (System.IO.BinaryReader br)
		{
			var x = br.ReadSingle();
			var y = br.ReadSingle();
			var z = br.ReadSingle();
			return new Coordinate3DNode(x, y, z);
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
