using System;
using System.IO;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Base class for all node types
	/// </summary>
	public abstract class Node
	{
		#region Properties

		/// <summary>
		/// Name of the node
		/// </summary>
		private readonly string _name;

		/// <summary>
		/// Name of the node
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

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
		/// Checks if a string is valid for a node name
		/// </summary>
		/// <param name="name">String to check</param>
		/// <returns>True if the string is valid for a node name</returns>
		/// <remarks>Valid node names are not null, empty, contain only whitespace, or forward slashes.</remarks>
		private static bool isValidNodeName (string name)
		{
			return !(String.IsNullOrWhiteSpace(name) || name.Contains("/"));
		}

		/// <summary>
		/// Creates the base for a new node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <exception cref="ArgumentException">Thrown if the name provided for the node is invalid.
		/// Valid node names are not null, empty, contain only whitespace, or forward slashes.</exception>
		protected Node (string name)
		{
			if(!isValidNodeName(name))
				throw new ArgumentException("Invalid string for node name", "name");
			_name = name;
		}

		#region Serialization

		/// <summary>
		/// Describes a method that accepts a stream reader and node name and produces a node
		/// </summary>
		/// <param name="br">Stream reader to use to pull data from the stream</param>
		/// <param name="name">Name to give the node</param>
		/// <returns>A constructed node</returns>
		private delegate Node NodeConstructor (BinaryReader br, string name);

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

			string name;
			var type = readHeader(br, out name);
			if(type == NodeType.End)
				return null; // End node
			else
			{// Regular node
				var reader = getPayloadReader(type);
				return reader(br, name);
			}
		}

		/// <summary>
		/// Reads the header of a node
		/// </summary>
		/// <param name="br">Reader used to pull data from the stream</param>
		/// <param name="name">Name of the node</param>
		/// <returns>Type of node read from the stream</returns>
		private static NodeType readHeader (BinaryReader br, out string name)
		{
			var type = (NodeType)br.ReadByte();
			name = (type == NodeType.End) ? null : br.ReadString();
			return type;
		}

		/// <summary>
		/// Returns a method that will construct the node
		/// </summary>
		/// <param name="type">Node type</param>
		/// <returns>A node constructor method</returns>
		private static NodeConstructor getPayloadReader (NodeType type)
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
			bw.Write(_name);
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
		/// <returns>A string in the form: Name(Type): Value</returns>
		public override string ToString ()
		{
			return String.Format("{0}({1}): {2}", _name, Type, StringValue);
		}
	}
}
