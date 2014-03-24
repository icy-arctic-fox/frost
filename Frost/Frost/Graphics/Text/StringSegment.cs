using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Plain text segment
	/// </summary>
	public class StringSegment : LiveTextSegment
	{
		/// <summary>
		/// Applies the segment to the renderer state
		/// </summary>
		/// <param name="appearance">Unused - no modifications are made to the appearance</param>
		/// <returns>Text in the segment</returns>
		public override string Apply (ref TextAppearance appearance)
		{
			throw new NotImplementedException();
		}
	}
}
