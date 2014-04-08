using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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
		public LiveTextString (IEnumerable<LiveTextSegment> segments)
		{
			if(segments != null)
				foreach(var segment in segments)
				{
					var toAdd = segment ?? new StringSegment(NullSegmentString);
					_segments.Add(toAdd);
				}
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
				var lexer = new LiveTextLexer(text);
				LiveTextToken token;
				while((token = lexer.GetNext()) != null)
				{
					var segment = convertTokenIntoSegment(token);
					if(segment != null)
						segments.Add(segment);
				}
			}

			return segments.AsReadOnly();
		}

		/// <summary>
		/// Gets a live text segment from a token
		/// </summary>
		/// <param name="token">Token to parse</param>
		/// <returns>A live text segment</returns>
		private static LiveTextSegment convertTokenIntoSegment (LiveTextToken token)
		{
			// Check for LiveTextStartFormatToken
			var startFormatToken = token as LiveTextStartFormatToken;
			if(startFormatToken != null) // Start of a formatter
				return parseStartFormatToken(startFormatToken);

			// Check for LiveTextEndFormatToken
			var endFormatToken = token as LiveTextEndFormatToken;
			if(endFormatToken != null) // End of a formatter
				throw new NotImplementedException();

			// else - default to string segment
			return new StringSegment(token.ToString());
		}

		/// <summary>
		/// Attempts to find a formatter segment for a token
		/// </summary>
		/// <param name="token">Token to parse</param>
		/// <returns>The corresponding formatter segment for <paramref name="token"/></returns>
		/// <remarks>A string segment containing the original formatting code will be returned if the formatting code is unknown.</remarks>
		private static LiveTextSegment parseStartFormatToken (LiveTextStartFormatToken token)
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

		/// <summary>
		/// Generates the string representation of the live text
		/// </summary>
		/// <returns>Live text containing the strings and formatting codes</returns>
		public override string ToString ()
		{
			var sb = new StringBuilder();
			foreach(var segment in _segments)
				sb.Append(segment == null ? NullSegmentString : segment.ToString());
			return sb.ToString();
		}
	}
}
