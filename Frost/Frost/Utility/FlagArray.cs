using System;
using System.Collections;
using System.Collections.Generic;

namespace Frost.Utility
{
	/// <summary>
	/// Compacts a list of flags into a byte array by using individual bits
	/// </summary>
	public class FlagArray : IEnumerable<bool>
	{
		private readonly int _count;
		private readonly byte[] _bytes;

		/// <summary>
		/// Creates a new flag array
		/// </summary>
		/// <param name="count">Number of flags (bits)</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> is less than 1</exception>
		public FlagArray (int count)
		{
			if(count <= 1)
				throw new ArgumentOutOfRangeException("count", "The number of flags can't be less than 1.");

			_count = count;
			var length = count / 8;
			if(count % 8 != 0)
				++length; // Round up
			_bytes = new byte[length];
		}

		/// <summary>
		/// Value of an individual flag
		/// </summary>
		/// <param name="index">Index of the flag to access</param>
		/// <returns>Value of the flag</returns>
		/// <exception cref="IndexOutOfRangeException">Thrown if <paramref name="index"/> is out of range</exception>
		public bool this[int index]
		{
			get
			{
				int byteIndex, offset;
				getBitIndex(index, out byteIndex, out offset);
				var b = _bytes[byteIndex];
				var v = b & (1 << offset);
				return v != 0;
			}

			set
			{
				int byteIndex, offset;
				getBitIndex(index, out byteIndex, out offset);
				var b = _bytes[byteIndex];
				b &= (byte)~(1 << offset);
				if(value)
					b |= (byte)(1 << offset);
				_bytes[byteIndex] = b;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the flags
		/// </summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the flags</returns>
		public IEnumerator<bool> GetEnumerator ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the flags
		/// </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the flags</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Copies the flags to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the flags copied from the collection.
		/// The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins</param>
		/// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="array"/> is null</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Thrown if <paramref name="arrayIndex"/> is less than 0</exception>
		/// <exception cref="T:System.ArgumentException">Thrown if the number of flags in the source list is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/></exception>
		public void CopyTo (bool[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a byte array containing all of the flags
		/// </summary>
		/// <returns>A byte array</returns>
		public byte[] ToByteArray ()
		{
			return _bytes.Duplicate();
		}

		/// <summary>
		/// Creates a byte array containing the selected flags
		/// </summary>
		/// <param name="start">Index of the first flag to store</param>
		/// <param name="count">Number of flags to include</param>
		/// <returns>A byte array containing the selected flags</returns>
		public byte[] ToByteArray (int start, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the number of flags contained in the array
		/// </summary>
		/// <returns>The number of flags</returns>
		public int Count
		{
			get { return _count; }
		}

		/// <summary>
		/// Number of bytes needed to store the flags
		/// </summary>
		public int ByteLength
		{
			get { return _bytes.Length; }
		}

		/// <summary>
		/// Calculates the byte and bit offset
		/// </summary>
		/// <param name="index">Index of the flag</param>
		/// <param name="byteIndex">Byte index</param>
		/// <param name="offset">Bit index inside of the byte</param>
		/// <exception cref="IndexOutOfRangeException">Thrown if <paramref name="index"/> is out of range</exception>
		private void getBitIndex (int index, out int byteIndex, out int offset)
		{
			if(index < 0 || index >= _count)
				throw new IndexOutOfRangeException("The flag index is out of range.");

			byteIndex = index / 8;
			offset    = index % 8;
		}
	}
}
