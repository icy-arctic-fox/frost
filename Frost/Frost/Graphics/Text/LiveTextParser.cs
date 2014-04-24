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
		/// Describes a method that translates formatting code information into changes in text appearance
		/// </summary>
		/// <param name="type">Name of the formatter</param>
		/// <param name="extra">Extra information for formatting (will be null if omitted)</param>
		/// <param name="before">Appearance of the text before the formatting is applied</param>
		/// <returns>Appearance of the text after the formatting is applied</returns>
		public delegate TextAppearance FormattingCodeTranslator (string type, string extra, TextAppearance before);

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
		/// <param name="translator">Method that will apply text appearance changes</param>
		/// <returns>Live text segments pulled from the string</returns>
		public IEnumerable<LiveTextSegment> Parse (FormattingCodeTranslator translator)
		{
			var segments = new List<LiveTextSegment>();

			LiveTextToken token;
			while((token = _lexer.GetNext()) != null)
			{
				var startToken = token as LiveTextStartFormatToken;
				if(startToken != null)
					parseStartToken(startToken, translator, segments);
				else
				{
					var endToken = token as LiveTextEndFormatToken;
					if(endToken != null)
						parseEndToken(endToken);
					else
					{
						var segmentToken = token as LiveTextSegmentToken;
						if(segmentToken != null)
							parseSegmentToken(segmentToken);
						else // String, nothing special
							parseStringToken(token);
					}
				}
			}

			return segments.AsReadOnly();
		}

		/// <summary>
		/// Handles translating a start formatting token into a text appearance change
		/// </summary>
		/// <param name="token">Token to get formatting information from</param>
		/// <param name="translator">Method that will apply text appearance changes</param>
		/// <param name="segments">List of segments to append to</param>
		private void parseStartToken (LiveTextStartFormatToken token, FormattingCodeTranslator translator,
									List<LiveTextSegment> segments)
		{
			if(translator != null)
			{// A translator is available, use it to generate the change in appearance
				var before  = _appearanceStack.Peek();
				var current = translator(token.FormatName, token.ExtraInfo, before);
				if(current != null)
				{// Only add it if a change was made
					_appearanceStack.Push(current);
					return;
				}
				// else - fallthrough...
			}
			// TODO: else - unrecognized formatter or no translator, put the literal text in
		}

		/// <summary>
		/// Handles the logic for when a end formatting token is encountered
		/// </summary>
		/// <param name="token">Token to get information from</param>
		private void parseEndToken (LiveTextEndFormatToken token)
		{
			// Pop off the top of the appearance stack
			if(_appearanceStack.Count > 1) // ... but don't pop off the default appearance
				_appearanceStack.Pop();
			// TODO: else - put a literal }
		}

		/// <summary>
		/// Handles translating a standalone segment token
		/// </summary>
		/// <param name="token">Token to get segment information from</param>
		private void parseSegmentToken (LiveTextSegmentToken token)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Handles a plain string token
		/// </summary>
		/// <param name="token">Token to get information from</param>
		private void parseStringToken (LiveTextToken token)
		{
			throw new NotImplementedException();
		}
	}
}
