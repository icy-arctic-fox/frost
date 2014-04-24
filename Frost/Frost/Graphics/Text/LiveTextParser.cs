using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Parses a string to extract formatting information which is used to generate live text segments to combine into a live text string
	/// </summary>
	/// <seealso cref="LiveTextLexer"/>
	/// <seealso cref="LiveTextString"/>
	internal class LiveTextParser
	{
		private readonly LiveTextLexer _lexer;
		private readonly Stack<TextAppearance> _appearanceStack = new Stack<TextAppearance>();

		/// <summary>
		/// Creates a new live text string parser
		/// </summary>
		/// <param name="text">Text containing the live text with formatting codes</param>
		/// <param name="appearance">Default and initial appearance of the text</param>
		/// <exception cref="ArgumentNullException">The initial <paramref name="appearance"/> can't be null.</exception>
		public LiveTextParser (string text, TextAppearance appearance)
		{
			if(appearance == null)
				throw new ArgumentNullException("appearance");

			_lexer = new LiveTextLexer(text);
			_appearanceStack.Push(appearance);
		}

		/// <summary>
		/// Parses the string that contains live text formatters and constructs a collection of segments that make up the live text
		/// </summary>
		/// <returns>Live text segments pulled from the string</returns>
		public IEnumerable<LiveTextSegment> Parse ()
		{
			var segments = new List<LiveTextSegment>();

			LiveTextToken token;
			while((token = _lexer.GetNext()) != null)
			{
				var startToken = token as LiveTextStartFormatToken;
				if(startToken != null)
					throw new NotImplementedException();
				else
				{
					var endToken = token as LiveTextEndFormatToken;
					if(endToken != null)
					{// End formatting, pop off the top of the appearance stack
						if(_appearanceStack.Count > 1) // ... but don't pop off the default appearance
							_appearanceStack.Pop();
					}
					else
					{
						var segmentToken = token as LiveTextSegmentToken;
						if(segmentToken != null)
							throw new NotImplementedException();
						else
						{
							// String, nothing special
							throw new NotImplementedException();
						}
					}
				}
			}

			return segments.AsReadOnly();
		}
	}
}
