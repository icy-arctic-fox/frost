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
		/// Returns an enumerator that iterates through the text segments
		/// </summary>
		/// <returns>A <see cref="LiveTextEnumerator"/> that can be used to iterate through the segments</returns>
		public IEnumerator<LiveTextSegment> GetEnumerator ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a text segments
		/// </summary>
		/// <returns>An enumerator object that can be used to iterate through the collection</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
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
