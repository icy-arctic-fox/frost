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
		/// <param name="renderer">Renderer that tracks the status of the live text</param>
		public override void Apply (LiveTextRenderer renderer)
		{
			throw new NotImplementedException();
		}
	}
}
