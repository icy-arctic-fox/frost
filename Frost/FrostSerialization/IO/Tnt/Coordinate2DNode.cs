using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// 2D coordinate node
	/// </summary>
	public class Coordinate2DNode : Node
	{
		#region Properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Coordinate2D"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Coordinate2D; }
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
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return String.Format("({0}, {1})", X, Y); }
		}
		#endregion

		/// <summary>
		/// Creates a new 2D coordinate node
		/// </summary>
		/// <param name="x">X value to store in the node</param>
		/// <param name="y">Y value to store in the node</param>
		public Coordinate2DNode (float x, float y)
		{
			X = x;
			Y = y;
		}

		#region Serialization

		/// <summary>
		/// Constructs a 2D coordinate node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed 2D coordinate node</returns>
		internal static Coordinate2DNode ReadPayload (System.IO.BinaryReader br)
		{
			var x = br.ReadSingle();
			var y = br.ReadSingle();
			return new Coordinate2DNode(x, y);
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
