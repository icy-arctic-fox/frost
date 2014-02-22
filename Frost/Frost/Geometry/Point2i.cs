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

		private readonly int _x;

		/// <summary>
		/// Offset along the x-axis
		/// </summary>
		public int X
		{
			get { return _x; }
		}

		private readonly int _y;

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

		/// <summary>
		/// Generates a string representation of the point
		/// </summary>
		/// <returns>A string in the form: (X, Y)</returns>
		public override string ToString ()
		{
			return String.Format("({0}, {1})", _x, _y);
		}
	}
}
