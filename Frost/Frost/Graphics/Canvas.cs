using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Frost.Graphics
{
	/// <summary>
	/// Bitmap capable of drawing various shapes and objects onto
	/// </summary>
	public class Canvas : IRenderTarget
	{
		private Pen _pen = Pen.Default;

		/// <summary>
		/// Draws an object to the canvas
		/// </summary>
		/// <param name="drawable">Object to draw</param>
		public void Draw (Drawable drawable)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws an object to the canvas with additional transformation and render options
		/// </summary>
		/// <param name="drawable">Object to draw</param>
		/// <param name="transform">Additional transformation and render options</param>
		public void Draw (Drawable drawable, RenderStates transform)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws a collection of vertices with additional transformation and render options
		/// </summary>
		/// <param name="verts">Vertices to draw</param>
		/// <param name="transform">Additional transformation and render options</param>
		public void Draw (VertexArray verts, RenderStates transform)
		{
			throw new NotImplementedException();
		}
	}
}
