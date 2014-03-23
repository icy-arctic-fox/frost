using System;
using System.Linq;
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
				text = text.Replace("\r", " ");
				text = text.Replace("\n", " ");
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
			using(var t = new SFML.Graphics.Text(text, appearance.Font.UnderlyingFont, appearance.Size))
			{
				var bounds = t.GetLocalBounds();
				var width  = (uint)Math.Ceiling(bounds.Width  + bounds.Left);
				var height = (uint)Math.Ceiling(bounds.Height + bounds.Top);
				return new Vector2u(width, height);
			}
		}

		/// <summary>
		/// Calculates the bounds of some text that has word wrapping applied to it
		/// </summary>
		/// <param name="text">Text to calculate the bounds of</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <param name="appearance">Information about the appearance of the text</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateWrappedBounds (string text, int width, TextAppearance appearance)
		{
			using(var t = new SFML.Graphics.Text())
			{
				// Prepare the text object
				t.Font = appearance.Font.UnderlyingFont;
				t.CharacterSize = appearance.Size;

				var wordWrap = performWordWrap(text, width, t);

				// Compute the bounds
				var bounds = wordWrap.Bounds;
				var textWidth   = bounds.Width  + bounds.Left;
				var textHeight  = bounds.Height + bounds.Top;
				var finalWidth  = textWidth  < 0 ? 0U : (uint)textWidth;
				var finalHeight = textHeight < 0 ? 0U : (uint)textHeight;

				return new Vector2u(finalWidth, finalHeight);
			}
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
		private static void drawText (RenderTarget target, string text, TextAppearance appearance)
		{
			using(var t = new SFML.Graphics.Text(text, appearance.Font.UnderlyingFont, appearance.Size))
				target.Draw(t);
		}

		/// <summary>
		/// Draws the text and applies word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="text">Text to render</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <param name="appearance">Information about the appearance of the text</param>
		private static void drawWrappedText (RenderTarget target, string text, int width, TextAppearance appearance)
		{
			using(var t = new SFML.Graphics.Text())
			{
				// Prepare the text object
				t.Font = appearance.Font.UnderlyingFont;
				t.CharacterSize = appearance.Size;

				var wordWrap = performWordWrap(text, width, t);

				// Draw each word
				foreach(var line in wordWrap.Lines)
					foreach(var word in line)
					{
						t.DisplayedString = word.Value.WordString;
						var states = RenderStates.Default;
						states.Transform.Translate(word.Bounds.Left, word.Bounds.Top);
						target.Draw(t, states);
					}
			}
		}

		/// <summary>
		/// Performs word wrapping on a block of text
		/// </summary>
		/// <param name="text">Text to apply word wrapping to</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <param name="t">Prepared text object that will calculate word sizes</param>
		/// <returns>Word wrapping information</returns>
		private static WordWrap<WrappedWord> performWordWrap (string text, int width, SFML.Graphics.Text t)
		{
			// Split the text into words
			var unbrokenLines = SplitTextOnLinebreaks(text);
			var lines = unbrokenLines.Select(SplitTextIntoWords);

			// Construct WrappedWord objects for each word
			var words = from line in lines from word in line select new WrappedWord(t, word);

			// Perform word wrapping
			var wordWrap = new WordWrap<WrappedWord>(width);
			foreach(var ww in words)
				wordWrap.Append(ww);

			return wordWrap;
		}

		/// <summary>
		/// This class is used to calculate the size of each word.
		/// Instances of this class are passed to <see cref="WordWrap{T}"/>.
		/// </summary>
		private class WrappedWord : ITextSize
		{
			private readonly SFML.Graphics.Text _t;
			private readonly string _word;

			/// <summary>
			/// Creates a new word in a word wrapped text block
			/// </summary>
			/// <param name="t">Text object that will compute the bounds</param>
			/// <param name="word">Word string</param>
			public WrappedWord (SFML.Graphics.Text t, string word)
			{
				_t    = t;
				_word = word;
			}

			/// <summary>
			/// String containing the word
			/// </summary>
			public string WordString
			{
				get { return _word; }
			}

			/// <summary>
			/// Computes the width and height of the word
			/// </summary>
			/// <param name="width">Width</param>
			/// <param name="height">Height</param>
			public void GetSize (out int width, out int height)
			{
				computeBounds(_word, out width, out height);
			}

			/// <summary>
			/// Computes the width and height of the word after trimming trailing whitespace
			/// </summary>
			/// <param name="width">Width</param>
			/// <param name="height">Height</param>
			public void GetTrimmedSize (out int width, out int height)
			{
				computeBounds(_word.TrimEnd(), out width, out height);
			}

			/// <summary>
			/// Actually performs the computation of the text bounds
			/// </summary>
			/// <param name="word">Text to gets the bounds of</param>
			/// <param name="width">Width</param>
			/// <param name="height">Height</param>
			private void computeBounds (string word, out int width, out int height)
			{
				_t.DisplayedString = word;
				var bounds = _t.GetLocalBounds();
				width  = (int)Math.Ceiling(bounds.Width  + bounds.Left);
				height = (int)Math.Ceiling(bounds.Height + bounds.Top);
			}
		}
	}
}
