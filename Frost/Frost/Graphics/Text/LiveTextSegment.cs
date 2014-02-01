namespace Frost.Graphics.Text
{
	/// <summary>
	/// A unit of live text
	/// </summary>
	public abstract class LiveTextSegment
	{
		/// <summary>
		/// Draws the live text segment
		/// </summary>
		/// <param name="target">Target to draw the segment on</param>
		/// <param name="position">Location to place the segment in the target</param>
		public abstract void Draw (IRenderTarget target, Point2D position);
	}
}
