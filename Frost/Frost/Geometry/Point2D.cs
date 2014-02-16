﻿using System;
using System.Drawing;
using SFML.Window;

namespace Frost.Geometry
{
	/// <summary>
	/// An two-dimensional point with integer values
	/// </summary>
	public struct Point2D
	{
		/// <summary>
		/// Point at (0, 0)
		/// </summary>
		public static readonly Point2D Origin = new Point2D(0, 0);

		/// <summary>
		/// Offset along the x-axis
		/// </summary>
		public readonly int X;

		/// <summary>
		/// Offset along the y-axis
		/// </summary>
		public readonly int Y;

		/// <summary>
		/// Creates a new point
		/// </summary>
		/// <param name="x">Offset along the x-axis</param>
		/// <param name="y">Offset along the y-axis</param>
		public Point2D (int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Calculates the distance to another point
		/// </summary>
		/// <param name="point">Other point</param>
		/// <returns>Distance between the two points</returns>
		public double DistanceTo (Point2D point)
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
			var xDist = X - x;
			var yDist = Y - y;
			return Math.Sqrt((xDist * xDist) + (yDist * yDist));
		}

		#region Implicit conversions

		/// <summary>
		/// Converts a <see cref="Point2D"/> to a <see cref="Vector2i"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>An SFML 2D point</returns>
		public static implicit operator Vector2i (Point2D point)
		{
			return new Vector2i(point.X, point.Y);
		}

		/// <summary>
		/// Converts a <see cref="Vector2i"/> to a <see cref="Point2D"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>A Frost 2D point</returns>
		public static implicit operator Point2D (Vector2i point)
		{
			return new Point2D(point.X, point.Y);
		}

		/// <summary>
		/// Converts a <see cref="Point2D"/> to a <see cref="Point"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>A .NET 2D point</returns>
		public static implicit operator Point (Point2D point)
		{
			return new Point(point.X, point.Y);
		}

		/// <summary>
		/// Converts a <see cref="Point"/> to a <see cref="Point2D"/>
		/// </summary>
		/// <param name="point">Point to convert</param>
		/// <returns>A Frost 2D point</returns>
		public static implicit operator Point2D (Point point)
		{
			return new Point2D(point.X, point.Y);
		}
		#endregion

		/// <summary>
		/// Generates a string representation of the point
		/// </summary>
		/// <returns>A string in the form: (X, Y)</returns>
		public override string ToString ()
		{
			return String.Format("({0}, {1})", X, Y);
		}
	}
}
