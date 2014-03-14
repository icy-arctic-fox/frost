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
				PrepareWordWrap(text);
			else
				PrepareNoWrap(text);
			Prepared = true;
		}

		#region Word Wrap

		private static readonly Regex _newlineRegex    = new Regex(@"\r?\n", RegexOptions.Compiled);

		/// <summary>
		/// Draws the text using word wrapping
		/// </summary>
		/// <param name="text">Text to render</param>
		protected void PrepareWordWrap (string text)
		{
			// Perform the word wrapping
			SFML.Graphics.FloatRect rect;
			var lines = splitTextIntoWords(text);
			var words = applyWordWrap(lines, TextObject, out rect);

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
		/// <param name="rect">Bounding box for all words</param>
		private static IEnumerable<IEnumerable<Word>> applyWordWrap (IEnumerable<IEnumerable<string>> textLines, SFML.Graphics.Text t, out SFML.Graphics.FloatRect rect)
		{
			var lines = new LinkedList<LinkedList<Word>>();
			float width = 0f, height = 0f;
			foreach(var lineText in textLines)
			{
				var line = new LinkedList<Word>();
				var x    = 0f;
				var max  = 0f;
				foreach(var wordText in lineText)
				{
					// Calculate the bounds of the word
					var bounds = getTextBounds(wordText, t);
					var word   = new Word(wordText, x, height);
					line.AddLast(word);

					// Advance the position for the next word
					x += bounds.Width + bounds.Left; // TODO: Add whitespace
					if(bounds.Height + bounds.Top > max)
						max = bounds.Height + bounds.Top;
					if(x > width)
						width = x;
				}
				height += max;
				lines.AddLast(line);
			}

			rect = new SFML.Graphics.FloatRect(0f, 0f, width, height);
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
		protected void PrepareNoWrap (string text)
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
