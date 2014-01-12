using System;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Contains extension methods for checking node types
	/// </summary>
	public static class NodeStructure
	{
		#region Required node values
		// These methods throw exceptions if the node isn't what was expected.

		/// <summary>
		/// Ensures that a node is of a specified type
		/// </summary>
		/// <param name="node">Node to check</param>
		/// <param name="type">Type of node that <paramref name="node"/> should be</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> does not match <paramref name="type"/></exception>
		public static void ExpectNodeType (this Node node, NodeType type)
		{
			if(node == null)
				throw new ArgumentNullException("node", "The node can't be null.");
			if(node.Type != type)
				throw new InvalidCastException("Expected node type " + type);
		}

		/// <summary>
		/// Ensures that a node is a byte node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="ByteNode"/></exception>
		public static byte ExpectByteNode (this Node node)
		{
			ExpectNodeType(node, NodeType.Byte);
			return ((ByteNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is a signed byte node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="SByteNode"/></exception>
		public static sbyte ExpectSByteNode (this Node node)
		{
			ExpectNodeType(node, NodeType.SByte);
			return ((SByteNode)node).Value;
		}
		#endregion
	}
}
