using System;
using Frost.Geometry;
using SFML.Graphics;

namespace Frost.Graphics
{
	/// <summary>
	/// Bitmap capable of drawing various shapes and objects onto
	/// </summary>
	public class Canvas // TODO: Implement IRenderTarget
	{
		private readonly RenderTexture _texture;

		/// <summary>
		/// Pen used to draw lines
		/// </summary>
		/// <remarks>A "null" pen means no line will be drawn.</remarks>
		public Pen Pen { get; set; }

		/// <summary>
		/// Creates a new canvas
		/// </summary>
		/// <param name="width">Width of the drawable region in pixels</param>
		/// <param name="height">Height of the drawable region in pixels</param>
		public Canvas (uint width, uint height)
		{
			_texture = new RenderTexture(width, height);
			Pen      = Pen.Default;
		}

		#region Drawing methods

		/// <summary>
		/// Draws a line between two points using the current pen
		/// </summary>
		/// <param name="start">First point to draw from</param>
		/// <param name="end">Second point to draw to</param>
		public void DrawLine (Point2f start, Point2f end)
		{
			DrawLine(start.X, start.Y, end.X, end.Y);
		}

		/// <summary>
		/// Draws a line between to points using the current pen
		/// </summary>
		/// <param name="startX">X-coordinate of the first point to draw from</param>
		/// <param name="startY">Y-coordinate of the first point to draw from</param>
		/// <param name="endX">X-coordinate of the second point to draw to</param>
		/// <param name="endY">Y-coordinate of the second point to draw to</param>
		public void DrawLine (float startX, float startY, float endX, float endY)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws a multiple lines between points using the current pen
		/// </summary>
		/// <param name="points">Set of ordered points to draw lines between</param>
		public void DrawLines (Point2f[] points)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws a rectangle with the current pen and brush
		/// </summary>
		/// <param name="bounds">Size and position of the rectangle</param>
		public void DrawRect (Rect2f bounds)
		{
			DrawRect(bounds.Left, bounds.Top, bounds.Width, bounds.Height);
		}

		/// <summary>
		/// Draws a rectangle with the current pen and brush
		/// </summary>
		/// <param name="x">X-coordinate of the top-left corner of the rectangle</param>
		/// <param name="y">Y-coordinate of the top-left corner of the rectangle</param>
		/// <param name="width">Width of the rectangle</param>
		/// <param name="height">Height of the rectangle</param>
		public void DrawRect (float x, float y, float width, float height)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws a rectangle with rounded corners using the current pen and brush
		/// </summary>
		/// <param name="bounds">Size and position of the rectangle</param>
		/// <param name="radius">Radius of the rounded corners</param>
		/// <param name="precision">Percentage, from 0 to 1, indicating the smoothness of the rounded corners (higher is better quality, but slower)</param>
		public void DrawRoundRect (Rect2f bounds, float radius, float precision = 1f)
		{
			DrawRoundRect(bounds.Left, bounds.Top, bounds.Width, bounds.Height, radius, precision);
		}

		/// <summary>
		/// Draws a rectangle with rounded corners using the current pen and brush
		/// </summary>
		/// <param name="x">X-coordinate of the top-left corner of the rectangle</param>
		/// <param name="y">Y-coordinate of the top-left corner of the rectangle</param>
		/// <param name="width">Width of the rectangle</param>
		/// <param name="height">Height of the rectangle</param>
		/// <param name="radius">Radius of the rounded corners</param>
		/// <param name="precision">Percentage, from 0 to 1, indicating the smoothness of the rounded corners (higher is better quality, but slower)</param>
		public void DrawRoundRect (float x, float y, float width, float height, float radius, float precision = 1f)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws a circle using the current pen and brush
		/// </summary>
		/// <param name="center">Position of the center of the circle</param>
		/// <param name="radius">Distance from the center to the edge</param>
		/// <param name="precision">Percentage, from 0 to 1, indicating the smoothness of the circle (higher is better quality, but slower)</param>
		public void DrawCirlce (Point2f center, float radius, float precision = 1f)
		{
			DrawCircle(center.X, center.Y, radius, precision);
		}

		/// <summary>
		/// Draws a circle using the current pen and brush
		/// </summary>
		/// <param name="x">X-coordinate of the center of the circle</param>
		/// <param name="y">Y-coordinate of the center of the circle</param>
		/// <param name="radius">Distance from the center to the edge</param>
		/// <param name="precision">Percentage, from 0 to 1, indicating the smoothness of the circle (higher is better quality, but slower)</param>
		public void DrawCircle (float x, float y, float radius, float precision = 1f)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws an ellipse using the current pen and brush
		/// </summary>
		/// <param name="center">Position of the center of the ellipse</param>
		/// <param name="radii">Horizontal and vertical distance from the center to the corresponding edge</param>
		/// <param name="precision">Percentage, from 0 to 1, indicating the smoothness of the ellipse (higher is better quality, but slower)</param>
		public void DrawEllipse (Point2f center, Point2f radii, float precision = 1f)
		{
			DrawEllipse(center.X, center.Y, radii.X, radii.Y, precision);
		}

		/// <summary>
		/// Draws an ellipse using the current pen and brush
		/// </summary>
		/// <param name="x">X-coordinate of the center of the ellipse</param>
		/// <param name="y">Y-coordinate of the center of the ellipse</param>
		/// <param name="radiusX">Horizontal distance from the center to the edge</param>
		/// <param name="radiusY">Vertical distance from the center to the edge</param>
		/// <param name="precision">Percentage, from 0 to 1, indicating the smoothness of the ellipse (higher is better quality, but slower)</param>
		public void DrawEllipse (float x, float y, float radiusX, float radiusY, float precision = 1f)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws an n-sided shape with equal side lengths using the current pen and brush
		/// </summary>
		/// <param name="center">Position of the center of the shape</param>
		/// <param name="radius">Distance from the center to an edge</param>
		/// <param name="sides">Number of sides on the shape</param>
		public void DrawNGon (Point2f center, float radius, uint sides)
		{
			DrawNGon(center.X, center.Y, radius, sides);
		}

		/// <summary>
		/// Draws an n-sided shape with equal side lengths using the current pen and brush
		/// </summary>
		/// <param name="x">X-coordinate of the center of the shape</param>
		/// <param name="y">Y-coordinate of the center of the shape</param>
		/// <param name="radius">Distance from the center to an edge</param>
		/// <param name="sides">Number of sides on the shape</param>
		public void DrawNGon (float x, float y, float radius, uint sides)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws an enclosed polygon using the current pen and brush
		/// </summary>
		/// <param name="points">Points to draw lines between</param>
		/// <remarks>A line will be drawn from the last point in <paramref name="points"/> to the first point in <paramref name="points"/> to make an enclosed shape.</remarks>
		public void DrawPolygon (Point2f[] points)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
