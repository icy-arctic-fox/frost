using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Color node
	/// </summary>
	public class ColorNode : Node
	{
		#region Properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Color"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Color; }
		}

		/// <summary>
		/// Amount of red
		/// </summary>
		public byte Red
		{
			get { return (byte)((Argb >> 16) & 0xff); }
			set { Argb = (value << 16) | (Argb & unchecked((int)0xff00ffff)); }
		}

		/// <summary>
		/// Amount of green
		/// </summary>
		public byte Green
		{
			get { return (byte)((Argb >> 8) & 0xff); }
			set { Argb = (value << 8) | (Argb & unchecked((int)0xffff00ff)); }
		}

		/// <summary>
		/// Amount of blue
		/// </summary>
		public byte Blue
		{
			get { return (byte)(Argb & 0xff); }
			set { Argb = value | (Argb & unchecked((int)0xffffff00)); }
		}

		/// <summary>
		/// Amount of alpha
		/// </summary>
		public byte Alpha
		{
			get { return (byte)((Argb >> 24) & 0xff); }
			set { Argb = (value << 24) | (Argb & 0x00ffffff); }
		}

		/// <summary>
		/// Color value represented as an integer.
		/// The color components are arranged as alpha, red, green, and blue.
		/// </summary>
		public int Argb { get; set; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get
			{
				var rgb   = Argb & 0x00ffffff;
				var alpha = ((Argb >> 24) & 0xff) / 255d;
				return String.Format("({0:X6}, {1:P})", rgb, alpha);
			}
		}
		#endregion

		/// <summary>
		/// Creates a new color node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="argb">Combined values of the color components arranged as ARGB</param>
		public ColorNode (string name, int argb)
			: base(name)
		{
			Argb = argb;
		}

		/// <summary>
		/// Creates a new color node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="red">Amount of the red component</param>
		/// <param name="green">Amount of the green component</param>
		/// <param name="blue">Amount of the blue component</param>
		/// <param name="alpha">Amount of the alpha component</param>
		public ColorNode (string name, byte red, byte green, byte blue, byte alpha = 255)
			: base(name)
		{
			Argb = (alpha << 24) | (red << 16) | (green << 8) | blue;
		}

		#region Serialization

		/// <summary>
		/// Constructs a color node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <param name="name">Name to give the new node</param>
		/// <returns>A constructed color node</returns>
		internal static ColorNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			var argb = br.ReadInt32();
			return new ColorNode(name, argb);
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			bw.Write(Argb);
		}
		#endregion
	}
}
