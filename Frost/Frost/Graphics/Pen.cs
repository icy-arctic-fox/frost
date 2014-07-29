namespace Frost.Graphics
{
	/// <summary>
	/// Information about a line's appearance
	/// </summary>
	public class Pen
	{
		/// <summary>
		/// Default pen type
		/// </summary>
		public static readonly Pen Default = new Pen(new Color(), 1f);

		private readonly Color _color;
		private readonly float _width;

		/// <summary>
		/// Color of the line
		/// </summary>
		public Color Color
		{
			get { return _color; }
		}

		/// <summary>
		/// Thickness of the line (in pixels)
		/// </summary>
		public float Width
		{
			get { return _width; }
		}

		/// <summary>
		/// Creates a pen
		/// </summary>
		/// <param name="color">Color of the pen</param>
		/// <param name="width">Thickness of the pen</param>
		public Pen (Color color, float width)
		{
			_color = color;
			_width = width;
		}

		// TODO: Style
	}
}
