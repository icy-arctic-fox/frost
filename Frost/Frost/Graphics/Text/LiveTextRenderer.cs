using System;
using System.Collections.Generic;
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
		/// Text to render
		/// </summary>
		public LiveTextString Text { get; set; }

		/// <summary>
		/// Creates a new live text renderer
		/// </summary>
		/// <param name="appearance">Initial (default) visual appearance of the text</param>
		/// <exception cref="ArgumentNullException">The <paramref name="appearance"/> of the text can't be null.</exception>
		public LiveTextRenderer (TextAppearance appearance)
			: base(appearance)
		{
			// ...
		}

		/// <summary>
		/// Creates a new live text renderer
		/// </summary>
		/// <param name="text">Text to render</param>
		/// <param name="appearance">Initial (default) visual appearance of the text</param>
		/// <exception cref="ArgumentNullException">The <paramref name="appearance"/> of the text can't be null.</exception>
		public LiveTextRenderer (LiveTextString text, TextAppearance appearance)
			: base(appearance)
		{
			Text = text;
		}

		/// <summary>
		/// Calculates the bounds of the space that the text will occupy
		/// </summary>
		/// <returns>Width and height of the bounds</returns>
		protected override Vector2u CalculateBounds ()
		{
			var liveText = Text ?? new LiveTextString(Appearance);
			var appearance = Appearance.CloneTextAppearance();
			return WordWrap
						? calculateWrappedBounds(liveText, MultiLine, WrapWidth, appearance)
						: calculateBounds(liveText, MultiLine, appearance);
		}

		/// <summary>
		/// Calculates the bounds of some live text that does not wrap onto new lines
		/// </summary>
		/// <param name="liveText">Text to calculate the bounds of</param>
		/// <param name="multiLine">Flag indicating whether newlines are allowed</param>
		/// <param name="appearance">Information about the initial (default) appearance of the text</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateBounds (LiveTextString liveText, bool multiLine, TextAppearance appearance)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calculates the bounds of some text that has word wrapping applied to it
		/// </summary>
		/// <param name="liveText">Text to calculate the bounds of</param>
		/// <param name="multiLine">Flag indicating whether the original newlines are allowed</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <param name="appearance">Information about the initial (default) appearance of the text</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateWrappedBounds (LiveTextString liveText, bool multiLine, int width,
														TextAppearance appearance)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the text onto a texture
		/// </summary>
		/// <param name="target">Texture to render to</param>
		protected override void Draw (RenderTexture target)
		{
			var liveText = Text ?? new LiveTextString(Appearance);
			var appearance = Appearance.CloneTextAppearance();

			if(WordWrap)
				drawWrappedText(target, liveText, MultiLine, WrapWidth, appearance);
			else
				drawText(target, liveText, MultiLine, appearance);
		}

		/// <summary>
		/// Draws the text without applying any word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="liveText">Text to render</param>
		/// <param name="multiLine">Flag indicating whether newlines are allowed</param>
		/// <param name="appearance">Information about the initial (default) appearance of the text</param>
		private static void drawText (RenderTarget target, LiveTextString liveText, bool multiLine, TextAppearance appearance)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the text and applies word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="liveText">Text to render</param>
		/// <param name="multiLine">Flag indicating whether the original newlines are allowed</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <param name="appearance">Information about the initial (default) appearance of the text</param>
		private static void drawWrappedText (RenderTarget target, LiveTextString liveText, bool multiLine, int width,
											TextAppearance appearance)
		{
			throw new NotImplementedException();
		}
	}
}
