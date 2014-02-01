using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Plain text segment
	/// </summary>
	public class StringSegment : LiveTextSegment
	{
		/// <summary>
		/// Draws the live text segment
		/// </summary>
		/// <param name="target">Target to draw the segment on</param>
		/// <param name="position">Location to place the segment in the target</param>
		public override void Draw (IRenderTarget target, Point2D position)
		{
			throw new NotImplementedException();
		}
	}
}
