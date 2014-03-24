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

		private const string FormattingCharString = @"\";

		/// <summary>
		/// String that appears when null is encountered in a <see cref="LiveTextString"/>
		/// </summary>
		public const string NullSegmentString = "null";

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
		/// Creates a live text string from existing segments
		/// </summary>
		/// <param name="segments">Collection of segments</param>
		private LiveTextString (IEnumerable<LiveTextSegment> segments)
		{
			_segments.AddRange(segments);
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
		/// <param name="text">String to parse and extract segments from</param>
		/// <returns>Collection of <see cref="LiveTextSegment"/> extracted from <paramref name="text"/></returns>
		public static IEnumerable<LiveTextSegment> Parse (string text)
		{
			var segments = new List<LiveTextSegment>();

			if(!String.IsNullOrEmpty(text))
			{// There is text to parse
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
			if(++index < text.Length)
			{// There are character after the current position
				var c = text[index];
				switch(c)
				{// Check against known formatting codes
				case IncreaseFontSizeSegment.FormattingChar:
					++index;
					return new IncreaseFontSizeSegment();
				case DecreaseFontSizeSegment.FormattingChar:
					++index;
					return new DecreaseFontSizeSegment();
				case FormattingChar: // Escape sequence
					++index;
					return new StringSegment(FormattingCharString);
				}
			}

			// Not a valid formatting code
			return new StringSegment(FormattingCharString);
		}

		#region Operators

		/// <summary>
		/// Concatenates a string to live text
		/// </summary>
		/// <param name="text">Original live text</param>
		/// <param name="other">String to append to the live text</param>
		/// <returns>Concatenated live text string</returns>
		public static LiveTextString operator + (LiveTextString text, string other)
		{
			if(other == null) // Replace with null value
				other = NullSegmentString;

			// Create new live text and append string to it
			var liveText = new LiveTextString(text._segments);
			liveText._segments.Add(new StringSegment(other));
			return liveText;
		}

		/// <summary>
		/// Concatenates a segment to live text
		/// </summary>
		/// <param name="text">Original live text</param>
		/// <param name="other">Segment to append to the live text</param>
		/// <returns>Concatenated live text string</returns>
		public static LiveTextString operator + (LiveTextString text, LiveTextSegment other)
		{
			if(other == null) // Replace with null value
				other = new StringSegment(NullSegmentString);

			// Create new live text and append segment to it
			var liveText = new LiveTextString(text._segments);
			liveText._segments.Add(other);
			return liveText;
		}

		/// <summary>
		/// Concatenates two live text strings
		/// </summary>
		/// <param name="text">Original live text</param>
		/// <param name="other">Other live text to concatenate</param>
		/// <returns>Concatenated live text string</returns>
		public static LiveTextString operator + (LiveTextString text, LiveTextString other)
		{
			if(other == null) // Replace with null value
				return text + new StringSegment(NullSegmentString);

			// Create new live text and append segments to it
			var liveText = new LiveTextString(text._segments);
			liveText._segments.AddRange(other._segments);
			return liveText;
		}

		/// <summary>
		/// Converts a <see cref="String"/> to a <see cref="LiveTextString"/>
		/// </summary>
		/// <param name="text">String to convert</param>
		/// <returns>A live text string that has formatting codes applied</returns>
		public static implicit operator LiveTextString (string text)
		{
			return new LiveTextString(text);
		}
		#endregion
	}
}
