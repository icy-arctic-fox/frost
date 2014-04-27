using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// A string of text that can have formatting codes mixed throughout it to change the appearance
	/// </summary>
	public class LiveTextString : IEnumerable<ILiveTextSegment>
	{
		private readonly List<ILiveTextSegment> _segments = new List<ILiveTextSegment>();
		private readonly TextAppearance _appearance;

		/// <summary>
		/// Creates an empty live text string
		/// </summary>
		/// <param name="appearance">Default appearance of the text</param>
		/// <exception cref="ArgumentNullException">The <paramref name="appearance"/> of the text can't be null.</exception>
		public LiveTextString (TextAppearance appearance)
		{
			if(appearance == null)
				throw new ArgumentNullException("appearance");

			_appearance = appearance;
		}

		/// <summary>
		/// Creates a live text string from some existing text
		/// </summary>
		/// <param name="text">Text to create the live text string from</param>
		/// <param name="appearance">Default appearance of the text</param>
		/// <param name="formatted">True to interpret any formatting codes contained in <paramref name="text"/></param>
		/// <remarks>If <paramref name="appearance"/> is null, then the default text appearance will be used.</remarks>
		/// <seealso cref="TextAppearance.GetDefaultAppearance"/>
		public LiveTextString (string text, TextAppearance appearance, bool formatted = true)
		{
			_appearance = appearance ?? TextAppearance.GetDefaultAppearance();
			if(formatted)
			{// Parse the string and store the segments
				var segments = Parse(text, appearance);
				_segments.AddRange(segments);
			}
			else if(!String.IsNullOrEmpty(text)) // No formatting and not blank
				_segments.Add(new LiveTextStringSegment(text, appearance));
		}

		/// <summary>
		/// Creates a live text string from existing segments
		/// </summary>
		/// <param name="segments">Collection of segments</param>
		/// <param name="appearance">Default appearance of the text</param>
		/// <remarks>If <paramref name="appearance"/> is null, then the default text appearance will be used.</remarks>
		/// <seealso cref="TextAppearance.GetDefaultAppearance"/>
		public LiveTextString (IEnumerable<ILiveTextSegment> segments, TextAppearance appearance)
		{
			_appearance = appearance ?? TextAppearance.GetDefaultAppearance();
			if(segments != null)
				foreach(var segment in segments)
				{
					var toAdd = segment ?? new LiveTextStringSegment(null, appearance);
					_segments.Add(toAdd);
				}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the text segments
		/// </summary>
		/// <returns>An enumerator object that can be used to iterate through the segments</returns>
		public IEnumerator<ILiveTextSegment> GetEnumerator ()
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
		/// <param name="appearance">Default appearance of the text</param>
		/// <returns>Collection of <see cref="ILiveTextSegment"/> extracted from <paramref name="text"/></returns>
		/// <remarks>If <paramref name="appearance"/> is null, then the default text appearance will be used.</remarks>
		/// <seealso cref="TextAppearance.GetDefaultAppearance"/>
		public static IEnumerable<ILiveTextSegment> Parse (string text, TextAppearance appearance = null)
		{
			var parser = new LiveTextParser(text, appearance);
			return parser.Parse(appearanceTranslator, null /* TODO: Create segment translator */);
		}

		private static TextAppearance appearanceTranslator (string type, string extra, TextAppearance before)
		{
			if(type == "b")
			{
				var after  = before.CloneTextAppearance();
				after.Bold = true;
				return after;
			}
			if(type == "i")
			{
				var after = before.CloneTextAppearance();
				after.Italic = true;
				return after;
			}
			if(type == "u")
			{
				var after = before.CloneTextAppearance();
				after.Underlined = true;
				return after;
			}
			return null;
		}

		#region Operators

		/// <summary>
		/// Concatenates a string to live text
		/// </summary>
		/// <param name="text">Original live text</param>
		/// <param name="other">String to append to the live text</param>
		/// <returns>Concatenated live text string</returns>
		/// <remarks>The use of this operator is not recommended since it duplicates the segments.</remarks>
		public static LiveTextString operator + (LiveTextString text, string other)
		{
			// Create new live text and append string to it
			var liveText = new LiveTextString(text._segments, text._appearance);
			liveText._segments.Add(new LiveTextStringSegment(other, text._appearance));
			return liveText;
		}

		/// <summary>
		/// Concatenates a segment to live text
		/// </summary>
		/// <param name="text">Original live text</param>
		/// <param name="other">Segment to append to the live text</param>
		/// <returns>Concatenated live text string</returns>
		/// <remarks>The use of this operator is not recommended since it duplicates the segments.</remarks>
		public static LiveTextString operator + (LiveTextString text, ILiveTextSegment other)
		{
			if(other == null) // Replace with null value
				other = new LiveTextStringSegment(null, text._appearance);

			// Create new live text and append segment to it
			var liveText = new LiveTextString(text._segments, text._appearance);
			liveText._segments.Add(other);
			return liveText;
		}

		/// <summary>
		/// Concatenates two live text strings
		/// </summary>
		/// <param name="text">Original live text</param>
		/// <param name="other">Other live text to concatenate</param>
		/// <returns>Concatenated live text string</returns>
		/// <remarks>The use of this operator is not recommended since it duplicates the segments.</remarks>
		public static LiveTextString operator + (LiveTextString text, LiveTextString other)
		{
			if(other == null) // Replace with null value
				return text + new LiveTextStringSegment(null, text._appearance);

			// Create new live text and append segments to it
			var liveText = new LiveTextString(text._segments, text._appearance);
			liveText._segments.AddRange(other._segments);
			return liveText;
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
				sb.Append(segment == null ? LiveTextStringSegment.NullTextReprsentation : segment.ToString());
			return sb.ToString();
		}
	}
}
