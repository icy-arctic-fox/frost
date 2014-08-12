using Frost.Geometry;
using SFML.Graphics;

namespace Frost.Graphics
{
	/// <summary>
	/// Pen that produces a solid, single-color line
	/// </summary>
	public class SolidPen : Pen
	{
		private readonly Color _color;
		private readonly float _thickness;

		/// <summary>
		/// Color of the line
		/// </summary>
		public Color Color
		{
			get { return _color; }
		}

		/// <summary>
		/// Width of the line in pixels
		/// </summary>
		public float Thickness
		{
			get { return _thickness; }
		}

		/// <summary>
		/// Creates a pen
		/// </summary>
		/// <param name="color">Color of the pen</param>
		/// <param name="thickness">Width of the line in pixels</param>
		public SolidPen (Color color, float thickness)
		{
			_color = color;
			_thickness = thickness;
		}

		/// <summary>
		/// Creates the vertices needed to represent the line
		/// </summary>
		/// <param name="v">Length and rotation of the line</param>
		/// <returns>A collection of vertices that can be used to draw the line</returns>
		/// <remarks>The pen's implementation for this method should not rotate the vertices.
		/// Rotation is done by the calling method.
		/// A rotation value is provided for cases where the pen type might want to adjust the appearance based on rotation.</remarks>
		internal override VertexArray ConsructLineVertices (Vector2 v)
		{
			throw new System.NotImplementedException();
		}
	}
}
