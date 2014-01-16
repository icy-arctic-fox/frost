using System;
using System.Windows.Forms;
using Frost.IO.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class IntegerNodeValueControl : UserControl, INodeEditorControl
	{
		public IntegerNodeValueControl ()
		{
			InitializeComponent();
		}

		private NodeType _type;

		/// <summary>
		/// Type of node that will be produced by <see cref="AsNode"/>
		/// </summary>
		public NodeType NodeType
		{
			get { return _type; }
			set { _type = value; }
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			var value = numericUpDown.Value;
			switch(_type)
			{
			case NodeType.Byte:
				return new ByteNode((byte)value);
			case NodeType.SByte:
				return new SByteNode((sbyte)value);
			case NodeType.Short:
				return new ShortNode((short)value);
			case NodeType.UShort:
				return new UShortNode((ushort)value);
			case NodeType.Int:
				return new IntNode((int)value);
			case NodeType.UInt:
				return new UIntNode((uint)value);
			case NodeType.Long:
				return new LongNode((long)value);
			case NodeType.ULong:
				return new ULongNode((ulong)value);
			}
			return null;
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			decimal min, max, value;
			switch(node.Type)
			{
			case NodeType.Byte:
				min   = Byte.MinValue;
				max   = Byte.MaxValue;
				value = ((ByteNode)node).Value;
				break;
			case NodeType.SByte:
				min   = SByte.MinValue;
				max   = SByte.MaxValue;
				value = ((SByteNode)node).Value;
				break;
			case NodeType.Short:
				min   = Int16.MinValue;
				max   = Int16.MaxValue;
				value = ((ShortNode)node).Value;
				break;
			case NodeType.UShort:
				min   = UInt16.MinValue;
				max   = UInt16.MaxValue;
				value = ((UShortNode)node).Value;
				break;
			case NodeType.Int:
				min   = Int32.MinValue;
				max   = Int32.MaxValue;
				value = ((IntNode)node).Value;
				break;
			case NodeType.UInt:
				min   = UInt32.MinValue;
				max   = UInt32.MaxValue;
				value = ((UIntNode)node).Value;
				break;
			case NodeType.Long:
				min   = Int64.MinValue;
				max   = Int64.MaxValue;
				value = ((LongNode)node).Value;
				break;
			case NodeType.ULong:
				min   = UInt64.MinValue;
				max   = UInt64.MaxValue;
				value = ((ULongNode)node).Value;
				break;
			default:
				min   = 0;
				max   = 100;
				value = 0;
				break;
			}

			numericUpDown.Minimum = min;
			numericUpDown.Maximum = max;
			numericUpDown.Value   = value;
			_type = node.Type;
		}
	}
}
