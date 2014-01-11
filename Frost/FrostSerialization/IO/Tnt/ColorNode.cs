using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Color node
	/// </summary>
	public class ColorNode : Node
	{
		#region Node properties

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
		/// <param name="argb">Combined values of the color components arranged as ARGB</param>
		public ColorNode (int argb)
		{
			Argb = argb;
		}

		/// <summary>
		/// Creates a new color node
		/// </summary>
		/// <param name="red">Amount of the red component</param>
		/// <param name="green">Amount of the green component</param>
		/// <param name="blue">Amount of the blue component</param>
		/// <param name="alpha">Amount of the alpha component</param>
		public ColorNode (byte red, byte green, byte blue, byte alpha = 255)
		{
			Argb = (alpha << 24) | (red << 16) | (green << 8) | blue;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public ColorNode CloneNode ()
		{
			return new ColorNode(Argb);
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
		/// Constructs a color node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed color node</returns>
		internal static ColorNode ReadPayload (System.IO.BinaryReader br)
		{
			var argb = br.ReadInt32();
			return new ColorNode(argb);
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
