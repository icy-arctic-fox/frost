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
				_text = value;
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
			{ // Strip new lines
				text = text.Replace("\n", String.Empty);
				text = text.Replace("\r", String.Empty);
			}

			if(WordWrap)
				PrepareWordWrap(text);
			else
				PrepareNoWrap(text);
		}

		#region Word Wrap

		private static readonly Regex _newlineRegex    = new Regex(@"\r?\n", RegexOptions.Compiled);
		private static readonly Regex _whitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

		/// <summary>
		/// Draws the text using word wrapping
		/// </summary>
		/// <param name="text">Text to render</param>
		protected void PrepareWordWrap (string text)
		{
			// Perform the word wrapping
			SFML.Graphics.FloatRect rect;
			var lines = breakTextIntoWords(text);
			var words = applyWordWrap(lines, TextObject, out rect);

			// Prepare the texture
			var width  = (uint)(rect.Width + rect.Left) + 1;
			var height = (uint)(rect.Height + rect.Top) + 1;
			PrepareTexture(width, height);

			// Draw the text
			drawWords(words);
		}

		/// <summary>
		/// Breaks apart a text string into natural lines and words on each line
		/// </summary>
		/// <param name="text">Text to break apart</param>
		/// <returns>A list of lines with each element containing a list of words</returns>
		private static IEnumerable<IEnumerable<string>> breakTextIntoWords (string text)
		{
			// Break apart the string at existing line breaks
			var origLines = _newlineRegex.Split(text);
			var lines = new string[origLines.Length][];

			// Break each line apart into words
			for(var i = 0; i < lines.Length; ++i)
			{
				var line  = origLines[i];
				var words = _whitespaceRegex.Split(line);
				lines[i]  = words;
			}

			return lines;
		}

		/// <summary>
		/// Moves words to new lines if they extend past the desired length
		/// </summary>
		/// <param name="origLines">List of lines with each element containing a list of words</param>
		/// <param name="t">Text object used to calculate the bounds</param>
		/// <param name="rect">Bounding box for all words</param>
		private static IEnumerable<IEnumerable<string>> applyWordWrap (IEnumerable<IEnumerable<string>> origLines, SFML.Graphics.Text t, out SFML.Graphics.FloatRect rect)
		{
			var lines = new LinkedList<LinkedList<string>>();
			foreach(var origLine in origLines)
			{//
				var line = new LinkedList<string>();
				var pos = 0f;
				foreach(var word in origLine)
				{
					// Calculate the bounds of the word
					var bounds = getTextBounds(word, t);
					pos += bounds.Width + bounds.Height;
				}
				lines.AddLast(line);
			}

			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws each of the words in the text
		/// </summary>
		/// <param name="lines">List of lines with each element containing a list of words</param>
		private void drawWords (IEnumerable<IEnumerable<string>> lines)
		{
			var state = new SFML.Graphics.RenderStates();

			foreach(var line in lines)
			{
				foreach(var word in line)
				{
					// TODO
					TextObject.DisplayedString = word;
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
