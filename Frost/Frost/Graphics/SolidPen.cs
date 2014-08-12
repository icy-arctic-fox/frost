namespace Frost.Graphics
{
	/// <summary>
	/// Pen that produces a solid, single-color line
	/// </summary>
	public class SolidPen : Pen
	{
		private readonly Color _color;
		private readonly float _thickness;

		/// <summary>
		/// Color of the line
		/// </summary>
		public Color Color
		{
			get { return _color; }
		}

		/// <summary>
		/// Width of the line in pixels
		/// </summary>
		public float Thickness
		{
			get { return _thickness; }
		}

		/// <summary>
		/// Creates a pen
		/// </summary>
		/// <param name="color">Color of the pen</param>
		/// <param name="thickness">Width of the line in pixels</param>
		public SolidPen (Color color, float thickness)
		{
			_color = color;
			_thickness = thickness;
		}
	}
}
