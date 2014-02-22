using System;

namespace Frost.Geometry
{
	/// <summary>
	/// An three-dimensional point with integer values
	/// </summary>
	public struct Point3i
	{
		/// <summary>
		/// Point at (0, 0)
		/// </summary>
		public static readonly Point3i Origin = new Point3i(0, 0, 0);

		/// <summary>
		/// Offset along the x-axis
		/// </summary>
		public readonly int X;

		/// <summary>
		/// Offset along the y-axis
		/// </summary>
		public readonly int Y;

		/// <summary>
		/// Offset along the z-axis
		/// </summary>
		public readonly int Z;

		/// <summary>
		/// Creates a new point
		/// </summary>
		/// <param name="x">Offset along the x-axis</param>
		/// <param name="y">Offset along the y-axis</param>
		/// <param name="z">Offset along the z-axis</param>
		public Point3i (int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Calculates the distance to another point
		/// </summary>
		/// <param name="point">Other point</param>
		/// <returns>Distance between the two points</returns>
		public double DistanceTo (Point3i point)
		{
			return DistanceTo(point.X, point.Y, point.Z);
		}

		/// <summary>
		/// Calculates the distance to another point
		/// </summary>
		/// <param name="x">X-coordinate of the other point</param>
		/// <param name="y">Y-coordinate of the other point</param>
		/// <param name="z">Z-coordinate of the other point</param>
		/// <returns>Distance between the two points</returns>
		public double DistanceTo (int x, int y, int z)
		{
			var xDist = X - x;
			var yDist = Y - y;
			var zDist = Z - z;
			return Math.Sqrt((xDist * xDist) + (yDist * yDist) + (zDist * zDist));
		}

		/// <summary>
		/// Generates a string representation of the point
		/// </summary>
		/// <returns>A string in the form: (X, Y, Z)</returns>
		public override string ToString ()
		{
			return String.Format("({0}, {1}, {2})", X, Y, Z);
		}
	}
}
