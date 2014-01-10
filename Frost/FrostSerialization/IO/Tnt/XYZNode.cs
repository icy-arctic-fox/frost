using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// X, Y, and Z node.
	/// Contains X, Y, and Z integer values for sizes or locations.
	/// </summary>
	public class XyzNode : Node
	{
		#region Properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Xyz"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Xyz; }
		}

		/// <summary>
		/// X value stored in the node
		/// </summary>
		public int X { get; set; }

		/// <summary>
		/// Y value stored in the node
		/// </summary>
		public int Y { get; set; }

		/// <summary>
		/// Z value stored in the node
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
		/// Creates a new x, y, and z node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="x">X value to store in the node</param>
		/// <param name="y">Y value to store in the node</param>
		/// <param name="z">Z value to store in the node</param>
		public XyzNode (string name, int x, int y, int z)
			: base(name)
		{
			X = x;
			Y = y;
			Z = z;
		}

		#region Serialization

		/// <summary>
		/// Constructs an x, y, and z node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <param name="name">Name to give the new node</param>
		/// <returns>A constructed x, y, and z node</returns>
		internal static XyzNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			var x = br.ReadInt32();
			var y = br.ReadInt32();
			var z = br.ReadInt32();
			return new XyzNode(name, x, y, z);
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
