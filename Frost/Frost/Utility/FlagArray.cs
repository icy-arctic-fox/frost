using System;
using System.Collections;
using System.Collections.Generic;

namespace Frost.Utility
{
	/// <summary>
	/// Compacts a list of flags into a byte array by using individual bits
	/// </summary>
	public class FlagArray : IEnumerable<bool>, IEnumerable
	{
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
		public bool this[int index]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
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
			throw new NotImplementedException();
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
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Number of bytes needed to store the flags
		/// </summary>
		public int ByteLength
		{
			get { throw new NotImplementedException(); }
		}
	}
}
