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
		}

		/// <summary>
		/// Draws the text using word wrapping
		/// </summary>
		/// <param name="text">Text to render</param>
		protected void PrepareWordWrap (string text)
		{
			// Break apart the string at existing line breaks
			var lines     = new LinkedList<LinkedList<string>>();
			var newlines  = new Regex(@"\r?\n", RegexOptions.Compiled);
			var origLines = newlines.Split(text);

			// Break each line apart into words
			var whitespace = new Regex(@"\s+", RegexOptions.Compiled);
			foreach(var line in origLines)
			{
				var words = whitespace.Split(line);
				var curLineWords = new LinkedList<string>(words);
				lines.AddLast(curLineWords);
			}

			var curLine = lines.First;
			while(curLine != null)
			{
				var pos = 0f;
				var wordList  = curLine.Value;
				var firstWord = wordList.First;
				var curWord   = firstWord;
				while(curWord != null)
				{
					var bounds = getTextBounds(curWord.Value);
					pos += bounds.Width + bounds.Left;
					if(pos > WrapLength)
					{// Line has overflowed, push upcoming words to the next line
						// If the current word is the first word on the line, select the next word to start at.
						// This will prevent infinite blank lines by putting at least one word per line.
						var startingWord = (curWord == firstWord) ? curWord : curWord.Next;
						if(startingWord != null)
						{// There's words to move to the next line
//							wordList.RemoveFollowing(startingWord); // Why doesn't .NET support this on linked lists?
						}
					}
					curWord = curWord.Next;
				}
				curLine = curLine.Next;
			}

			/*
        
        @lines.push([line[0]]) # Add at least one item to prevent infinte empty lines
        line_length, _ = segment_size(line[0]) unless carry
        
        j, carry = 1, false
        while j < line.count
          s = line[j]
          
          # Get the width of the segment
          w, _ = segment_size(s)
          segment_length = w
          
          if w > 0 # Segment has width (text or image) - add the width of a space
            w, _ = segment_size(' ')
            segment_length += w
          end
          
          line_length = line_length + segment_length
          if line_length > @target_width # Exceeded the length for the line
            items = line.slice!(j, line.count - j)
            lines.insert(i + 1, items) # Push the segments to the next line
            line_length = segment_length # Set this here because calling segment_size() multiple times
            carry = true                 # on the same segment can cause problems
            break # Not really needed, but here anyways
          else # Segment can fit on the line
            @lines[-1].push(' ') if w > 0 # Segment has width and needs to be prefixed with whitespace
            @lines[-1].push(s)
          end
          
          j += 1
        end
        
        i += 1
      end
			 * */
		}

		/// <summary>
		/// Draws the text not using word wrapping
		/// </summary>
		/// <param name="text">Text to render</param>
		protected void PrepareNoWrap (string text)
		{
			// Calculate the size of the text
			var bounds = TextObject.GetLocalBounds();
			var width  = (uint)(bounds.Width + bounds.Left) + 1;
			var height = (uint)(bounds.Height + bounds.Top) + 1;
			PrepareTexture(width, height);

			// Draw the text
			Buffer.Draw(TextObject);
		}

		/// <summary>
		/// Gets the bounds calculated for some given text
		/// </summary>
		/// <param name="text">Text to calculate the bounds of</param>
		/// <returns>Text bounds</returns>
		private SFML.Graphics.FloatRect getTextBounds (string text)
		{
			TextObject.DisplayedString = text;
			return TextObject.GetLocalBounds();
		}
	}
}
