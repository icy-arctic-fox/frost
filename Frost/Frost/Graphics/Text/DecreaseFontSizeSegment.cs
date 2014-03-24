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
		/// <param name="appearance">Unused - no modifications are made to the appearance</param>
		/// <returns>Text in the segment</returns>
		public override string Apply (ref TextAppearance appearance)
		{
			appearance.Size -= SizeIncrease;
			return null;
		}
	}
}
