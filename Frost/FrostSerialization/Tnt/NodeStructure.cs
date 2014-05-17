using System;
using System.Collections.Generic;

namespace Frost.Tnt
{
	/// <summary>
	/// Contains extension methods for checking node types
	/// </summary>
	public static class NodeStructure
	{
		/// <summary>
		/// Maps full class names of nodes to their corresponding <see cref="NodeType"/> value
		/// </summary>
		private static readonly Dictionary<string, NodeType> _nodeTypeMap = new Dictionary<string, NodeType> {
			{typeof(BooleanNode).FullName,  NodeType.Boolean},
			{typeof(ByteNode).FullName,     NodeType.Byte},
			{typeof(SByteNode).FullName,    NodeType.SByte},
			{typeof(ShortNode).FullName,    NodeType.Short},
			{typeof(UShortNode).FullName,   NodeType.UShort},
			{typeof(IntNode).FullName,      NodeType.Int},
			{typeof(UIntNode).FullName,     NodeType.UInt},
			{typeof(LongNode).FullName,     NodeType.Long},
			{typeof(ULongNode).FullName,    NodeType.ULong},
			{typeof(FloatNode).FullName,    NodeType.Float},
			{typeof(DoubleNode).FullName,   NodeType.Double},
			{typeof(StringNode).FullName,   NodeType.String},
			{typeof(GuidNode).FullName,     NodeType.Guid},
			{typeof(DateTimeNode).FullName, NodeType.DateTime},
			{typeof(TimeSpanNode).FullName, NodeType.TimeSpan},
			{typeof(BlobNode).FullName,     NodeType.Blob},
			{typeof(Point2iNode).FullName,  NodeType.Point2i},
			{typeof(Point3iNode).FullName,  NodeType.Point3i},
			{typeof(Point2fNode).FullName,  NodeType.Point2f},
			{typeof(Point3fNode).FullName,  NodeType.Point3f},
			{typeof(ColorNode).FullName,    NodeType.Color},
			{typeof(ListNode).FullName,     NodeType.List},
			{typeof(ComplexNode).FullName,  NodeType.Complex}
		};

		/// <summary>
		/// Converts a node type class to the <see cref="NodeType"/> enumeration
		/// </summary>
		/// <param name="t">Type of node class to convert</param>
		/// <returns>A node type</returns>
		/// <remarks><see cref="NodeType.End"/> will be returned if <paramref name="t"/> is not a known sub-class of <see cref="Node"/>.</remarks>
		public static NodeType ClassToNodeType (Type t)
		{
			var typeName = t.FullName;
			NodeType type;
			return _nodeTypeMap.TryGetValue(typeName, out type) ? type : NodeType.End;
		}

		#region Required node types
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
				throw new ArgumentNullException("node");
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

