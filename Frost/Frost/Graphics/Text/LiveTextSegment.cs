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
		/// <param name="renderer">Renderer that tracks the status of the live text</param>
		public abstract void Apply (LiveTextRenderer renderer);
	}
}
