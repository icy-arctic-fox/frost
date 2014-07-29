using System;
using System.Drawing;
using SFML.Window;

namespace Frost.Geometry
{
	/// <summary>
	/// A two-dimensional box representing a size
	/// </summary>
	public struct Size2
	{
		/// <summary>
		/// Empty size (0 x 0)
		/// </summary>
		public static readonly Size2 Empty = new Size2(0, 0);

		private readonly uint _w, _h;

		/// <summary>
		/// Horizontal length of the box
		/// </summary>
		public uint Width
		{
			get { return _w; }
		}

		/// <summary>
		/// Vertical length of the box
		/// </summary>
		public uint Height
		{
			get { return _h; }
		}

		/// <summary>
		/// Creates a new size
		/// </summary>
		/// <param name="width">Horizontal length</param>
		/// <param name="height">Vertical length</param>
		public Size2 (uint width, uint height)
		{
			_w = width;
			_h = height;
		}

		#region Operators
		#region Point2
		#region Equality

		/// <summary>
		/// Compares two sizes to determine if they're equal
		/// </summary>
		/// <param name="left">First size to compare</param>
		/// <param name="right">Second size to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The sizes are considered equal if their <see cref="Width"/> and <see cref="Height"/> values are the same.</remarks>
		public static bool operator == (Size2 left, Size2 right)
		{
			return (left._w == right._w) && (left._h == right._h);
		}

		/// <summary>
		/// Compares two sizes to determine if they're not equal
		/// </summary>
		/// <param name="left">First size to compare</param>
		/// <param name="right">Second size to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The sizes are considered not equal if their <see cref="Width"/> or <see cref="Height"/> values are different.</remarks>
		public static bool operator != (Size2 left, Size2 right)
		{
			return (left._w != right._w) || (left._h != right._h);
		}
		#endregion

		#region Addition

		// TODO
		#endregion

		#region Subtraction

		// TODO
		#endregion

		#region Scaling

		// TODO
		#endregion
		#endregion

		#region Vector2i
		#region Equality

		/// <summary>
		/// Compares two sizes to determine if they're equal
		/// </summary>
		/// <param name="left">First size to compare</param>
		/// <param name="right">Second size to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The sizes are considered equal if their <see cref="Width"/> and <see cref="Height"/> values are the same.</remarks>
		public static bool operator == (Size2 left, Vector2u right)
		{
			return (left._w == right.X) && (left._h == right.Y);
		}

		/// <summary>
		/// Compares two sizes to determine if they're not equal
		/// </summary>
		/// <param name="left">First size to compare</param>
		/// <param name="right">Second size to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The sizes are considered not equal if their <see cref="Width"/> or <see cref="Height"/> values are different.</remarks>
		public static bool operator != (Size2 left, Vector2u right)
		{
			return (left._w != right.X) || (left._h != right.Y);
		}

		/// <summary>
		/// Compares two sizes to determine if they're equal
		/// </summary>
		/// <param name="left">First size to compare</param>
		/// <param name="right">Second size to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The sizes are considered equal if their <see cref="Width"/> and <see cref="Height"/> values are the same.</remarks>
		public static bool operator == (Vector2u left, Size2 right)
		{
			return (left.X == right._w) && (left.Y == right._h);
		}

		/// <summary>
		/// Compares two sizes to determine if they're not equal
		/// </summary>
		/// <param name="left">First size to compare</param>
		/// <param name="right">Second size to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The sizes are considered not equal if their <see cref="Width"/> or <see cref="Height"/> values are different.</remarks>
		public static bool operator != (Vector2u left, Size2 right)
		{
			return (left.X != right._w) || (left.Y != right._h);
		}
		#endregion

		#region Addition

		// TODO
		#endregion

		#region Subtraction

		// TODO
		#endregion

		#region Scaling

		// TODO
		#endregion
		#endregion

		#region Point
		#region Equality

		/// <summary>
		/// Compares two sizes to determine if they're equal
		/// </summary>
		/// <param name="left">First size to compare</param>
		/// <param name="right">Second size to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The sizes are considered equal if their <see cref="Width"/> and <see cref="Height"/> values are the same.</remarks>
		public static bool operator == (Size2 left, Size right)
		{
			return (left._w == right.Width) && (left._h == right.Height);
		}

