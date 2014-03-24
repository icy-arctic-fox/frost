using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Decreases the size of the font
	/// </summary>
	public class DecreaseFontSizeSegment : LiveTextSegment
	{
		/// <summary>
		/// Character used to represent a decrease in font size in a formatting code
		/// </summary>
		public const char FormattingChar = '-';

		/// <summary>
		/// Amount (in pixels) to decrease the font size by
		/// </summary>
		public const uint SizeIncrease = 2U;

		/// <summary>
		/// Applies the segment to the renderer state
		/// </summary>
		/// <param name="appearance">Appearance to decrease the font size of</param>
		/// <returns>Unused - null is returned</returns>
		public override string Apply (ref TextAppearance appearance)
		{
			appearance.Size -= SizeIncrease;
			return null;
		}

		/// <summary>
		/// Gets the formatting code that represents the segment
		/// </summary>
		/// <returns>A formatting code string</returns>
		public override string ToString ()
		{
			return new String(new[] {LiveTextString.FormattingChar, FormattingChar});
		}
	}
}
