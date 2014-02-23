using System;
using System.Drawing;
using SFML.Window;

namespace Frost.Geometry
{
	/// <summary>
	/// An two-dimensional point with floating-point values
	/// </summary>
	public struct Point2f
	{
		/// <summary>
		/// Point at (0, 0)
		/// </summary>
		public static readonly Point2f Origin = new Point2f(0, 0);

		private readonly float _x, _y;

		/// <summary>
		/// Offset along the x-axis
		/// </summary>
		public float X
		{
			get { return _x; }
		}

		/// <summary>
		/// Offset along the y-axis
		/// </summary>
		public float Y
		{
			get { return _y; }
		}

		/// <summary>
		/// Creates a new point
		/// </summary>
		/// <param name="x">Offset along the x-axis</param>
		/// <param name="y">Offset along the y-axis</param>
		public Point2f (float x, float y)
		{
			_x = x;
			_y = y;
		}

		/// <summary>
		/// Calculates the distance to another point
		/// </summary>
		/// <param name="point">Other point</param>
		/// <returns>Distance between the two points</returns>
		public double DistanceTo (Point2f point)
		{
			return DistanceTo(point.X, point.Y);
		}

		/// <summary>
		/// Calculates the distance to another point
		/// </summary>
		/// <param name="x">X-coordinate of the other point</param>
		/// <param name="y">Y-coordinate of the other point</param>
		/// <returns>Distance between the two points</returns>
		public double DistanceTo (float x, float y)
		{
			var xDist = _x - x;
			var yDist = _y - y;
			return Math.Sqrt((xDist * xDist) + (yDist * yDist));
		}

		#region Operators
		#region Point2f
		#region Equality

		/// <summary>
		/// Compares two points to determine if they're equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The points are considered equal if their <see cref="X"/> and <see cref="Y"/> values are the same.</remarks>
		public static bool operator == (Point2f left, Point2f right)
		{
			return (Math.Abs(left._x - right._x) < Single.Epsilon) &&
				(Math.Abs(left._y - right._y) < Single.Epsilon);
		}

		/// <summary>
		/// Compares two points to determine if they're not equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The points are considered not equal if their <see cref="X"/> or <see cref="Y"/> values are different.</remarks>
		public static bool operator != (Point2f left, Point2f right)
		{
			return (Math.Abs(left._x - right._x) > Single.Epsilon) ||
				(Math.Abs(left._y - right._y) > Single.Epsilon);
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

		#region Vector2f
		#region Equality

		/// <summary>
		/// Compares two points to determine if they're equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The points are considered equal if their <see cref="X"/> and <see cref="Y"/> values are the same.</remarks>
		public static bool operator == (Point2f left, Vector2f right)
		{
			return (Math.Abs(left._x - right.X) < Single.Epsilon) &&
				(Math.Abs(left._y - right.Y) < Single.Epsilon);
		}

		/// <summary>
		/// Compares two points to determine if they're not equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The points are considered not equal if their <see cref="X"/> or <see cref="Y"/> values are different.</remarks>
		public static bool operator != (Point2f left, Vector2f right)
		{
			return (Math.Abs(left._x - right.X) > Single.Epsilon) ||
				(Math.Abs(left._y - right.Y) > Single.Epsilon);
		}

		/// <summary>
		/// Compares two points to determine if they're equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The points are considered equal if their <see cref="X"/> and <see cref="Y"/> values are the same.</remarks>
		public static bool operator == (Vector2f left, Point2f right)
		{
			return (Math.Abs(left.X - right._x) < Single.Epsilon) &&
				(Math.Abs(left.Y - right._y) < Single.Epsilon);
		}

		/// <summary>
		/// Compares two points to determine if they're not equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The points are considered not equal if their <see cref="X"/> or <see cref="Y"/> values are different.</remarks>
		public static bool operator != (Vector2f left, Point2f right)
		{
			return (Math.Abs(left.X - right._x) > Single.Epsilon) ||
				(Math.Abs(left.Y - right._y) > Single.Epsilon);
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

		#region PointF
		#region Equality

		/// <summary>
		/// Compares two points to determine if they're equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The points are considered equal if their <see cref="X"/> and <see cref="Y"/> values are the same.</remarks>
		public static bool operator == (Point2f left, PointF right)
		{
			return (Math.Abs(left._x - right.X) < Single.Epsilon) &&
				(Math.Abs(left._y - right.Y) < Single.Epsilon);
		}

		/// <summary>
		/// Compares two points to determine if they're not equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The points are considered not equal if their <see cref="X"/> or <see cref="Y"/> values are different.</remarks>
		public static bool operator != (Point2f left, PointF right)
		{
			return (Math.Abs(left._x - right.X) > Single.Epsilon) ||
				(Math.Abs(left._y - right.Y) > Single.Epsilon);
		}

		/// <summary>
		/// Compares two points to determine if they're equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The points are considered equal if their <see cref="X"/> and <see cref="Y"/> values are the same.</remarks>
		public static bool operator == (PointF left, Point2f right)
		{
			return (Math.Abs(left.X - right._x) < Single.Epsilon) &&
				(Math.Abs(left.Y - right._y) < Single.Epsilon);
		}

		/// <summary>
		/// Compares two points to determine if they're not equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The points are considered not equal if their <see cref="X"/> or <see cref="Y"/> values are different.</remarks>
		public static bool operator != (PointF left, Point2f right)
		{
			return (Math.Abs(left.X - right._x) > Single.Epsilon) ||
				(Math.Abs(left.Y - right._y) > Single.Epsilon);
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
		/// Converts a <see cref="Point2f"/> to a <see cref="Vector2f"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>An SFML 2D <see cref="Vector2f"/></returns>
		public static implicit operator Vector2f (Point2f point)
		{
			return new Vector2f(point._x, point._y);
		}

		/// <summary>
		/// Converts a <see cref="Vector2f"/> to a <see cref="Point2f"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>A Frost 2D <see cref="Point2f"/></returns>
		public static implicit operator Point2f (Vector2f point)
		{
			return new Point2f(point.X, point.Y);
		}

		/// <summary>
		/// Converts a <see cref="Point2f"/> to a <see cref="PointF"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>A .NET 2D <see cref="PointF"/></returns>
		public static implicit operator PointF (Point2f point)
		{
			return new PointF(point._x, point._y);
		}

		/// <summary>
		/// Converts a <see cref="PointF"/> to a <see cref="Point2f"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>A Frost 2D <see cref="Point2f"/></returns>
		public static implicit operator Point2f (PointF point)
		{
			return new Point2f(point.X, point.Y);
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
		/// <remarks><paramref name="obj"/> is considered the same if it is a <see cref="Point2f"/>, <see cref="Vector2f"/>, or <see cref="PointF"/> with the same <see cref="X"/> and <see cref="Y"/> values.</remarks>
		public override bool Equals (object obj)
		{
			if(obj is Point2f)
				return this == (Point2f)obj;
			if(obj is Vector2f)
				return this == (Vector2f)obj;
			if(obj is PointF)
				return this == (PointF)obj;
			return false;
		}

		/// <summary>
		/// Generates a hash from the values in the point
		/// </summary>
		/// <returns>A hash code</returns>
		public override int GetHashCode ()
		{
			var hash = 17;
			hash = hash * 31 + _x.GetHashCode();
			hash = hash * 31 + _y.GetHashCode();
			return hash;
		}
	}
}
