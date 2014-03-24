namespace Frost.Graphics.Text
{
	/// <summary>
	/// A unit of live text
	/// </summary>
	public abstract class LiveTextSegment
	{
		/// <summary>
		/// Applies the segment to the renderer state
		/// </summary>
		/// <param name="appearance">Make modifications to the appearance of the text with this object</param>
		/// <returns>Text for the segment or null if there is none (only appearance is modified)</returns>
		public abstract string Apply (ref TextAppearance appearance);
	}
}
