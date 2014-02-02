using System;
using SFML.Graphics;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Draws text onto the screen.
	/// Rendering time for text is reduced by drawing to an off-screen texture
	/// and then copying it to an on-screen texture.
	/// </summary>
	public abstract class TextRenderer
	{
		/// <summary>
		/// Underlying texture used to draw text on
		/// </summary>
		private RenderTexture _texture;

		/// <summary>
		/// Prepares the text for drawing.
		/// This method renders the text internally so that it is ready to be quickly drawn.
		/// </summary>
		public void Prepare ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the text onto a texture
		/// </summary>
		/// <param name="target">Object to draw the text onto</param>
		public void Draw (IRenderTarget target)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the text onto a texture at a given position
		/// </summary>
		/// <param name="target">Object to draw the text onto</param>
		/// <param name="x">X-offset at which to draw the text</param>
		/// <param name="y">Y-offset at which to draw the text</param>
		public void Draw (IRenderTarget target, int x, int y)
		{
			throw new NotImplementedException();
		}
	}
}
