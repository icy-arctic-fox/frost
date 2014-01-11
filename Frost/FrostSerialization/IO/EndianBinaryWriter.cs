using System;
using System.IO;
using Frost.Utility;

namespace Frost.IO
{
	/// <summary>
	/// A stream writer that can read in little and big endian
	/// </summary>
	public class EndianBinaryWriter : BinaryWriter
	{
		private readonly Endian _endian;

		/// <summary>
		/// Endian being used for writing
		/// </summary>
		public Endian Endian
		{
			get { return _endian; }
		}

		/// <summary>
		/// Creates a new endian-aware binary writer
		/// </summary>
		/// <param name="output">Output stream to write to</param>
		/// <param name="e">Endian to use</param>
		public EndianBinaryWriter (Stream output, Endian e)
			: base(output)
		{
			_endian = e;
		}

		/// <summary>
		/// Creates a new endian-aware binary writer
		/// </summary>
		/// <param name="output">Output stream to write to</param>
		/// <param name="encoding">Text encoding</param>
		/// <param name="e">Endian to use</param>
		public EndianBinaryWriter (Stream output, System.Text.Encoding encoding, Endian e = Endian.Big)
			: base(output, encoding)
		{
			_endian = e;
		}

		/// <summary>
		/// Writes a double-precision floating point number and advances the stream by 8 bytes
		/// </summary>
		public override void Write (double value)
		{
			var result = NeedFlip ? value.FlipEndian() : value;
			base.Write(result);
		}

		/// <summary>
		/// Writes a signed short integer and advances the stream by 2 bytes
		/// </summary>
		public override void Write (short value)
		{
			var result = NeedFlip ? value.FlipEndian() : value;
			base.Write(result);
		}

		/// <summary>
		/// Writes a signed integer and advances the stream by 4 bytes
		/// </summary>
		public override void Write (int value)
		{
			var result = NeedFlip ? value.FlipEndian() : value;
			base.Write(result);
		}

		/// <summary>
		/// Writes a signed long integer and advances the stream by 8 bytes
		/// </summary>
		public override void Write (long value)
		{
			var result = NeedFlip ? value.FlipEndian() : value;
			base.Write(result);
		}

		/// <summary>
		/// Writes a single-precision floating point number and advances the stream by 4 bytes
		/// </summary>
		public override void Write (float value)
		{
			var result = NeedFlip ? value.FlipEndian() : value;
			base.Write(result);
		}

		/// <summary>
		/// Writes an unsigned short integer and advances the stream by 2 bytes
		/// </summary>
		public override void Write (ushort value)
		{
			var result = NeedFlip ? value.FlipEndian() : value;
			base.Write(result);
		}

		/// <summary>
		/// Writes an unsigned integer and advances the stream by 4 bytes
		/// </summary>
		public override void Write (uint value)
		{
			var result = NeedFlip ? value.FlipEndian() : value;
			base.Write(result);
		}

		/// <summary>
		/// Writes an unsigned long integer and advances the stream by 8 bytes
		/// </summary>
		public override void Write (ulong value)
		{
			var result = NeedFlip ? value.FlipEndian() : value;
			base.Write(result);
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
