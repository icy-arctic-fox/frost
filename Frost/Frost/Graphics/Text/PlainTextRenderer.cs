using System;
using SFML.Graphics;
using SFML.Window;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Draws text onto the screen.
	/// Rendering time for text is reduced by drawing to an off-screen texture
	/// and then copying it to an on-screen texture.
	/// </summary>
	public class PlainTextRenderer : TextRenderer
	{
		/// <summary>
		/// Text to render
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Retrieves the text to render and corrects it based on rendering properties
		/// </summary>
		/// <returns>Fixed text string</returns>
		private string getFixedText ()
		{
			var text = Text ?? String.Empty;
			if(!MultiLine)
			{// Strip newline characters
				text = text.Replace("\r", "");
				text = text.Replace("\n", "");
			}
			return text;
		}

		/// <summary>
		/// Creates a new plain text renderer
		/// </summary>
		/// <param name="appearance">Visual appearance of the text</param>
		/// <exception cref="ArgumentNullException">The <paramref name="appearance"/> of the text can't be null.</exception>
		public PlainTextRenderer (TextAppearance appearance)
			: base(appearance)
		{
			// ...
		}

		/// <summary>
		/// Calculates the bounds of the space that the text will occupy
		/// </summary>
		/// <returns>Width and height of the bounds</returns>
		protected override Vector2u CalculateBounds ()
		{
			var text = getFixedText();
			var appearance = Appearance.CloneTextAppearance();
			return WordWrap ? calculateWrappedBounds(text, WrapWidth, appearance) : calculateBounds(text, appearance);
		}

		/// <summary>
		/// Calculates the bounds of some text that does not wrap multiple lines
		/// </summary>
		/// <param name="text">Text to calculate the bounds of</param>
		/// <param name="appearance">Information about the appearance of the text</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateBounds (string text, TextAppearance appearance)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calculates the bounds of some text that has word wrapping applied to it
		/// </summary>
		/// <param name="text">Text to calculate the bounds of</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <param name="appearance">Information about the appearance of the text</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateWrappedBounds (string text, uint width, TextAppearance appearance)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the text onto a texture
		/// </summary>
		/// <param name="target">Texture to render to</param>
		protected override void Draw (RenderTexture target)
		{
			var text = getFixedText();
			var appearance = Appearance.CloneTextAppearance();

			if(WordWrap)
				drawWrappedText(target, text, WrapWidth, appearance);
			else
				drawText(target, text, appearance);
		}

		/// <summary>
		/// Draws the text without applying any word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="text">Text to render</param>
		/// <param name="appearance">Information about the appearance of the text</param>
		private static void drawText (RenderTexture target, string text, TextAppearance appearance)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the text and applies word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="text">Text to render</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <param name="appearance">Information about the appearance of the text</param>
		private static void drawWrappedText (RenderTexture target, string text, uint width, TextAppearance appearance)
		{
			throw new NotImplementedException();
		}
	}
}
