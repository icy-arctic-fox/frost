using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
		/// Creates the base for a new node
		/// </summary>
		/// <param name="name">Name of the node</param>
		protected Node (string name)
		{
			throw new NotImplementedException();
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
		/// <param name="s">Stream to read the node from</param>
		/// <returns>A node read from the stream or null if an "End" node was read</returns>
		internal static Node ReadNodeFromStream (Stream s)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads the header of a node
		/// </summary>
		/// <param name="br">Reader used to pull data from the stream</param>
		/// <param name="name">Name of the node</param>
		/// <returns>Type of node read from the stream</returns>
		private static NodeType readHeader (BinaryReader br, out string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns a method that will construct the node
		/// </summary>
		/// <param name="type">Node type</param>
		/// <returns>A node constructor method</returns>
		private static NodeConstructor getPayloadReader (NodeType type)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
