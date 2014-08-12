namespace Frost.Graphics
{
	/// <summary>
	/// Pen that produces a solid, single-color line
	/// </summary>
	public class SolidPen : Pen
	{
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
		public SolidPen (Color color, float width)
		{
			_color = color;
			_width = width;
		}
	}
}
