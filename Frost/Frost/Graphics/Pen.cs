namespace Frost.Graphics
{
	/// <summary>
	/// Information about a line's appearance
	/// </summary>
	public abstract class Pen
	{
		/// <summary>
		/// Default pen type
		/// </summary>
		public static readonly Pen Default = new SolidPen(new Color(), 1f);
	}
}
