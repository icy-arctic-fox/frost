using System;
using System.Drawing;
using SFML.Graphics;

namespace Frost.Geometry
{
	/// <summary>
	/// Rectangular bounds with floating-point values for a 2D object
	/// </summary>
	public struct Rect2f
	{
		/// <summary>
		/// Rectangle at (0, 0) with a width and height of 0
		/// </summary>
		public static readonly Rect2f Empty = new Rect2f(0, 0, 0, 0);

		private readonly float _x, _y, _w, _h;

		/// <summary>
		/// Horizontal position of the left bound
		/// </summary>
		public float Left
		{
			get { return _x;}
		}

		/// <summary>
		/// Horizontal position of the right bound
		/// </summary>
		public float Right
		{
			get { return _x + _w; }
		}

		/// <summary>
		/// Vertical position of the top bound
		/// </summary>
		public float Top
		{
			get { return _y; }
		}

		/// <summary>
		/// Vertical position of the bottom bound
		/// </summary>
		public float Bottom
		{
			get { return _y + _h; }
		}

		/// <summary>
		/// Horizontal length of the rectangular region
		/// </summary>
		public float Width
		{
			get { return _w; }
		}

		/// <summary>
		/// Vertical length of the rectangular region
		/// </summary>
		public float Height
		{
			get { return _h; }
		}

		/// <summary>
		/// Position of the top-left corner of the rectangle
		/// </summary>
		public Point2f TopLeft
		{
			get { return new Point2f(_x, _y); }
		}

		/// <summary>
		/// Position of the top-right corner of the rectangle
		/// </summary>
		public Point2f TopRight
		{
			get { return new Point2f(_x + _w, _y); }
		}

		/// <summary>
		/// Position of the bottom-left corner of the rectangle
		/// </summary>
		public Point2f BottomLeft
		{
			get { return new Point2f(_x, _y + _h); }
		}

		/// <summary>
		/// Position of the bottom-right corner of the rectangle
		/// </summary>
		public Point2f BottomRight
		{
			get { return new Point2f(_x + _w, _y + _h); }
		}

		/// <summary>
		/// Indicates whether the rectangular region has no area
		/// </summary>
		public bool IsEmpty
		{
			get { return Math.Abs(_h) < Single.Epsilon || Math.Abs(_w) < Single.Epsilon; }
		}

		/// <summary>
		/// Creates a new rectangle
		/// </summary>
		/// <param name="x">Offset along the x-axis</param>
		/// <param name="y">Offset along the y-axis</param>
		/// <param name="width">Width of the rectangular region</param>
		/// <param name="height">Height of the rectangular region</param>
		public Rect2f (float x, float y, float width, float height)
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
		public Rect2f (Point2f p, float width, float height)
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
		public Rect2f (Point2f topLeft, Point2f bottomRight)
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
		public bool Contains (Point2f point)
		{
			return (_x <= point.X && _x + _w >= point.X) && (_y <= point.Y && _y + _h >= point.Y);
		}

		/// <summary>
		/// Checks if a another rectangle intersects
		/// </summary>
		/// <param name="rect">Rectangle to check for intersection against</param>
		/// <returns>True if the rectangle overlaps with <paramref name="rect"/></returns>
		public bool Intersects (Rect2f rect)
		{
			return Contains(rect.TopLeft) || Contains(rect.TopRight) || Contains(rect.BottomLeft) || Contains(rect.BottomRight);
		}

		/// <summary>
		/// Gets the bounds of an overlapping region between two rectangular regions
		/// </summary>
		/// <param name="rect">Rectangle that overlaps</param>
		/// <returns>Rectangular bounds for the overlapping region</returns>
		/// <remarks><see cref="IsEmpty"/> will be true for the returned rectangle if the rectangles don't overlap each other.</remarks>
		public Rect2f Intersection (Rect2f rect)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Increases the bounds of the rectangle outward by a specified amount
		/// </summary>
		/// <param name="amount">Amount to inflate the rectangle by on each side</param>
		/// <returns>An inflated rectangle</returns>
		public Rect2f Inflate (float amount)
		{
			var x = _x - amount;
			var y = _y - amount;
			var w = _w + amount + amount;
			var h = _h + amount + amount;
			return new Rect2f(x, y, w, h);
		}

		/// <summary>
		/// Decreases the bounds of the rectangle inward by a specified amount
		/// </summary>
		/// <param name="amount">Amount to deflate the rectangle by on each side</param>
		/// <returns>A deflated rectangle</returns>
		public Rect2f Deflate (float amount)
		{
			var x = _x + amount;
			var y = _y + amount;
			var w = _w - amount - amount;
			var h = _h - amount - amount;
			return new Rect2f(x, y, w, h);
		}

		#region Implicit conversions

		/// <summary>
		/// Converts a <see cref="Rect2f"/> to a <see cref="FloatRect"/>
		/// </summary>
		/// <param name="rect">Rectangle to convert</param>
		/// <returns>An SFML 2D <see cref="FloatRect"/></returns>
		public static implicit operator FloatRect (Rect2f rect)
		{
			return new FloatRect(rect._x, rect._y, rect._w, rect._h);
		}

		/// <summary>
		/// Converts a <see cref="FloatRect"/> to a <see cref="Rect2f"/>
		/// </summary>
		/// <param name="rect">Rectangle to convert</param>
		/// <returns>A Frost 2D <see cref="Rect2f"/></returns>
		public static implicit operator Rect2f (FloatRect rect)
		{
			return new Rect2f(rect.Left, rect.Top, rect.Width, rect.Height);
		}

		/// <summary>
		/// Converts a <see cref="Rect2f"/> to a <see cref="RectangleF"/>
		/// </summary>
		/// <param name="rect">Rectangle to convert</param>
		/// <returns>A .NET 2D <see cref="RectangleF"/></returns>
		public static implicit operator RectangleF (Rect2f rect)
		{
			return new RectangleF(rect._x, rect._y, rect._w, rect._h);
		}

		/// <summary>
		/// Converts a <see cref="RectangleF"/> to a <see cref="Rect2f"/>
		/// </summary>
		/// <param name="rect">Rectangle to convert</param>
		/// <returns>A Frost 2D <see cref="Rect2f"/></returns>
		public static implicit operator Rect2f (RectangleF rect)
		{
			return new Rect2f(rect.Left, rect.Top, rect.Width, rect.Height);
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
