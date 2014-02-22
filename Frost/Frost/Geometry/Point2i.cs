using System;
using System.Drawing;
using SFML.Window;

namespace Frost.Geometry
{
	/// <summary>
	/// An two-dimensional point with integer values
	/// </summary>
	public struct Point2i
	{
		/// <summary>
		/// Point at (0, 0)
		/// </summary>
		public static readonly Point2i Origin = new Point2i(0, 0);

		private readonly int _x, _y;

		/// <summary>
		/// Offset along the x-axis
		/// </summary>
		public int X
		{
			get { return _x; }
		}

		/// <summary>
		/// Offset along the y-axis
		/// </summary>
		public int Y
		{
			get { return _y; }
		}

		/// <summary>
		/// Creates a new point
		/// </summary>
		/// <param name="x">Offset along the x-axis</param>
		/// <param name="y">Offset along the y-axis</param>
		public Point2i (int x, int y)
		{
			_x = x;
			_y = y;
		}

		/// <summary>
		/// Calculates the distance to another point
		/// </summary>
		/// <param name="point">Other point</param>
		/// <returns>Distance between the two points</returns>
		public double DistanceTo (Point2i point)
		{
			return DistanceTo(point.X, point.Y);
		}

		/// <summary>
		/// Calculates the distance to another point
		/// </summary>
		/// <param name="x">X-coordinate of the other point</param>
		/// <param name="y">Y-coordinate of the other point</param>
		/// <returns>Distance between the two points</returns>
		public double DistanceTo (int x, int y)
		{
			var xDist = _x - x;
			var yDist = _y - y;
			return Math.Sqrt((xDist * xDist) + (yDist * yDist));
		}

		#region Operators
		#region Point2i
		#region Equality

		/// <summary>
		/// Compares two points to determine if they're equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The points are considered equal if their <see cref="X"/> and <see cref="Y"/> values are the same.</remarks>
		public static bool operator == (Point2i left, Point2i right)
		{
			return (left._x == right._x) && (left._y == right._y);
		}

		/// <summary>
		/// Compares two points to determine if they're not equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The points are considered not equal if their <see cref="X"/> or <see cref="Y"/> values are different.</remarks>
		public static bool operator != (Point2i left, Point2i right)
		{
			return (left._x != right._x) || (left._y != right._y);
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
		/// Compares two points to determine if they're equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The points are considered equal if their <see cref="X"/> and <see cref="Y"/> values are the same.</remarks>
		public static bool operator == (Point2i left, Vector2i right)
		{
			return (left._x == right.X) && (left._y == right.Y);
		}

		/// <summary>
		/// Compares two points to determine if they're not equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The points are considered not equal if their <see cref="X"/> or <see cref="Y"/> values are different.</remarks>
		public static bool operator != (Point2i left, Vector2i right)
		{
			return (left._x != right.X) || (left._y != right.Y);
		}

		/// <summary>
		/// Compares two points to determine if they're equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The points are considered equal if their <see cref="X"/> and <see cref="Y"/> values are the same.</remarks>
		public static bool operator == (Vector2i left, Point2i right)
		{
			return (left.X == right._x) && (left.Y == right._y);
		}

		/// <summary>
		/// Compares two points to determine if they're not equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The points are considered not equal if their <see cref="X"/> or <see cref="Y"/> values are different.</remarks>
		public static bool operator != (Vector2i left, Point2i right)
		{
			return (left.X != right._x) || (left.Y != right._y);
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
		/// Compares two points to determine if they're equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The points are considered equal if their <see cref="X"/> and <see cref="Y"/> values are the same.</remarks>
		public static bool operator == (Point2i left, Point right)
		{
			return (left._x == right.X) && (left._y == right.Y);
		}

		/// <summary>
		/// Compares two points to determine if they're not equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The points are considered not equal if their <see cref="X"/> or <see cref="Y"/> values are different.</remarks>
		public static bool operator != (Point2i left, Point right)
		{
			return (left._x != right.X) || (left._y != right.Y);
		}

		/// <summary>
		/// Compares two points to determine if they're equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The points are considered equal if their <see cref="X"/> and <see cref="Y"/> values are the same.</remarks>
		public static bool operator == (Point left, Point2i right)
		{
			return (left.X == right._x) && (left.Y == right._y);
		}

		/// <summary>
		/// Compares two points to determine if they're not equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The points are considered not equal if their <see cref="X"/> or <see cref="Y"/> values are different.</remarks>
		public static bool operator != (Point left, Point2i right)
		{
			return (left.X != right._x) || (left.Y != right._y);
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
		/// Converts a <see cref="Point2i"/> to a <see cref="Vector2i"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>An SFML 2D <see cref="Vector2i"/></returns>
		public static implicit operator Vector2i (Point2i point)
		{
			return new Vector2i(point._x, point._y);
		}

		/// <summary>
		/// Converts a <see cref="Vector2i"/> to a <see cref="Point2i"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>A Frost 2D <see cref="Point2i"/></returns>
		public static implicit operator Point2i (Vector2i point)
		{
			return new Point2i(point.X, point.Y);
		}

		/// <summary>
		/// Converts a <see cref="Point2i"/> to a <see cref="Point"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>A .NET 2D <see cref="Point"/></returns>
		public static implicit operator Point (Point2i point)
		{
			return new Point(point._x, point._y);
		}

		/// <summary>
		/// Converts a <see cref="Point"/> to a <see cref="Point2i"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>A Frost 2D <see cref="Point2i"/></returns>
		public static implicit operator Point2i (Point point)
		{
			return new Point2i(point.X, point.Y);
		}
		#endregion
		#endregion

		/// <summary>
		/// Generates a string representation of the point
		/// </summary>
		/// <returns>A string in the form: (X, Y)</returns>
		public override string ToString ()
		{
			return String.Format("({0}, {1})", _x, _y);
		}

		/// <summary>
		/// Determines if the point is equal to another object
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if <paramref name="obj"/> is considered the same</returns>
		/// <remarks><paramref name="obj"/> is considered the same if it is a <see cref="Point2i"/>, <see cref="Vector2i"/>, or <see cref="Point"/> with the same <see cref="X"/> and <see cref="Y"/> values.</remarks>
		public override bool Equals (object obj)
		{
			if(obj is Point2i)
				return this == (Point2i)obj;
			if(obj is Vector2i)
				return this == (Vector2i)obj;
			if(obj is Point)
				return this == (Point)obj;
			return false;
		}

		/// <summary>
		/// Generates a hash from the values in the point
		/// </summary>
		/// <returns>A hash code</returns>
		public override int GetHashCode ()
		{
			var hash = 17;
			hash = hash * 31 + _x;
			hash = hash * 31 + _y;
			return hash;
		}
	}
}
