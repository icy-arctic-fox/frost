using System;
using System.Collections;
using System.Collections.Generic;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// A string of text that can have formatting codes mixed throughout it to change the appearance
	/// </summary>
	public class LiveTextString : IEnumerable<LiveTextSegment>
	{
		/// <summary>
		/// Character used to mark the start of a formatting code
		/// </summary>
		public const char FormattingChar = '\\';

		private readonly List<LiveTextSegment> _segments = new List<LiveTextSegment>();

		/// <summary>
		/// Creates an empty live text string
		/// </summary>
		public LiveTextString ()
		{
			// ...
		}

		/// <summary>
		/// Creates a live text string from some existing text
		/// </summary>
		/// <param name="text">Text to create the live text string from</param>
		/// <param name="formatted">True to interpret any formatting codes contained in <paramref name="text"/></param>
		public LiveTextString (string text, bool formatted = true)
		{
			if(formatted)
			{// Parse the string and store the segments
				var segments = Parse(text);
				_segments.AddRange(segments);
			}
			else if(!String.IsNullOrEmpty(text))
				_segments.Add(new StringSegment(text));
		}

		/// <summary>
		/// Returns an enumerator that iterates through the text segments
		/// </summary>
		/// <returns>An enumerator object that can be used to iterate through the segments</returns>
		public IEnumerator<LiveTextSegment> GetEnumerator ()
		{
			return _segments.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a text segments
		/// </summary>
		/// <returns>An enumerator object that can be used to iterate through the collection</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Parses a live text string and extracts the segments
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">The <paramref name="text"/> to parse can't be null.</exception>
		public static IEnumerable<LiveTextSegment> Parse (string text)
		{
			if(text == null)
				throw new ArgumentNullException("text");

			var segments = new List<LiveTextSegment>();
			var start = 0;
			for(var i = 0; i < text.Length; ++i)
			{// Iterate through each character
				var c = text[i];
				if(c == FormattingChar)
				{// Start of a formatting code
					if(start < i)
					{// There was a string prior to this
						var prevString = text.Substring(start, i - start);
						segments.Add(new StringSegment(prevString));
					}

					// Parse the formatting code and add the extracted segment
					var segment = parseFormattingCode(text, ref i);
					segments.Add(segment);
					start = i;
				}
			}

			if(start < text.Length - 1)
			{// There's text left over
				var remaining = text.Substring(start);
				segments.Add(new StringSegment(remaining));
			}

			return segments.AsReadOnly();
		}

		/// <summary>
		/// Parses a string to get a formatting code and create a live text segment from it
		/// </summary>
		/// <param name="text">String to parse</param>
		/// <param name="index">Index of the first character (index of <see cref="FormattingChar"/>).
		/// After the call, this will be the index of the first character after the formatting code.</param>
		/// <returns>Extracted live text string</returns>
		/// <remarks>If the formatting code is invalid, a <see cref="StringSegment"/> is returned containing <see cref="FormattingChar"/>.</remarks>
		private static LiveTextSegment parseFormattingCode (string text, ref int index)
		{
			throw new NotImplementedException();
		}

		#region Operators

		/// <summary>
		/// Concatenates a string to live text
		/// </summary>
		/// <param name="text">Original live text</param>
		/// <param name="other">String to append to the live text</param>
		/// <returns>Concatenated live text string</returns>
		public static LiveTextString operator +(LiveTextString text, string other)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Concatenates a segment to live text
		/// </summary>
		/// <param name="text">Original live text</param>
		/// <param name="other">Segment to append to the live text</param>
		/// <returns>Concatenated live text string</returns>
		public static LiveTextString operator + (LiveTextString text, LiveTextSegment other)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Concatenates two live text strings
		/// </summary>
		/// <param name="text">Original live text</param>
		/// <param name="other">Other live text to concatenate</param>
		/// <returns>Concatenated live text string</returns>
		public static LiveTextString operator + (LiveTextString text, LiveTextString other)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
