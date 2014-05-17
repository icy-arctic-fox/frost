using System;
using Frost.Utility;

namespace Frost.Tnt
{
	/// <summary>
	/// Raw binary data node
	/// </summary>
	public class BlobNode : Node
	{
		#region Node properties

		/// <summary>
		/// Indicates the type of node.
		/// This can be used to safely cast nodes.
		/// </summary>
		/// <remarks>The type for this node is always <see cref="NodeType.Blob"/>.</remarks>
		public override NodeType Type
		{
			get { return NodeType.Blob; }
		}

		private byte[] _data;

		/// <summary>
		/// Data stored in the node
		/// </summary>
		/// <exception cref="ArgumentNullException">Can't set the byte array to null.</exception>
		public byte[] Data
		{
			get { return _data; }
			set
			{
				if(value == null)
					throw new ArgumentNullException("value");
				_data = value;
			}
		}

		/// <summary>
		/// Value of the node as a string
		/// </summary>
		public override string StringValue
		{
			get { return String.Format("{0} bytes", _data.Length); }
		}
		#endregion

		/// <summary>
		/// Creates a new blob node
		/// </summary>
		/// <param name="data">Bytes to store in the node</param>
		/// <exception cref="ArgumentNullException">Thrown when attempting to set the data to null.
		/// The byte array cannot be null.</exception>
		public BlobNode (byte[] data)
		{
			Data = data;
		}

		/// <summary>
		/// Creates a new node that is a copy of the current instance
		/// </summary>
		/// <returns>A new node that is a copy of this instance</returns>
		public BlobNode CloneNode ()
		{
			var data = _data.Duplicate();
			return new BlobNode(data);
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
		/// Constructs a blob node by reading its payload from a stream
		/// </summary>
		/// <param name="br">Reader to use to pull data from the stream</param>
		/// <returns>A constructed blob node</returns>
		internal static BlobNode ReadPayload (System.IO.BinaryReader br)
		{
			var length = br.ReadInt32();
			var data   = br.ReadBytes(length);
			return new BlobNode(data);
		}

		/// <summary>
		/// Writes the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Writer to use to put data on the stream</param>
		internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			bw.Write(_data.Length);
			bw.Write(_data);
		}
		#endregion
	}
}
