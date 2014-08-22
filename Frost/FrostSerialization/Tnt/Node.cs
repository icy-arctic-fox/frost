using System;
using System.IO;
using System.Text;

namespace Frost.Tnt
{
	/// <summary>
	/// Base class for all node types
	/// </summary>
	public abstract class Node : ICloneable
	{
		/// <summary>
		/// Character that separates node names and indices in paths
		/// </summary>
		protected const string TraversalPathSeperator = "/";

		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		public abstract NodeType Type { get; }

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public abstract string StringValue { get; }
		#endregion

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public abstract object Clone ();

		#region Serialization

		/// <summary>
		/// Describes a method that accepts a stream reader and node name and produces a node
		/// </summary>
		/// <param name="br">Stream reader to use to pull data from the stream</param>
		/// <returns>A constructed node</returns>
		protected delegate Node NodeConstructor (BinaryReader br);

		/// <summary>
		/// Reads a node from a stream
		/// </summary>
		/// <param name="br">Reader used to pull data from the stream</param>
		/// <returns>A node read from the stream or null if an "End" node was read</returns>
		/// <exception cref="ArgumentNullException">The reader (<paramref name="br"/>) used to pull data from the stream can't be null.</exception>
		/// <exception cref="InvalidDataException">Thrown if the data in the stream is in an unexpected format</exception>
		internal static Node ReadFromStream (BinaryReader br)
		{
			if(br == null)
				throw new ArgumentNullException("br");

			var type = readHeader(br);
			if(type == NodeType.End)
				return null; // End node
			// else - Regular node
			try
			{
				var reader = GetPayloadReader(type);
				return reader(br);
			}
			catch(NotSupportedException e)
			{
				throw new InvalidDataException("The data in the stream is in an unrecognized format.", e);
			}
		}

		/// <summary>
		/// Reads the header of a node
		/// </summary>
		/// <param name="br">Reader used to pull data from the stream</param>
		/// <returns>Type of node read from the stream</returns>
		private static NodeType readHeader (BinaryReader br)
		{
			var type = (NodeType)br.ReadByte();
			return type;
		}

		/// <summary>
		/// Returns a method that will construct the node
		/// </summary>
		/// <param name="type">Node type</param>
		/// <returns>A node constructor method</returns>
		/// <exception cref="NotSupportedException">Thrown if the type specified by <paramref name="type"/> is not known</exception>
		protected static NodeConstructor GetPayloadReader (NodeType type)
		{
			switch(type)
			{
			case NodeType.Boolean:
				return BooleanNode.ReadPayload;
			case NodeType.Byte:
				return ByteNode.ReadPayload;
			case NodeType.SByte:
				return SByteNode.ReadPayload;
			case NodeType.Short:
				return ShortNode.ReadPayload;
			case NodeType.UShort:
				return UShortNode.ReadPayload;
			case NodeType.Int:
				return IntNode.ReadPayload;
			case NodeType.UInt:
				return UIntNode.ReadPayload;
			case NodeType.Long:
				return LongNode.ReadPayload;
			case NodeType.ULong:
				return ULongNode.ReadPayload;
			case NodeType.Float:
				return FloatNode.ReadPayload;
			case NodeType.Double:
				return DoubleNode.ReadPayload;
			case NodeType.String:
				return StringNode.ReadPayload;
			case NodeType.Guid:
				return GuidNode.ReadPayload;
			case NodeType.DateTime:
				return DateTimeNode.ReadPayload;
			case NodeType.TimeSpan:
				return TimeSpanNode.ReadPayload;
			case NodeType.Blob:
				return BlobNode.ReadPayload;
			case NodeType.Point2i:
				return Point2iNode.ReadPayload;
			case NodeType.Point3i:
				return Point3iNode.ReadPayload;
			case NodeType.Point2f:
				return Point2fNode.ReadPayload;
			case NodeType.Point3f:
				return Point3fNode.ReadPayload;
			case NodeType.Color:
				return ColorNode.ReadPayload;
			case NodeType.List:
				return ListNode.ReadPayload;
			case NodeType.Complex:
				return ComplexNode.ReadPayload;
			default:
				throw new NotSupportedException("Unknown node type " + type);
			}
		}

