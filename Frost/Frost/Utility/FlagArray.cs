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
		private const int FlagsPerElement = sizeof(int) * 8;

		private readonly int _count;
		private readonly int[] _flags;

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
			var length = count / FlagsPerElement;
			if(count % FlagsPerElement != 0)
				++length; // Round up
			_flags = new int[length];
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
				var b = _flags[byteIndex];
				var v = b & (1 << offset);
				return v != 0;
			}

			set
			{
				int byteIndex, offset;
				getBitIndex(index, out byteIndex, out offset);
				var b = _flags[byteIndex];
				b &= (byte)~(1 << offset);
				if(value)
					b |= (byte)(1 << offset);
				_flags[byteIndex] = b;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the flags
		/// </summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the flags</returns>
		public IEnumerator<bool> GetEnumerator ()
		{
			for(var i = 0; i < _count; ++i)
				yield return this[i];
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
			if(array == null)
				throw new ArgumentNullException("array", "The array to copy to can't be null.");
			if(arrayIndex < 0)
				throw new ArgumentOutOfRangeException("arrayIndex", "The starting index can't be less than 0.");
			if(_count > array.Length - arrayIndex)
				throw new ArgumentException("There are more elements to copy than available space in the array.");

			var i = arrayIndex;
			var enumerator = GetEnumerator();
			while(enumerator.MoveNext())
				array[i++] = enumerator.Current;
		}

		/// <summary>
		/// Creates a byte array containing all of the flags
		/// </summary>
		/// <returns>A byte array</returns>
		public byte[] ToByteArray ()
		{
			var length = ByteLength;
			var bytes  = new byte[length];

			for(int srcIndex = 0, destIndex = 0, destByte = 0, destBit = 0; destIndex < _count; ++srcIndex, ++destIndex)
			{
				var v = this[srcIndex];
				var b = bytes[destByte];
				b &= (byte)~(1 << destBit);
				if(v)
					b |= (byte)(1 << destBit);
				bytes[destByte] = b;

				++destBit;
				if(destBit >= 8)
				{
					destBit = 0;
					++destByte;
				}
			}

			return bytes;
		}

		/// <summary>
		/// Creates a byte array containing the selected flags
		/// </summary>
		/// <param name="start">Index of the first flag to store</param>
		/// <param name="count">Number of flags to include</param>
		/// <returns>A byte array containing the selected flags</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Thrown if <paramref name="start"/> is less than 0 or greater than or equal to <see cref="Count"/></exception>
		/// <exception cref="T:System.ArgumentException">Thrown if <paramref name="count"/> is greater than the number of flags available minus <paramref name="start"/></exception>
		public byte[] ToByteArray (int start, int count)
		{
			if(start < 0 || start >= _count)
				throw new ArgumentOutOfRangeException("start", "The starting index is out of range.");
			if(count > _count - start)
				throw new ArgumentException("The number of items to copy exceeds the number of items available.");

			var length = count / 8;
			if(count % 8 != 0)
				++length;
			var bytes = new byte[length];

			for(int srcIndex = start, destIndex = 0, destByte = 0, destBit = 0; destIndex < count; ++srcIndex, ++destIndex)
			{
				var v = this[srcIndex];
				var b = bytes[destByte];
				b &= (byte)~(1 << destBit);
				if(v)
					b |= (byte)(1 << destBit);
				bytes[destByte] = b;

				++destBit;
				if(destBit >= 8)
				{
					destBit = 0;
					++destByte;
				}
			}

			return bytes;
		}

		/// <summary>
		/// Creates an integer array containing all of the flags
		/// </summary>
		/// <returns>An integer array</returns>
		public int[] ToIntArray ()
		{
			var ints = new int[_flags.Length];
			for(var i = 0; i < ints.Length; ++i)
				ints[i] = _flags[i];
			return ints;
		}

		/// <summary>
		/// Creates an integer array containing the selected flags
		/// </summary>
		/// <param name="start">Index of the first flag to store</param>
		/// <param name="count">Number of flags to include</param>
		/// <returns>An integer array containing the selected flags</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Thrown if <paramref name="start"/> is less than 0 or greater than or equal to <see cref="Count"/></exception>
		/// <exception cref="T:System.ArgumentException">Thrown if <paramref name="count"/> is greater than the number of flags available minus <paramref name="start"/></exception>
		public int[] ToIntArray (int start, int count)
		{
			if(start < 0 || start >= _count)
				throw new ArgumentOutOfRangeException("start", "The starting index is out of range.");
			if(count > _count - start)
				throw new ArgumentException("The number of items to copy exceeds the number of items available.");

			var length = count / FlagsPerElement;
			if(count % FlagsPerElement != 0)
				++length;
			var ints = new int[length];

			for(int srcIndex = start, destIndex = 0, destByte = 0, destBit = 0; destIndex < count; ++srcIndex, ++destIndex)
			{
				var v = this[srcIndex];
				var b = ints[destByte];
				b &= (byte)~(1 << destBit);
				if(v)
					b |= (byte)(1 << destBit);
				ints[destByte] = b;

				++destBit;
				if(destBit >= FlagsPerElement)
				{
					destBit = 0;
					++destByte;
				}
			}

			return ints;
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
			get { return _flags.Length * sizeof(int); }
		}

		/// <summary>
		/// Number of integers needed to store the flags
		/// </summary>
		public int IntLength
		{
			get { return _flags.Length; }
		}

		/// <summary>
		/// Calculates the byte and bit offset
		/// </summary>
		/// <param name="index">Index of the flag</param>
		/// <param name="intIndex">Integer index</param>
		/// <param name="offset">Bit index inside of the byte</param>
		/// <exception cref="IndexOutOfRangeException">Thrown if <paramref name="index"/> is out of range</exception>
		private void getBitIndex (int index, out int intIndex, out int offset)
		{
			if(index < 0 || index >= _count)
				throw new IndexOutOfRangeException("The flag index is out of range.");

			intIndex = index / FlagsPerElement;
			offset   = index % FlagsPerElement;
		}
	}
}
