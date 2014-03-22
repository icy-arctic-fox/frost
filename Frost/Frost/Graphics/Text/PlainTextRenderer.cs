﻿using System;
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
		/// Calculates the bounds of the space that the text will occupy
		/// </summary>
		/// <returns>Width and height of the bounds</returns>
		protected override Vector2u CalculateBounds ()
		{
			var text = getFixedText();
			return WordWrap ? calculateWrappedBounds(text, WrapWidth) : calculateBounds(text);
		}

		/// <summary>
		/// Calculates the bounds of some text that does not wrap multiple lines
		/// </summary>
		/// <param name="text">Text to calculate the bounds of</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateBounds (string text)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calculates the bounds of some text that has word wrapping applied to it
		/// </summary>
		/// <param name="text">Text to calculate the bounds of</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateWrappedBounds (string text, uint width)
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
			if(WordWrap)
				drawWrappedText(target, text, WrapWidth);
			else
				drawText(target, text);
		}

		/// <summary>
		/// Draws the text without applying any word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="text">Text to render</param>
		private static void drawText (RenderTexture target, string text)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the text and applies word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="text">Text to render</param>
		/// <param name="width">Target width to wrap lines by</param>
		private static void drawWrappedText (RenderTexture target, string text, uint width)
		{
			throw new NotImplementedException();
		}
	}
}
