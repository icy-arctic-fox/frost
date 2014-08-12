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
	public class Canvas // TODO: Implement IRenderTarget
	{
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
			Pen = Pen.Default;
		}
	}
}
