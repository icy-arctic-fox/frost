﻿using System;
using System.IO;
using System.Text;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Base class for all node types
	/// </summary>
	public abstract class Node
	{
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

		// TODO: Add functionality for cloning nodes

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
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="br"/> is null.
		/// The reader used to pull data from the stream can't be null.</exception>
		internal static Node ReadFromStream (BinaryReader br)
		{
			if(br == null)
				throw new ArgumentNullException("br", "The reader used to pull data from the stream can't be null.");

			var type = readHeader(br);
			if(type == NodeType.End)
				return null; // End node
			else
			{// Regular node
				var reader = GetPayloadReader(type);
				return reader(br);
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
			case NodeType.Xy:
				return XyNode.ReadPayload;
			case NodeType.Xyz:
				return XyzNode.ReadPayload;
			case NodeType.Coordinate2D:
				return Coordinate2DNode.ReadPayload;
			case NodeType.Coordinate3D:
				return Coordinate3DNode.ReadPayload;
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
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bw"/> is null.
		/// The writer used to put data on the stream can't be null.</exception>
		internal void WriteToStream (BinaryWriter bw)
		{
			if(bw == null)
				throw new ArgumentNullException("bw", "The writer used to put data on the stream can't be null.");

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
