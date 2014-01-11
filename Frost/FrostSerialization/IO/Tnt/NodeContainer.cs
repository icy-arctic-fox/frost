using System;
using System.IO;

namespace Frost.IO.Tnt
{
	/// <summary>
	/// Wraps a typed-node structure.
	/// This class is used to serialize and deserialize a node structure.
	/// </summary>
	/// <remarks>The node data is serialized in big-endian.
	/// When using the serialization and streaming methods that use a <see cref="BinaryReader"/> or <see cref="BinaryWriter"/>,
	/// please make sure that they are writing in big-endian format.</remarks>
	public class NodeContainer : ISerializable, IStreamable
	{
		/// <summary>
		/// Current version
		/// </summary>
		public const int TypedNodeVersion = 1;

		private readonly Node _root;

		/// <summary>
		/// Root node in the container
		/// </summary>
		public Node Root
		{
			get { return _root; }
		}

		private readonly int _ver;

		/// <summary>
		/// Version of the nodes used in the structure
		/// </summary>
		public int Version
		{
			get { return _ver; }
		}

		/// <summary>
		/// Accesses a node in the tree by path name.
		/// The name or index of each node is separated by a /.
		/// </summary>
		/// <param name="path">Path to the desired node</param>
		/// <returns>The node at the end of the path or null if the node doesn't exist</returns>
		/// <exception cref="InvalidCastException">Thrown if the root node isn't a list or complex node</exception>
		public Node this[string path]
		{
			get
			{
				switch(_root.Type)
				{
				case NodeType.Complex:
					return ((ComplexNode)_root)[path];
				case NodeType.List:
					return ((ListNode)_root)[path];
				default:
					throw new InvalidCastException("The root node to traverse must be a list or complex node.");
				}
			}
		}

		/// <summary>
		/// Creates a new node container
		/// </summary>
		/// <param name="root">Root node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="root"/> is null.
		/// The root node of the container can't be null.</exception>
		public NodeContainer (Node root)
		{
			if(root == null)
				throw new ArgumentNullException("root", "The root node of the container can't be null.");
			_root = root;
			_ver  = TypedNodeVersion;
		}

		private NodeContainer (Node root, int ver)
		{
			_root = root;
			_ver  = ver;
		}

		#region Serialization

		/// <summary>
		/// Creates a node container by extracting serialized data
		/// </summary>
		/// <param name="data">Marshaled data of the node container (as created by <see cref="Serialize"/>)</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null.
		/// The marshaled data can't be null.</exception>
		/// <exception cref="FormatException">Thrown if <paramref name="data"/> contains an unrecognized typed-node version</exception>
		public NodeContainer (byte[] data)
		{
			if(data == null)
				throw new ArgumentNullException("data", "The marshaled data can't be null.");

			using(var ms = new MemoryStream(data))
			using(var br = new EndianBinaryReader(ms, Endian.Big))
			{
				_ver = br.ReadInt32();
				if(_ver != TypedNodeVersion)
					throw new FormatException("Unsupported typed-node version or unrecognized typed-node data");
				_root = Node.ReadFromStream(br);
			}
		}

		/// <summary>
		/// Packs up the contents of the container into an array of bytes
		/// </summary>
		/// <returns>Byte array containing marshaled data from the object</returns>
		public byte[] Serialize ()
		{
			using(var ms = new MemoryStream())
			{
				using(var bw = new EndianBinaryWriter(ms, Endian.Big))
					WriteToStream(bw);
				return ms.ToArray();
			}
		}

		/// <summary>
		/// Constructs a node container from data contained in a stream
		/// </summary>
		/// <param name="s">Stream to pull data from</param>
		/// <returns>A constructed node container</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is null.
		/// The stream to pull data from can't be null.</exception>
		public static NodeContainer ReadFromStream (Stream s)
		{
			if(s == null)
				throw new ArgumentNullException("s", "The stream pull node data from can't be null.");
			using(var br = new EndianBinaryReader(s, Endian.Big))
				return ReadFromStream(br);
		}

		/// <summary>
		/// Constructs a node container from data contained in a stream
		/// </summary>
		/// <param name="br">Reader used to pull data from the stream</param>
		/// <returns>A constructed node container</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="br"/> is null.
		/// The reader used to pull data from the stream can't be null.</exception>
		public static NodeContainer ReadFromStream (BinaryReader br)
		{
			if(br == null)
				throw new ArgumentNullException("br", "The reader used to pull data from the stream can't be null.");

			var ver = br.ReadInt32();
			if(ver != TypedNodeVersion)
				throw new FormatException("Unsupported typed-node version or unrecognized typed-node data");
			var root = Node.ReadFromStream(br);
			return new NodeContainer(root, ver);
		}

		/// <summary>
		/// Writes the contents of the container to a stream
		/// </summary>
		/// <param name="s">Stream to write data to</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is null.
		/// The stream to write data to can't be null.</exception>
		public void WriteToStream (Stream s)
		{
			if(s == null)
				throw new ArgumentNullException("s", "The stream to write the container to can't be null.");
			using(var bw = new EndianBinaryWriter(s, Endian.Big))
				WriteToStream(bw);
		}

		/// <summary>
		/// Writes the contents of the container to a stream using a stream writer
		/// </summary>
		/// <param name="bw">Binary writer to use for writing data to the stream</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bw"/> is null.
		/// The writer for putting data on the stream can't be null.</exception>
		public void WriteToStream (BinaryWriter bw)
		{
			if(bw == null)
				throw new ArgumentNullException("bw", "The writer used to put data on the stream can't be null.");

			bw.Write(_ver);
			_root.WriteToStream(bw);
		}
		#endregion

		/// <summary>
		/// Creates a string that represents the node container
		/// </summary>
		/// <returns>Structure of the container as a string</returns>
		public override string ToString ()
		{
			var sb = new System.Text.StringBuilder();
			sb.AppendFormat("Node container version {0}\n", _ver);
			_root.ToString(sb, 0);
			return sb.ToString();
		}
	}
}