		/// <summary>
		/// Writes the node (header and payload) to a stream
		/// </summary>
		/// <param name="bw">Writer used to put data on the stream</param>
		/// <exception cref="ArgumentNullException">The writer (<paramref name="bw"/>) used to put data on the stream can't be null.</exception>
		internal void WriteToStream (BinaryWriter bw)
		{
			if(bw == null)
				throw new ArgumentNullException("bw");

			writeHeader(bw);
			WritePayload(bw);
		}

		/// <summary>
		/// Writes just the node's header to a stream
		/// </summary>
		/// <param name="bw">Writer used to put data on the stream</param>
		private void writeHeader (BinaryWriter bw)
		{
			bw.Write((byte)Type);
		}

		/// <summary>
		/// Writes just the payload section of a node to a stream
		/// </summary>
		/// <param name="bw">Writer used to put data on the stream</param>
		internal abstract void WritePayload (BinaryWriter bw);
		#endregion
		
		/// <summary>
		/// Creates a default node for a type
		/// </summary>
		/// <param name="type">Type of node to create</param>
		/// <returns>A new node</returns>
		/// <exception cref="NotSupportedException">Thrown if <paramref name="type"/> refers to an unknown node type</exception>
		/// <remarks>If <paramref name="type"/> is <see cref="NodeType.List"/>, then its element type will be <see cref="NodeType.Complex"/>.</remarks>
		public static Node CreateDefaultNode (NodeType type)
		{
			switch(type)
			{
			case NodeType.Boolean:
				return new BooleanNode(default(bool));
			case NodeType.Byte:
				return new ByteNode(default(byte));
			case NodeType.SByte:
				return new SByteNode(default(sbyte));
			case NodeType.Short:
				return new ShortNode(default(short));
			case NodeType.UShort:
				return new UShortNode(default(ushort));
			case NodeType.Int:
				return new IntNode(default(int));
			case NodeType.UInt:
				return new UIntNode(default(uint));
			case NodeType.Long:
				return new LongNode(default(long));
			case NodeType.ULong:
				return new ULongNode(default(ulong));
			case NodeType.Float:
				return new FloatNode(default(float));
			case NodeType.Double:
				return new DoubleNode(default(double));
			case NodeType.String:
				return new StringNode(String.Empty);
			case NodeType.Guid:
				return new GuidNode(Guid.Empty);
			case NodeType.DateTime:
				return new DateTimeNode(DateTime.Now);
			case NodeType.TimeSpan:
				return new TimeSpanNode(TimeSpan.Zero);
			case NodeType.Blob:
				return new BlobNode(new byte[0]);
			case NodeType.Point2i:
				return new Point2iNode(default(int), default(int));
			case NodeType.Point3i:
				return new Point3iNode(default(int), default(int), default(int));
			case NodeType.Point2f:
				return new Point2fNode(default(float), default(float));
			case NodeType.Point3f:
				return new Point3fNode(default(float), default(float), default(float));
			case NodeType.Color:
				return new ColorNode(default(int));
			case NodeType.List:
				return new ListNode(NodeType.Complex);
			case NodeType.Complex:
				return new ComplexNode();
			default:
				throw new NotSupportedException("Unknown node type");
			}
		}

		/// <summary>
		/// Generates a string representation of the node
		/// </summary>
		/// <returns>String structure of the node</returns>
		public override string ToString ()
		{
			var sb = new StringBuilder();
			ToString(sb, 0);
			return sb.ToString().Trim();
		}

		/// <summary>
		/// Character used to indicate an increase in depth
		/// </summary>
		protected const char IndentCharacter = ' ';

		/// <summary>
		/// Appends the contents of the node as a string.
		/// This method is recursive across node classes and is used to construct a string for complex node structures.
		/// </summary>
		/// <param name="sb">String builder to append to</param>
		/// <param name="depth">Current depth (starting at 0)</param>
		internal virtual void ToString (StringBuilder sb, int depth)
		{
			sb.AppendFormat("({0}): {1}", Type, StringValue);
		}
	}
}
