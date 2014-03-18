using System;
using SFML.Graphics;
using SFML.Window;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Draws live text onto the screen.
	/// Rendering time for text is reduced by drawing to an off-screen texture
	/// and then copying it to an on-screen texture.
	/// </summary>
	public class LiveTextRenderer : TextRenderer
	{
		/// <summary>
		/// Calculates the bounds of the space that the text will occupy
		/// </summary>
		/// <returns>Width and height of the bounds</returns>
		protected override Vector2u CalculateBounds ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the text onto a texture
		/// </summary>
		/// <param name="target">Texture to render to</param>
		protected override void Draw (RenderTexture target)
		{
			throw new NotImplementedException();
		}
	}
}
