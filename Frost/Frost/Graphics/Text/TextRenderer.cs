using SFML.Graphics;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Draws text onto the screen.
	/// This renderer supports 'LiveText' formatting.
	/// </summary>
	public abstract class TextRenderer
	{
		/// <summary>
		/// Underlying texture used to draw text segments to
		/// </summary>
		private RenderTexture _texture;
	}
}
