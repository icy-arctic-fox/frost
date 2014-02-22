using System;
using System.Drawing;
using SFML.Graphics;

namespace Frost.Geometry
{
	/// <summary>
	/// Rectangular bounds with integer values for a 2D object
	/// </summary>
	public struct Rect2i
	{
		/// <summary>
		/// Rectangle at (0, 0) with a width and height of 0
		/// </summary>
		public static readonly Rect2i Empty = new Rect2i(0, 0, 0, 0);

		private readonly int _x, _y, _w, _h;

		/// <summary>
		/// Horizontal position of the left bound
		/// </summary>
		public int Left
		{
			get { return _x;}
		}

		/// <summary>
		/// Horizontal position of the right bound
		/// </summary>
		public int Right
		{
			get { return _x + _w; }
		}

		/// <summary>
		/// Vertical position of the top bound
		/// </summary>
		public int Top
		{
			get { return _y; }
		}

		/// <summary>
		/// Vertical position of the bottom bound
		/// </summary>
		public int Bottom
		{
			get { return _y + _h; }
		}

		/// <summary>
		/// Horizontal length of the rectangular region
		/// </summary>
		public int Width
		{
			get { return _w; }
		}

		/// <summary>
		/// Vertical length of the rectangular region
		/// </summary>
		public int Height
		{
			get { return _h; }
		}

		/// <summary>
		/// Creates a new rectangle
		/// </summary>
		/// <param name="x">Offset along the x-axis</param>
		/// <param name="y">Offset along the y-axis</param>
		/// <param name="width">Width of the rectangular region</param>
		/// <param name="height">Height of the rectangular region</param>
		public Rect2i (int x, int y, int width, int height)
		{
			_x = x;
			_y = y;
			_w = width;
			_h = height;
		}

		/// <summary>
		/// Creates a new rectangle
		/// </summary>
		/// <param name="p">Position of the upper-left corner (X, Y)</param>
		/// <param name="width">Width of the rectangular region</param>
		/// <param name="height">Height of the rectangular region</param>
		public Rect2i (Point2i p, int width, int height)
		{
			_x = p.X;
			_y = p.Y;
			_w = width;
			_h = height;
		}

		/// <summary>
		/// Creates a new rectangle
		/// </summary>
		/// <param name="topLeft">Position of the upper-left corner (X, Y)</param>
		/// <param name="bottomRight">Position of the lower-right corner (X, Y)</param>
		public Rect2i (Point2i topLeft, Point2i bottomRight)
		{
			_x = topLeft.X;
			_y = topLeft.Y;
			_w = bottomRight.X - topLeft.X;
			_h = bottomRight.Y - topLeft.Y;
		}

		/// <summary>
		/// Checks if a point is contained within the rectangular bounds
		/// </summary>
		/// <param name="point">Point to look for</param>
		/// <returns>True if <paramref name="point"/> is inside the rectangular bounds</returns>
		public bool Contains (Point2i point)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Checks if a another rectangle intersects
		/// </summary>
		/// <param name="rect">Rectangle to check for intersection against</param>
		/// <returns>True if the rectangle overlaps with <paramref name="rect"/></returns>
		public bool Intersects (Rect2i rect)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the bounds of an overlapping region between two rectangular regions
		/// </summary>
		/// <param name="rect">Rectangle that overlaps</param>
		/// <returns>Rectangular bounds for the overlapping region</returns>
		/// <remarks><see cref="Empty"/> will be returned if the rectangles don't overlap each other.</remarks>
		public Rect2i Intersection (Rect2i rect)
		{
			throw new NotImplementedException();
		}

		#region Implicit conversions

		/// <summary>
		/// Converts a <see cref="Rect2i"/> to a <see cref="IntRect"/>
		/// </summary>
		/// <param name="rect">Rectangle to convert</param>
		/// <returns>An SFML 2D <see cref="IntRect"/></returns>
		public static implicit operator IntRect (Rect2i rect)
		{
			return new IntRect(rect._x, rect._y, rect._w, rect._h);
		}

		/// <summary>
		/// Converts a <see cref="IntRect"/> to a <see cref="Rect2i"/>
		/// </summary>
		/// <param name="rect">Rectangle to convert</param>
		/// <returns>A Frost 2D <see cref="Rect2i"/></returns>
		public static implicit operator Rect2i (IntRect rect)
		{
			return new Rect2i(rect.Left, rect.Top, rect.Width, rect.Height);
		}

		/// <summary>
		/// Converts a <see cref="Rect2i"/> to a <see cref="Rectangle"/>
		/// </summary>
		/// <param name="rect">Rectangle to convert</param>
		/// <returns>A .NET 2D <see cref="Rectangle"/></returns>
		public static implicit operator Rectangle (Rect2i rect)
		{
			return new Rectangle(rect._x, rect._y, rect._w, rect._h);
		}

		/// <summary>
		/// Converts a <see cref="Rectangle"/> to a <see cref="Rect2i"/>
		/// </summary>
		/// <param name="rect">Rectangle to convert</param>
		/// <returns>A Frost 2D <see cref="Rect2i"/></returns>
		public static implicit operator Rect2i (Rectangle rect)
		{
			return new Rect2i(rect.Left, rect.Top, rect.Width, rect.Height);
		}
		#endregion

		/// <summary>
		/// Generates a string representation of the point
		/// </summary>
		/// <returns>A string in the form: (X, Y)[Width, Height]</returns>
		public override string ToString ()
		{
			return String.Format("({0}, {1})[{2}, {3}]", _x, _y, _w, _h);
		}
	}
}
