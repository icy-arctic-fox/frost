using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Collections of properties that define how text should appear
	/// </summary>
	public class TextAppearance
	{
		private const uint DefaultSize = 10;

		/// <summary>
		/// Height of each character in pixels
		/// </summary>
		public uint Size { get; set; }

		private Font _font;

		/// <summary>
		/// Font face
		/// </summary>
		/// <exception cref="ArgumentNullException">The font face can't be null.</exception>
		public Font Font
		{
			get { return _font; }
			set
			{
				if(value == null)
					throw new ArgumentNullException("value");
				_font = value;
			}
		}

		// TODO: Bold, Underline, Italics flags
		// TODO: Outline Thickness, Outline Color
		// TODO: Horizontal alignment

		/// <summary>
		/// Creates a description of how text should appear
		/// </summary>
		/// <param name="font">Font face</param>
		/// <param name="size">Height of each character in pixels</param>
		/// <exception cref="ArgumentNullException">The font face can't be null.</exception>
		public TextAppearance (Font font, uint size = DefaultSize)
		{
			if(font == null)
				throw new ArgumentNullException("font");

			_font = font;
			Size  = size;
		}
	}
}
