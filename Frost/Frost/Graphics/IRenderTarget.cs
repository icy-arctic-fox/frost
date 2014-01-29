using SFML.Graphics;

namespace Frost.Graphics
{
	/// <summary>
	/// Something that can be drawn to, like a texture (bitmap) or screen
	/// </summary>
	public interface IRenderTarget
	{
		/// <summary>
		/// Draws an object to the target
		/// </summary>
		/// <param name="drawable">Object to draw</param>
		void Draw (Drawable drawable);

		/// <summary>
		/// Draws an object to the target with additional transformation and render options
		/// </summary>
		/// <param name="drawable">Object to draw</param>
		/// <param name="transform">Additional transformation and render options</param>
		void Draw (Drawable drawable, RenderStates transform);
	}
}