		/// <summary>
		/// Compares two sizes to determine if they're not equal
		/// </summary>
		/// <param name="left">First size to compare</param>
		/// <param name="right">Second size to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The sizes are considered not equal if their <see cref="Width"/> or <see cref="Height"/> values are different.</remarks>
		public static bool operator != (Size2 left, Size right)
		{
			return (left._w != right.Width) || (left._h != right.Height);
		}

		/// <summary>
		/// Compares two sizes to determine if they're equal
		/// </summary>
		/// <param name="left">First size to compare</param>
		/// <param name="right">Second size to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The sizes are considered equal if their <see cref="Width"/> and <see cref="Height"/> values are the same.</remarks>
		public static bool operator == (Size left, Size2 right)
		{
			return (left.Width == right._w) && (left.Height == right._h);
		}

		/// <summary>
		/// Compares two sizes to determine if they're not equal
		/// </summary>
		/// <param name="left">First size to compare</param>
		/// <param name="right">Second size to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The sizes are considered not equal if their <see cref="Width"/> or <see cref="Height"/> values are different.</remarks>
		public static bool operator != (Size left, Size2 right)
		{
			return (left.Width != right._w) || (left.Height != right._h);
		}
		#endregion

		#region Addition

		// TODO
		#endregion

		#region Subtraction

		// TODO
		#endregion

		#region Scaling

		// TODO
		#endregion
		#endregion

		#region Implicit conversions

		/// <summary>
		/// Converts a <see cref="Size2"/> to a <see cref="Vector2u"/>
		/// </summary>
		/// <param name="size">Size to convert</param>
		/// <returns>An SFML 2D <see cref="Vector2u"/></returns>
		public static implicit operator Vector2u (Size2 size)
		{
			return new Vector2u(size._w, size._h);
		}

		/// <summary>
		/// Converts a <see cref="Vector2u"/> to a <see cref="Size2"/>
		/// </summary>
		/// <param name="size">Size to convert</param>
		/// <returns>A Frost 2D <see cref="Size2"/></returns>
		public static implicit operator Size2 (Vector2u size)
		{
			return new Size2(size.X, size.Y);
		}

		/// <summary>
		/// Converts a <see cref="Size2"/> to a <see cref="Size"/>
		/// </summary>
		/// <param name="size">Size to convert</param>
		/// <returns>A .NET 2D <see cref="Size"/></returns>
		public static implicit operator Size (Size2 size)
		{
			return new Size((int)size._w, (int)size._h);
		}

		/// <summary>
		/// Converts a <see cref="Size"/> to a <see cref="Size2"/>
		/// </summary>
		/// <param name="size">Size to convert</param>
		/// <returns>A Frost 2D <see cref="Size2"/></returns>
		public static implicit operator Size2 (Size size)
		{
			return new Size2((uint)size.Width, (uint)size.Height);
		}
		#endregion
		#endregion

		/// <summary>
		/// Generates a string representation of the size
		/// </summary>
		/// <returns>A string in the form: [Width x Height]</returns>
		public override string ToString ()
		{
			return String.Format("[{0} x {1}]", _w, _h);
		}

		/// <summary>
		/// Determines if the size is equal to another object
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if <paramref name="obj"/> is considered the same</returns>
		/// <remarks><paramref name="obj"/> is considered the same if it is a <see cref="Size2"/>, <see cref="Vector2u"/>, or <see cref="Size"/> with the same <see cref="Width"/> and <see cref="Height"/> values.</remarks>
		public override bool Equals (object obj)
		{
			if(obj is Size2)
				return this == (Size2)obj;
			if(obj is Vector2u)
				return this == (Vector2u)obj;
			if(obj is Size)
				return this == (Size)obj;
			return false;
		}

		/// <summary>
		/// Generates a hash from the values in the size
		/// </summary>
		/// <returns>A hash code</returns>
		public override int GetHashCode ()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 31 + (int)_w;
				hash = hash * 31 + (int)_h;
				return hash;
			}
		}
	}
}
