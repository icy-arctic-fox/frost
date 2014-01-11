using System;
using System.IO;
using Frost.Utility;

namespace Frost.IO
{
	/// <summary>
	/// A stream reader that can read in little and big endian
	/// </summary>
	public class EndianBinaryReader : BinaryReader
	{
		private readonly Endian _endian;

		/// <summary>
		/// Endian being used for reading
		/// </summary>
		public Endian Endian
		{
			get { return _endian; }
		}

		/// <summary>
		/// Creates a new endian-aware binary reader
		/// </summary>
		/// <param name="input">Input stream to read from</param>
		/// <param name="e">Endian to use</param>
		public EndianBinaryReader (Stream input, Endian e)
			: base(input)
		{
			_endian = e;
		}

		/// <summary>
		/// Creates a new endian-aware binary reader
		/// </summary>
		/// <param name="input">Input stream to read from</param>
		/// <param name="encoding">Text encoding</param>
		/// <param name="e">Endian to use</param>
		public EndianBinaryReader (Stream input, System.Text.Encoding encoding, Endian e = Endian.Big)
			: base(input, encoding)
		{
			_endian = e;
		}

		/// <summary>
		/// Reads a double-precision floating point number and advances the stream by 8 bytes
		/// </summary>
		/// <returns>A double</returns>
		public override double ReadDouble ()
		{
			var value = base.ReadDouble();
			return NeedFlip ? value.FlipEndian() : value;
		}

		/// <summary>
		/// Reads a signed short integer and advances the stream by 2 bytes
		/// </summary>
		/// <returns>A signed short</returns>
		public override short ReadInt16 ()
		{
			var value = base.ReadInt16();
			return NeedFlip ? value.FlipEndian() : value;
		}

		/// <summary>
		/// Reads a signed integer and advances the stream by 4 bytes
		/// </summary>
		/// <returns>A signed integer</returns>
		public override int ReadInt32 ()
		{
			var value = base.ReadInt32();
			return NeedFlip ? value.FlipEndian() : value;
		}

		/// <summary>
		/// Reads a signed long integer and advances the stream by 8 bytes
		/// </summary>
		/// <returns>A signed long</returns>
		public override long ReadInt64 ()
		{
			var value = base.ReadInt64();
			return NeedFlip ? value.FlipEndian() : value;
		}

		/// <summary>
		/// Reads a single-precision floating point number and advances the stream by 4 bytes
		/// </summary>
		/// <returns>A float</returns>
		public override float ReadSingle ()
		{
			var value = base.ReadSingle();
			return NeedFlip ? value.FlipEndian() : value;
		}

		/// <summary>
		/// Reads an unsigned short integer and advances the stream by 2 bytes
		/// </summary>
		/// <returns>An unsigned short</returns>
		public override ushort ReadUInt16 ()
		{
			var value = base.ReadUInt16();
			return NeedFlip ? value.FlipEndian() : value;
		}

		/// <summary>
		/// Reads an unsigned integer and advances the stream by 4 bytes
		/// </summary>
		/// <returns>An unsigned integer</returns>
		public override uint ReadUInt32 ()
		{
			var value = base.ReadUInt32();
			return NeedFlip ? value.FlipEndian() : value;
		}

		/// <summary>
		/// Reads an unsigned long integer and advances the stream by 8 bytes
		/// </summary>
		/// <returns>An unsigned long</returns>
		public override ulong ReadUInt64 ()
		{
			var value = base.ReadUInt64();
			return NeedFlip ? value.FlipEndian() : value;
		}

		/// <summary>
		/// Indicates whether the values need to have their endian flipped
		/// </summary>
		private bool NeedFlip
		{
			get { return (_endian == Endian.Little) ^ BitConverter.IsLittleEndian; }
		}
	}
}
