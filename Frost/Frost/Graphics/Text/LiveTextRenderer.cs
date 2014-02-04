using System;
using SFML.Graphics;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Draws live text onto the screen.
	/// Rendering time for text is reduced by drawing to an off-screen texture
	/// and then copying it to an on-screen texture.
	/// </summary>
	public abstract class LiveTextRenderer : ITextRenderer
	{
		/// <summary>
		/// Underlying texture used to draw text on
		/// </summary>
		private RenderTexture _texture;

		/// <summary>
		/// Underlying texture to draw the text onto when preparing
		/// </summary>
		protected RenderTarget Texture
		{
			get { return _texture; }
		}

		/// <summary>
		/// Indicates whether the text has been prepared (rendered) internally so that is is ready to be quickly drawn.
		/// </summary>
		public bool Prepared
		{
			get { return _texture != null; }
		}

		/// <summary>
		/// Prepares the underlying texture to be drawn on.
		/// This must be called as part of the <see cref="Prepare"/> process.
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		protected void PrepareTexture (uint width, uint height)
		{
			if(_texture != null)
				_texture.Dispose();
			_texture = new RenderTexture(width, height);
		}

		/// <summary>
		/// Prepares the text for drawing.
		/// This method renders the text internally so that it is ready to be quickly drawn.
		/// </summary>
		public abstract void Prepare ();

		/// <summary>
		/// Draws the text onto a texture at a given position
		/// </summary>
		/// <param name="target">Object to draw the text onto</param>
		/// <param name="x">X-offset at which to draw the text</param>
		/// <param name="y">Y-offset at which to draw the text</param>
		/// <remarks>If the text hasn't been prepared by <see cref="Prepare"/> prior to calling this method,
		/// <see cref="Prepare"/> will be called before drawing the text.</remarks>
		public void Draw (IRenderTarget target, int x = 0, int y = 0)
		{
			if(!Prepared)
			{
				Prepare();
#if DEBUG
				if(!Prepared)
					throw new ApplicationException("The text renderer implementation did not prepare the underlying texture.");
#endif
			}

			// TODO: Copy data from _texture onto target
			throw new NotImplementedException();
		}
	}
}
