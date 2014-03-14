using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Draws text onto the screen.
	/// Rendering time for text is reduced by drawing to an off-screen texture
	/// and then copying it to an on-screen texture.
	/// </summary>
	public class PlainTextRenderer : TextRenderer
	{
		private string _text = String.Empty;

		/// <summary>
		/// Text to render
		/// </summary>
		public string Text
		{
			get { return _text; }
			set
			{
				Prepared = false;
				_text    = value;
			}
		}

		/// <summary>
		/// Prepares the text for drawing.
		/// This method renders the text internally so that it is ready to be quickly drawn.
		/// </summary>
		public override void Prepare ()
		{
			var text = _text;
			if(!MultiLine)
			{// Strip new lines
				text = text.Replace("\n", String.Empty);
				text = text.Replace("\r", String.Empty);
			}

			if(WordWrap)
				prepareWordWrap(text);
			else
				prepareNoWrap(text);
			Prepared = true;
		}

		#region Word Wrap

		private static readonly Regex _newlineRegex = new Regex(@"\r?\n", RegexOptions.Compiled);

		/// <summary>
		/// Draws the text using word wrapping
		/// </summary>
		/// <param name="text">Text to render</param>
		private void prepareWordWrap (string text)
		{
			// Perform the word wrapping
			SFML.Graphics.FloatRect rect;
			var lines = splitTextIntoWords(text);
			var words = applyWordWrap(lines, TextObject, WrapWidth, out rect);

			// Prepare the texture
			var width  = (uint)(rect.Width  + rect.Left) + 1;
			var height = (uint)(rect.Height + rect.Top)  + 1;
			PrepareTexture(width, height);

			// Draw the text
			drawWords(words);
		}

		/// <summary>
		/// Breaks apart a text string into natural lines and words on each line
		/// </summary>
		/// <param name="text">Text to break apart</param>
		/// <returns>A list of lines with each element containing a list of words</returns>
		private static IEnumerable<IEnumerable<string>> splitTextIntoWords (string text)
		{
			// Break apart the string at existing line breaks
			var textLines = _newlineRegex.Split(text);
			var lineCount = textLines.Length;
			var lines     = new List<IEnumerable<string>>(lineCount);

			// Break each line apart into words
			for(var i = 0; i < lineCount; ++i)
			{
				var lineText = textLines[i];
				var words    = splitLineIntoWords(lineText);
				lines.Add(words);
			}

			return lines;
		}

		/// <summary>
		/// Splits a line into words
		/// </summary>
		/// <param name="lineText">Line of text to split</param>
		/// <returns>Words obtained from splitting the line</returns>
		private static IEnumerable<string> splitLineIntoWords (string lineText)
		{
			var words = new List<string>();
			var whitespaceFound = false;
			var startIndex = 0;
			for(var i = 0; i < lineText.Length; ++i)
			{// Iterate through each character, split on whitespace -> non-whitespace boundary
				var c = lineText[i];
				if(Char.IsWhiteSpace(c))
					whitespaceFound = true;
				else if(whitespaceFound)
				{// End of whitespace, starting new word
					var word = lineText.Substring(startIndex, i - startIndex);
					words.Add(word);
					startIndex = i;
					whitespaceFound = false;
				}
			}

			// Add the last word
			var lastWord = lineText.Substring(startIndex);
			words.Add(lastWord);

			return words;
		}

		/// <summary>
		/// Moves words to new lines if they extend past the desired length
		/// </summary>
		/// <param name="textLines">List of lines with each element containing a list of words</param>
		/// <param name="t">Text object used to calculate the bounds</param>
		/// <param name="wrapWidth">Width of the box to wrap text to</param>
		/// <param name="rect">Bounding box for all words</param>
		private static IEnumerable<IEnumerable<Word>> applyWordWrap (IEnumerable<IEnumerable<string>> textLines, SFML.Graphics.Text t, uint wrapWidth, out SFML.Graphics.FloatRect rect)
		{
			var lines = new List<List<Word>>();
			float rectWidth = 0f, y = 0f;
			var lineSpacing = t.Font.GetLineSpacing(t.CharacterSize);

			foreach(var lineText in textLines)
			{
				var line = new List<Word>();
				var wrapped = false;
				var x = 0f;

				foreach(var wordText in lineText)
				{
					// Calculate the bounds of the word
					var bounds = getTextBounds(wordText, t);
					var word   = new Word(wordText, x, y);
					line.Add(word); // This guarantees at least one word per line.
					// Otherwise, there could be infinite blank lines.

					// Advance the position for the next word
					x += bounds.Width + bounds.Left;
					if(x > rectWidth) // Expand the width of the text bounds
						rectWidth = x;

					if(x >= wrapWidth)
					{// Wrap to the next line
						lines.Add(line);
						line = new List<Word>();
						wrapped = true;

						// Reset position to start of next line
						x  = 0f;
						y += lineSpacing;
					}
					else
						wrapped = false;
				}

				if(line.Count > 0 || !wrapped)
				{// Add the line if it wasn't blank, or if the line wasn't wrapped and was blank
					y += lineSpacing; // Expand the height of the text bounds to accommodate the previous line
					lines.Add(line);
				}
			}

			var rectHeight = lineSpacing * lines.Count;
			rect = new SFML.Graphics.FloatRect(0f, 0f, rectWidth, rectHeight);
			return lines;
		}

		/// <summary>
		/// Draws each of the words in the text
		/// </summary>
		/// <param name="lines">List of lines with each element containing a list of words</param>
		private void drawWords (IEnumerable<IEnumerable<Word>> lines)
		{
			foreach(var line in lines)
			{
				foreach(var word in line)
				{
					TextObject.DisplayedString = word.Text;
					var state = SFML.Graphics.RenderStates.Default;
					state.Transform.Translate(word.X, word.Y);
					Buffer.Draw(TextObject, state);
				}
			}
		}

		/// <summary>
		/// Information about a word when wrapping text
		/// </summary>
		private struct Word
		{
			/// <summary>
			/// The actual text for the word
			/// </summary>
			public readonly string Text;

			/// <summary>
			/// Location of the word
			/// </summary>
			public readonly float X, Y;

			/// <summary>
			/// Creates a word reference
			/// </summary>
			/// <param name="text">Actual text for the word</param>
			/// <param name="x">X-coordinate of the word</param>
			/// <param name="y">Y-coordinate of the word</param>
			public Word(string text, float x, float y)
			{
				Text = text;
				X = x;
				Y = y;
			}
		}
		#endregion

		/// <summary>
		/// Draws the text not using word wrapping
		/// </summary>
		/// <param name="text">Text to render</param>
		private void prepareNoWrap (string text)
		{
			// Calculate the size of the text
			TextObject.DisplayedString = text;
			var bounds = getTextBounds(text, TextObject);
			var width  = (uint)(bounds.Width  + bounds.Left) + 1;
			var height = (uint)(bounds.Height + bounds.Top)  + 1;
			PrepareTexture(width, height);

			// Draw the text
			Buffer.Draw(TextObject);
		}

		/// <summary>
		/// Gets the bounds calculated for some given text
		/// </summary>
		/// <param name="text">Text to calculate the bounds of</param>
		/// <param name="t">Text object used to calculate the bounds</param>
		/// <returns>Text bounds</returns>
		private static SFML.Graphics.FloatRect getTextBounds (string text, SFML.Graphics.Text t)
		{
			t.DisplayedString = text;
			return t.GetLocalBounds();
		}
	}
}
