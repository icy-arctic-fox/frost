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
			// TODO: Add string segment
/*			else if(!String.IsNullOrEmpty(text))
				_segments.Add(new StringSegment(text)); */
		}

		/// <summary>
		/// Creates a live text string from existing segments
		/// </summary>
		/// <param name="segments">Collection of segments</param>
		public LiveTextString (IEnumerable<ILiveTextSegment> segments)
		{
			throw new NotImplementedException();
			/*			if(segments != null)
				foreach(var segment in segments)
				{
					var toAdd = segment ?? new StringSegment(NullSegmentString);
					_segments.Add(toAdd);
				}*/
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
		/// <returns>Collection of <see cref="ILiveTextSegment"/> extracted from <paramref name="text"/></returns>
		public static IEnumerable<ILiveTextSegment> Parse (string text)
		{
			var parser = new LiveTextParser(text, null /* TODO: Appearance parameter is needed */);
			return parser.Parse(null /* TODO: Create appearance translator */, null /* TODO: Create segment translator */);
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
			var liveText = new LiveTextString(text._segments);
			// TODO: liveText._segments.Add(new StringSegment(other));
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
			// TODO: Handle null
			/* if(other == null) // Replace with null value
				other = new StringSegment(NullSegmentString); */

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
		/// <remarks>The use of this operator is not recommended since it duplicates the segments.</remarks>
		public static LiveTextString operator + (LiveTextString text, LiveTextString other)
		{
			// TODO: Handle null
			/* if(other == null) // Replace with null value
				return text + new StringSegment(NullSegmentString); */

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
				sb.Append(segment == null ? LiveTextStringSegment.NullTextReprsentation : segment.ToString());
			return sb.ToString();
		}
	}
}
