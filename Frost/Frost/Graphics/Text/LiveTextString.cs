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
	}
}
