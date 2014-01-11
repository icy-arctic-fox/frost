using System;
using System.Collections;
using System.Collections.Generic;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Contains multiple nodes of different types and addresses them by name
	/// </summary>
	public class ComplexNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Complex"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Complex; }
		}

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { throw new NotImplementedException(); }
		}
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

		private readonly Dictionary<string, Node> _nodes = new Dictionary<string, Node>();

		/// <summary>
		/// Creates a new empty complex node
		/// </summary>
		public ComplexNode ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new complex node with initial contents
		/// </summary>
		/// <param name="nodes">Initial nodes to add</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="nodes"/> is null.
		/// The initial collection of nodes can't be null.</exception>
		/// <exception cref="ArgumentException">Thrown if one of the nodes in <paramref name="nodes"/> is null or has an invalid name</exception>
		public ComplexNode (IEnumerable<KeyValuePair<string, Node>> nodes)
		{
			if(nodes == null)
				throw new ArgumentNullException("nodes", "The collection of initial nodes can't be null.");
			foreach(var entry in nodes)
			{
				var name = entry.Key;
				var node = entry.Value;
				if(!isValidNodeName(name))
					throw new ArgumentException("Invalid node name - " + (name ?? "null"), "nodes");
				if(node == null)
					throw new ArgumentException("Cannot add a null node", "nodes");
				_nodes.Add(name, node);
			}
		}

		#region Serialization

		/// <summary>
		/// Constructs a complex node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed complex node</returns>
		internal static ComplexNode ReadPayload (System.IO.BinaryReader br)
		{
			var complex = new ComplexNode();
			while(true)
			{
				var node = ReadFromStream(br);
				if(node == null)
					break; // End marker
				var name = br.ReadString();
				complex._nodes.Add(name, node);
			}
			return complex;
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			foreach(var entry in _nodes)
			{
				var name = entry.Key;
				var node = entry.Value;
				node.WriteToStream(bw);
				bw.Write(name);
			}
			bw.Write((byte)NodeType.End);
		}
		#endregion
	}
}
