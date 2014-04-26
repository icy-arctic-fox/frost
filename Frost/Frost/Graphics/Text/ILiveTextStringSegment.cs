using System.Collections.Generic;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// A segment that contains text that can be displayed in a block of live text
	/// </summary>
	public interface ILiveTextStringSegment : ILiveTextSegment
	{
		/// <summary>
		/// Text contained in the segment
		/// </summary>
		string Text { get; }

		/// <summary>
		/// Splits the segment into new segments on line breaks
		/// </summary>
		/// <returns>New segments</returns>
		IEnumerable<ILiveTextStringSegment> SplitOnLineBreaks ();

		/// <summary>
		/// Removes newline characters from the segment's text
		/// </summary>
		/// <returns>New segment without newline characters</returns>
		ILiveTextStringSegment StripLineBreaks ();
	}
}
