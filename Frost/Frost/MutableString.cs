using System;
using System.Collections;
using System.Collections.Generic;

namespace Frost
{
	/// <summary>
	/// Character string that allows its contents to be changed
	/// </summary>
	/// <remarks>This class is not thread-safe.
	/// Users of this class will need to ensure that access to this class is in thread-safe context.</remarks>
	public sealed class MutableString : IEquatable<string>, IComparable<string>, IEnumerable<char>, ICloneable
	{
		private const int DefaultCapacity = 64;

		private char[] _chars;

		/// <summary>
		/// Number of characters in the string
		/// </summary>
		public int Length
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Number of available characters the string can hold before reallocating memory
		/// </summary>
		public int Capacity
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Indexed access to each character in the string
		/// </summary>
		/// <param name="index">Index of the character to access</param>
		public char this[int index]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		#region Constructors

		/// <summary>
		/// Creates an empty mutable string
		/// </summary>
		public MutableString ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates an empty mutable string with an initial character capacity
		/// </summary>
		/// <param name="capacity">Initial number of characters the string can hold</param>
		public MutableString (int capacity)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a mutable string from an existing string
		/// </summary>
		/// <param name="s">Existing string</param>
		/// <exception cref="ArgumentNullException">The existing string can't be null.</exception>
		public MutableString (string s)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a mutable string from a set of characters
		/// </summary>
		/// <param name="chars">Initial characters</param>
		/// <exception cref="ArgumentNullException">The initial characters can't be null.</exception>
		public MutableString (char[] chars)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Copies an existing mutable string
		/// </summary>
		/// <param name="s">String to copy from</param>
		/// <exception cref="ArgumentNullException">The string to copy from can't be null.</exception>
		public MutableString (MutableString s)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Operations
		#endregion

		/// <summary>
		/// Returns an enumerator that iterates through the characters in the string
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the characters in the string</returns>
		public IEnumerator<char> GetEnumerator ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection of characters
		/// </summary>
		/// <returns>An enumerator object that can be used to iterate through the collection of characters</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance
		/// </summary>
		/// <returns>A new mutable string with the same contents as the current instance</returns>
		public object Clone ()
		{
			return new MutableString(this);
		}

		/// <summary>
		/// Determines whether another object is equal the current instance of the mutable string
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if the object has the same contents</returns>
		public override bool Equals (object obj)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>True if the current object is equal to the <paramref name="other"/> parameter; otherwise, false</returns>
		public bool Equals (string other)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether another mutable string is equal to the current instance
		/// </summary>
		/// <param name="other">String to compare against</param>
		/// <returns>True if the mutable strings' contents are equal</returns>
		private bool Equals (MutableString other)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Hashes the contents of the string
		/// </summary>
		/// <returns>A hash code for the current string</returns>
		public override int GetHashCode ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines if the left and right strings are equal
		/// </summary>
		/// <param name="left">First string to compare</param>
		/// <param name="right">Second string to compare</param>
		/// <returns>True if the <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		public static bool operator == (MutableString left, MutableString right)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines if the left and right strings are not equal
		/// </summary>
		/// <param name="left">First string to compare</param>
		/// <param name="right">Second string to compare</param>
		/// <returns>True if the <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		public static bool operator != (MutableString left, MutableString right)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public int CompareTo (string other)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a non-mutable string
		/// </summary>
		/// <returns>Regular string</returns>
		public override string ToString ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts a mutable string to a non-mutable string
		/// </summary>
		/// <param name="str">Mutable string to convert</param>
		/// <returns>Non-mutable string</returns>
		public static implicit operator string (MutableString str)
		{
			return str == null ? null : str.ToString();
		}
	}
}
