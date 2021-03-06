﻿using System;

namespace Frost.Geometry
{
	/// <summary>
	/// An three-dimensional point with floating-point values
	/// </summary>
	public struct Point3f
	{
		/// <summary>
		/// Point at (0, 0, 0)
		/// </summary>
		public static readonly Point3f Origin = new Point3f(0, 0, 0);

		private readonly float _x, _y, _z;

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
		/// Offset along the z-axis
		/// </summary>
		public float Z
		{
			get { return _z; }
		}

		/// <summary>
		/// Creates a new point
		/// </summary>
		/// <param name="x">Offset along the x-axis</param>
		/// <param name="y">Offset along the y-axis</param>
		/// <param name="z">Offset along the z-axis</param>
		public Point3f (int x, int y, int z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		/// <summary>
		/// Calculates the distance to another point
		/// </summary>
		/// <param name="point">Other point</param>
		/// <returns>Distance between the two points</returns>
		public double DistanceTo (Point3f point)
		{
			return DistanceTo(point._x, point._y, point._z);
		}

		/// <summary>
		/// Calculates the distance to another point
		/// </summary>
		/// <param name="x">X-coordinate of the other point</param>
		/// <param name="y">Y-coordinate of the other point</param>
		/// <param name="z">Z-coordinate of the other point</param>
		/// <returns>Distance between the two points</returns>
		public double DistanceTo (float x, float y, float z)
		{
			var xDist = _x - x;
			var yDist = _y - y;
			var zDist = _z - z;
			return Math.Sqrt((xDist * xDist) + (yDist * yDist) + (zDist * zDist));
		}
		
		#region Operators
		#region Equality

		/// <summary>
		/// Compares two points to determine if they're equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal</returns>
		/// <remarks>The points are considered equal if their <see cref="X"/>, <see cref="Y"/>, and <see cref="Z"/> values are the same.</remarks>
		public static bool operator == (Point3f left, Point3f right)
		{
			return (Math.Abs(left._x - right._x) < Single.Epsilon) &&
				(Math.Abs(left._y - right._y) < Single.Epsilon) &&
				(Math.Abs(left._z - right._z) < Single.Epsilon);
		}

		/// <summary>
		/// Compares two points to determine if they're not equal
		/// </summary>
		/// <param name="left">First point to compare</param>
		/// <param name="right">Second point to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal</returns>
		/// <remarks>The points are considered not equal if their <see cref="X"/>, <see cref="Y"/>, or <see cref="Z"/> values are different.</remarks>
		public static bool operator != (Point3f left, Point3f right)
		{
			return (Math.Abs(left._x - right._x) > Single.Epsilon) ||
				(Math.Abs(left._y - right._y) > Single.Epsilon) ||
				(Math.Abs(left._z - right._z) > Single.Epsilon);
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

		/// <summary>
		/// Generates a string representation of the point
		/// </summary>
		/// <returns>A string in the form: (X, Y, Z)</returns>
		public override string ToString ()
		{
			return String.Format("({0}, {1}, {2})", _x, _y, _z);
		}

		/// <summary>
		/// Determines if the point is equal to another object
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if <paramref name="obj"/> is considered the same</returns>
		/// <remarks><paramref name="obj"/> is considered the same if it is a <see cref="Point3f"/> with the same <see cref="X"/>, <see cref="Y"/>, and <see cref="Z"/> values.</remarks>
		public override bool Equals (object obj)
		{
			if(obj is Point3f)
				return this == (Point3f)obj;
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
			hash = hash * 31 + _z.GetHashCode();
			return hash;
		}
	}
}
