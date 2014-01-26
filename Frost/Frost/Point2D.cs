using System;

namespace Frost
{
	/// <summary>
	/// An two-dimension point
	/// </summary>
	public struct Point2D
	{
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
			var xDist = X - point.X;
			var yDist = Y - point.Y;
			return Math.Sqrt((xDist * xDist) + (yDist + yDist));
		}
	}
}
