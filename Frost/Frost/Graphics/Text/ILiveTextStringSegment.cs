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
	}
}