		/// <summary>
		/// Ensures that a node is a short node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="ShortNode"/></exception>
		public static short ExpectShortNode (this Node node)
		{
			ExpectNodeType(node, NodeType.Short);
			return ((ShortNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is an unsigned short node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="UShortNode"/></exception>
		public static ushort ExpectUShortNode (this Node node)
		{
			ExpectNodeType(node, NodeType.UShort);
			return ((UShortNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is an integer node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="IntNode"/></exception>
		public static int ExpectIntNode (this Node node)
		{
			ExpectNodeType(node, NodeType.Int);
			return ((IntNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is an unsigned integer node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="UIntNode"/></exception>
		public static uint ExpectUIntNode (this Node node)
		{
			ExpectNodeType(node, NodeType.UInt);
			return ((UIntNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is a long node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="LongNode"/></exception>
		public static long ExpectLongNode (this Node node)
		{
			ExpectNodeType(node, NodeType.Long);
			return ((LongNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is an unsigned long node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="ULongNode"/></exception>
		public static ulong ExpectULongNode (this Node node)
		{
			ExpectNodeType(node, NodeType.ULong);
			return ((ULongNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is a float node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="FloatNode"/></exception>
		public static float ExpectFloatNode (this Node node)
		{
			ExpectNodeType(node, NodeType.Float);
			return ((FloatNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is a double node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="DoubleNode"/></exception>
		public static double ExpectDoubleNode (this Node node)
		{
			ExpectNodeType(node, NodeType.Double);
			return ((DoubleNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is a string node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="StringNode"/></exception>
		public static string ExpectStringNode (this Node node)
		{
			ExpectNodeType(node, NodeType.String);
			return ((StringNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is a Guid node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="GuidNode"/></exception>
		public static Guid ExpectGuidNode (this Node node)
		{
			ExpectNodeType(node, NodeType.Guid);
			return ((GuidNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is a date and time node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="DateTimeNode"/></exception>
		public static DateTime ExpectDateTimeNode (this Node node)
		{
			ExpectNodeType(node, NodeType.DateTime);
			return ((DateTimeNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is a time span node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="TimeSpanNode"/></exception>
		public static TimeSpan ExpectTimeSpanNode (this Node node)
		{
			ExpectNodeType(node, NodeType.TimeSpan);
			return ((TimeSpanNode)node).Value;
		}

		/// <summary>
		/// Ensures that a node is a blob node and retrieves its data
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's data</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="BlobNode"/></exception>
		public static byte[] ExpectBlobNode (this Node node)
		{
			ExpectNodeType(node, NodeType.Blob);
			return ((BlobNode)node).Data;
		}

		/// <summary>
		/// Ensures that a node is a 2D integer point node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <param name="x">The node's x-value</param>
		/// <param name="y">The node's y-value</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="Point2iNode"/></exception>
		public static void ExpectPoint2iNode (this Node node, out int x, out int y)
		{
			ExpectNodeType(node, NodeType.Point2i);
			var point = (Point2iNode)node;
			x = point.X;
			y = point.Y;
		}

		/// <summary>
		/// Ensures that a node is a 3D integer point node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <param name="x">The node's x-value</param>
		/// <param name="y">The node's y-value</param>
		/// <param name="z">The node's z-value</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="Point3iNode"/></exception>
		public static void ExpectPoint3iNode (this Node node, out int x, out int y, out int z)
		{
			ExpectNodeType(node, NodeType.Point3i);
			var point = (Point3iNode)node;
			x = point.X;
			y = point.Y;
			z = point.Z;
		}

		/// <summary>
		/// Ensures that a node is a 2D point node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <param name="x">The node's x-coordinate</param>
		/// <param name="y">The node's y-coordinate</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="Point2fNode"/></exception>
		public static void ExpectPoint2fNode (this Node node, out float x, out float y)
		{
			ExpectNodeType(node, NodeType.Point2f);
			var point = (Point2fNode)node;
			x = point.X;
			y = point.Y;
		}

		/// <summary>
		/// Ensures that a node is a 3D point node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's value</returns>
		/// <param name="x">The node's x-coordinate</param>
		/// <param name="y">The node's y-coordinate</param>
		/// <param name="z">The node's z-coordinate</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="Point3fNode"/></exception>
		public static void ExpectPoint3fNode (this Node node, out float x, out float y, out float z)
		{
			ExpectNodeType(node, NodeType.Point3f);
			var point = (Point3fNode)node;
			x = point.X;
			y = point.Y;
			z = point.Z;
		}

		/// <summary>
		/// Ensures that a node is a color node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The node's ARGB value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="ColorNode"/></exception>
		public static int ExpectColorNode (this Node node)
		{
			ExpectNodeType(node, NodeType.Color);
			return ((ColorNode)node).Argb;
		}

		/// <summary>
		/// Ensures that a node is a list node with expected elements and returns the list
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <param name="type">Expected list element type</param>
		/// <returns>The list node</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="ListNode"/>
		/// or the type of elements in the list don't match <paramref name="type"/></exception>
		public static ListNode ExpectListNode (this Node node, NodeType type)
		{
			ExpectNodeType(node, NodeType.List);
			var list = (ListNode)node;
			if(list.ElementType != type)
				throw new InvalidCastException("The elements in the list node are not of the expected type.");
			return list;
		}

		/// <summary>
		/// Ensures that a node is a complex node and retrieves its value
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>The complex node</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if the type for <paramref name="node"/> is not a <see cref="ComplexNode"/></exception>
		public static ComplexNode ExpectComplexNode (this Node node)
		{
			ExpectNodeType(node, NodeType.Complex);
			return (ComplexNode)node;
		}
		#endregion
		
		#region Required complex child node types
		// These methods throw exceptions if the child node in a complex node isn't what was expected.

		/// <summary>
		/// Ensures that a child node is of a specified type
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <param name="type">Type of node that the child should be</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> or <paramref name="name"/> are null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node does not match <paramref name="type"/></exception>
		public static void ExpectNodeType (this ComplexNode node, string name, NodeType type)
		{
			if(node == null)
				throw new ArgumentNullException("node");
			if(name == null)
				throw new ArgumentNullException("name");
			Node child;
			if(!node.TryGetValue(name, out child))
				throw new FormatException("The complex node does not contain a child node by the name of '" + name + "'");
			if(child.Type != type)
				throw new InvalidCastException("Expected node type " + type);
		}

		/// <summary>
		/// Ensures that a child node is of a specified type
		/// </summary>
		/// <typeparam name="TChild">Node type that the child is expected to be</typeparam>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node cast to the correct type</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> or <paramref name="name"/> are null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node does not match the type parameter</exception>
		public static TChild ExpectNodeType<TChild> (this ComplexNode node, string name) where TChild : Node
		{
			if(node == null)
				throw new ArgumentNullException("node");
			if(name == null)
				throw new ArgumentNullException("name");
			Node child;
			if(!node.TryGetValue(name, out child))
				throw new FormatException("The complex node does not contain a child node by the name of '" + name + "'");
			var type = ClassToNodeType(typeof(TChild));
			if(child.Type != type)
				throw new InvalidCastException("Expected node type " + type);
			return (TChild)child;
		}

		/// <summary>
		/// Ensures that a child node is a byte node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="ByteNode"/></exception>
		public static byte ExpectByteNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<ByteNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is a signed byte node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="SByteNode"/></exception>
		public static sbyte ExpectSByteNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<SByteNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is a short node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="ShortNode"/></exception>
		public static short ExpectShortNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<ShortNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is an unsigned short node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="UShortNode"/></exception>
		public static ushort ExpectUShortNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<UShortNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is an integer node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="IntNode"/></exception>
		public static int ExpectIntNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<IntNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is an unsigned integer node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="UIntNode"/></exception>
		public static uint ExpectUIntNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<UIntNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is a long node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="LongNode"/></exception>
		public static long ExpectLongNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<LongNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is an unsigned long node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="ULongNode"/></exception>
		public static ulong ExpectULongNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<ULongNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is a float node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="FloatNode"/></exception>
		public static float ExpectFloatNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<FloatNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is a double node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="DoubleNode"/></exception>
		public static double ExpectDoubleNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<DoubleNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is a string node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="StringNode"/></exception>
		public static string ExpectStringNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<StringNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is a GUID node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="GuidNode"/></exception>
		public static Guid ExpectGuidNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<GuidNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is a date and time node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="DateTimeNode"/></exception>
		public static DateTime ExpectDateTimeNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<DateTimeNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is a time span node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's value</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="TimeSpanNode"/></exception>
		public static TimeSpan ExpectTimeSpanNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<TimeSpanNode>(node, name);
			return child.Value;
		}

		/// <summary>
		/// Ensures that a child node is a blob node and retrieves its data
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The child node's data</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="BlobNode"/></exception>
		public static byte[] ExpectBlobNode (this ComplexNode node, string name)
		{
			var child = ExpectNodeType<BlobNode>(node, name);
			return child.Data;
		}

		/// <summary>
		/// Ensures that a child node is a 2D integer node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <param name="x">The child node's x-value</param>
		/// <param name="y">The child node's y-value</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="Point2iNode"/></exception>
		public static void ExpectPoint2iNode (this ComplexNode node, string name, out int x, out int y)
		{
			var child = ExpectNodeType<Point2iNode>(node, name);
			x = child.X;
			y = child.Y;
		}

		/// <summary>
		/// Ensures that a child node is a XYZ node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <param name="x">The child node's x-value</param>
		/// <param name="y">The child node's y-value</param>
		/// <param name="z">The child node's z-value</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="Point3iNode"/></exception>
		public static void ExpectXyzNode (this ComplexNode node, string name, out int x, out int y, out int z)
		{
			var child = ExpectNodeType<Point3iNode>(node, name);
			x = child.X;
			y = child.Y;
			z = child.Z;
		}

		/// <summary>
		/// Ensures that a child node is a 2D point node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <param name="x">The child node's x-coordinate</param>
		/// <param name="y">The child node's y-coordinate</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="Point2fNode"/></exception>
		public static void ExpectPoint2fNode (this ComplexNode node, string name, out float x, out float y)
		{
			var child = ExpectNodeType<Point2fNode>(node, name);
			x = child.X;
			y = child.Y;
		}

		/// <summary>
		/// Ensures that a child node is a 3D coordinate node and retrieves its value
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <param name="x">The child node's x-coordinate</param>
		/// <param name="y">The child node's y-coordinate</param>
		/// <param name="z">The child node's z-coordinate</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="Point3fNode"/></exception>
		public static void ExpectPoint3fNode (this ComplexNode node, string name, out float x, out float y, out float z)
		{
			var child = ExpectNodeType<Point3fNode>(node, name);
			x = child.X;
			y = child.Y;
			z = child.Z;
		}

		/// <summary>
		/// Ensures that a child node is a list node and that its elements are of a given type
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <param name="type">Type of elements in the list node</param>
		/// <returns>The list node</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="ListNode"/>
		/// or the elements of the list node do not match <paramref name="type"/></exception>
		public static ListNode ExpectListNode (this ComplexNode node, string name, NodeType type)
		{
			var child = ExpectNodeType<ListNode>(node, name);
			if (child.ElementType != type)
				throw new InvalidCastException("The elements in the list node are not of the expected type.");
			return child;
		}

		/// <summary>
		/// Ensures that a child node is a complex node
		/// </summary>
		/// <param name="node">Complex node to check</param>
		/// <param name="name">Name of the child node</param>
		/// <returns>The complex node</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if no child exists in <paramref name="node"/> with the name <paramref name="name"/></exception>
		/// <exception cref="InvalidCastException">Thrown if the type for the child node is not a <see cref="ComplexNode"/></exception>
		public static ComplexNode ExpectComplexNode (this ComplexNode node, string name)
		{
			return ExpectNodeType<ComplexNode>(node, name);
		}
		#endregion
	}
}
