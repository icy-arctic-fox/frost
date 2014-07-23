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
		public int Length { get; private set; }

		/// <summary>
		/// Number of available characters the string can hold before reallocating memory
		/// </summary>
		public int Capacity
		{
			get { return _chars.Length; }
			set
			{
				if(value < 0)
					throw new ArgumentOutOfRangeException("value", "The new capacity must be non-negative.");
				_chars = resizeArray(_chars, value, Length);
			}
		}

		/// <summary>
		/// Indexed access to each character in the string
		/// </summary>
		/// <param name="index">Index of the character to access</param>
		public char this[int index]
		{
			get
			{
				if(index < 0 || index >= Length)
					throw new IndexOutOfRangeException();
				return _chars[index];
			}

			set
			{
				if(index < 0 || index >= Length)
					throw new IndexOutOfRangeException();
				_chars[index] = value;
			}
		}

		#region Constructors

		/// <summary>
		/// Creates an empty mutable string
		/// </summary>
		public MutableString ()
		{
			_chars = new char[DefaultCapacity];
		}

		/// <summary>
		/// Creates an empty mutable string with an initial character capacity
		/// </summary>
		/// <param name="capacity">Initial number of characters the string can hold</param>
		/// <exception cref="ArgumentOutOfRangeException">The <paramref name="capacity"/> cannot be a negative number.</exception>
		public MutableString (int capacity)
		{
			if(capacity < 0)
				throw new ArgumentOutOfRangeException("capacity", "The capacity cannot be a negative number.");

			_chars = new char[capacity];
		}

		/// <summary>
		/// Creates a mutable string from an existing string
		/// </summary>
		/// <param name="source">Existing string</param>
		/// <exception cref="ArgumentNullException">The existing string can't be null.</exception>
		public MutableString (string source)
		{
			if(source == null)
				throw new ArgumentNullException("source");

			_chars = source.ToCharArray();
			Length = _chars.Length;
		}

		/// <summary>
		/// Creates a mutable string from a set of characters
		/// </summary>
		/// <param name="chars">Initial characters</param>
		/// <exception cref="ArgumentNullException">The initial characters can't be null.</exception>
		public MutableString (IList<char> chars)
		{
			if(chars == null)
				throw new ArgumentNullException("chars");

			Length = chars.Count;
			_chars = copyCharArray(chars);
		}

		/// <summary>
		/// Copies an existing mutable string
		/// </summary>
		/// <param name="str">String to copy from</param>
		/// <exception cref="ArgumentNullException">The string to copy from can't be null.</exception>
		public MutableString (MutableString str)
		{
			if(str == null)
				throw new ArgumentNullException("str");

			Length = str.Length;
			_chars = copyCharArray(str._chars);
		}
		#endregion

		#region Operations

		/// <summary>
		/// Appends one or more items to the string, expanding the capacity as needed
		/// </summary>
		/// <param name="items">Collection of items to append to the string</param>
		/// <remarks><see cref="Object.ToString"/> is called for every item in <paramref name="items"/></remarks>
		public void Append (params object[] items)
		{
			if(items == null)
				throw new ArgumentNullException("items");

			// Get the string representation of each item
			var strings = new string[items.Length];
			var size = 0;
			for(var i = 0; i < items.Length; ++i)
			{
				var str = items[i].ToString();
				strings[i] = str;
				size += str.Length;
			}

			var newLength = Length + size;
			if(newLength > Capacity)
			{// Capacity is too small, extend the array
				var mult = newLength / DefaultCapacity;
				if(newLength % DefaultCapacity == 0)
					++mult; // Allow extra padding (planning ahead for more appending)
				var newCapacity = DefaultCapacity * mult;
				_chars = resizeArray(_chars, newCapacity, Length);
			}

			// Append each string
			for(int i = 0, j = Length; i < strings.Length; ++i)
			{
				var str = strings[i];
				for(var k = 0; k < str.Length; ++j, ++k)
					_chars[j] = str[k];
			}
		}

		/// <summary>
		/// Appends a set of items joined together by a formatting template
		/// </summary>
		/// <param name="format">String formatting template</param>
		/// <param name="items">Collection of items to append to the string</param>
		/// <exception cref="ArgumentNullException">The formatting template can't be null.</exception>
		/// <remarks><see cref="Object.ToString"/> is called for every item in <paramref name="items"/></remarks>
		public void AppendFormat (string format, params object[] items)
		{
			if(format == null)
				throw new ArgumentNullException("format");
			if(items == null)
				throw new ArgumentNullException("items");

			var str = String.Format(format, items);
			Append(str);
		}
		#endregion

		/// <summary>
		/// Returns an enumerator that iterates through the characters in the string
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the characters in the string</returns>
		public IEnumerator<char> GetEnumerator ()
		{
			for(var i = 0; i < Length; ++i)
				yield return _chars[i];
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
			if(obj == null)
				return false;

			// Compare as mutable string
			var other = obj as MutableString;
			if(other != null)
				return equals(other);

			// Compare as regular string
			var str = obj as string;
			if(str != null)
				return Equals(str);

			return false;
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>True if the current object is equal to the <paramref name="other"/> parameter; otherwise, false</returns>
		public bool Equals (string other)
		{
			if(other == null)
				return false;

			if(Length != other.Length)
				return false;

			for(var i = 0; i < Length; ++i)
				if(_chars[i] != other[i])
					return false;

			return true;
		}

		/// <summary>
		/// Determines whether another mutable string is equal to the current instance
		/// </summary>
		/// <param name="other">String to compare against</param>
		/// <returns>True if the mutable strings' contents are equal</returns>
		private bool equals (MutableString other)
		{
			if(other == null)
				return false;

			if(Length != other.Length)
				return false;

			for(var i = 0; i < Length; ++i)
				if(_chars[i] != other._chars[i])
					return false;

			return true;
		}

		/// <summary>
		/// Hashes the contents of the string
		/// </summary>
		/// <returns>A hash code for the current string</returns>
		public override int GetHashCode ()
		{
			unchecked
			{
				var hash = 17;
				for(var i = 0; i < Length; ++i)
					hash = hash * 23 + _chars[i];
				return hash;
			}
		}

		/// <summary>
		/// Determines if the left and right strings are equal
		/// </summary>
		/// <param name="left">First string to compare</param>
		/// <param name="right">Second string to compare</param>
		/// <returns>True if the <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		public static bool operator == (MutableString left, MutableString right)
		{
			if(ReferenceEquals(left, right))
				return true;

			return !ReferenceEquals(left, null) && left.equals(right);
		}

		/// <summary>
		/// Determines if the left and right strings are not equal
		/// </summary>
		/// <param name="left">First string to compare</param>
		/// <param name="right">Second string to compare</param>
		/// <returns>True if the <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		public static bool operator != (MutableString left, MutableString right)
		{
			if(ReferenceEquals(left, right))
				return false;

			return ReferenceEquals(left, null) || !left.equals(right);
		}

		/// <summary>
		/// Compares the current mutable string with another string
		/// </summary>
		/// <param name="other">A string to compare with this object.</param>
		/// <returns>A value that indicates the relative order of the string being compared.
		/// The return value has the following meanings:<list type="unordered">
		/// <item>Less than zero - this object is less than the <paramref name="other"/> parameter</item>
		/// <item>Zero - this object is equal to <paramref name="other"/></item>
		/// <item>Greater than zero - this object is greater than <paramref name="other"/></item>
		/// </list></returns>
		public int CompareTo (string other)
		{
			if(other == null)
				return 1; // null appears before non-null

			for(var i = 0; i < Length && i < other.Length; ++i)
			{
				var left  = _chars[i];
				var right = other[i];
				if(left != right)
					return left.CompareTo(right);
			}

			if(Length == other.Length)
				return 0;
			return (Length < other.Length) ? -1 : 1;
		}

		/// <summary>
		/// Creates a non-mutable string
		/// </summary>
		/// <returns>Regular string</returns>
		public override string ToString ()
		{
			var chars = resizeArray(_chars, Length, Length);
			return new string(chars);
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

		#region Utility methods

		/// <summary>
		/// Copies an array of characters
		/// </summary>
		/// <param name="source">Source character array</param>
		/// <returns>Copy of the character array</returns>
		private static char[] copyCharArray (IList<char> source)
		{
			var dest = new char[source.Count];
			for(var i = 0; i < dest.Length; ++i)
				dest[i] = source[i];
			return dest;
		}

		/// <summary>
		/// Creates a new array containing the contents of an existing array
		/// </summary>
		/// <param name="original">Original array</param>
		/// <param name="newSize">Size to resize the array to</param>
		/// <param name="count">Number of items to copy (default is to copy all items)</param>
		/// <returns>Resized array</returns>
		private static char[] resizeArray (char[] original, int newSize, int count = -1)
		{
			if(count < 0)
				count = original.Length; // Default to original size

			if(newSize == count)
				return original; // Don't do anything - same size

			var newArray = new char[newSize];
			var limit = newSize < count ? newSize : count;
			for(var i = 0; i < limit; ++i)
				newArray[i] = original[i];

			return newArray;
		}
		#endregion
	}
}
