using Frost.Geometry;
using SFML.Graphics;

namespace Frost.Graphics
{
	/// <summary>
	/// Information about a line's appearance
	/// </summary>
	public abstract class Pen
	{
		/// <summary>
		/// Default pen type
		/// </summary>
		public static readonly Pen Default = new SolidPen(new Color(), 1f);

		/// <summary>
		/// Creates the vertices needed to represent the line
		/// </summary>
		/// <param name="v">Length and rotation of the line</param>
		/// <returns>A collection of vertices that can be used to draw the line</returns>
		/// <remarks>The pen's implementation for this method should not rotate the vertices.
		/// Rotation is done by the calling method.
		/// A rotation value is provided for cases where the pen type might want to adjust the appearance based on rotation.</remarks>
		internal abstract VertexArray ConsructLineVertices (Vector2 v);
	}
}
