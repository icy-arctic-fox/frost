using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Collections of properties that define how text should appear
	/// </summary>
	public class TextAppearance : ICloneable
	{
		private const uint DefaultSize = 10;
		private static readonly Color DefaultColor = new Color(0, 0, 0);

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

		/// <summary>
		/// Base color of the text
		/// </summary>
		public Color Color { get; set; }

		/// <summary>
		/// Increase the weight of the font
		/// </summary>
		public bool Bold { get; set; }

		/// <summary>
		/// Apply a shearing effect to the font
		/// </summary>
		public bool Italic { get; set; }

		/// <summary>
		/// Place a line under the text
		/// </summary>
		public bool Underlined { get; set; }

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
			Color = DefaultColor;
		}

		/// <summary>
		/// Creates a description of how text should appear by copying an existing instance
		/// </summary>
		/// <param name="copy">Instance to copy from</param>
		/// <exception cref="ArgumentNullException">The instance to copy from can't be null.</exception>
		public TextAppearance (TextAppearance copy)
		{
			if(copy == null)
				throw new ArgumentNullException("copy");

			_font      = copy._font;
			Size       = copy.Size;
			Color      = copy.Color;
			Bold       = copy.Bold;
			Italic     = copy.Italic;
			Underlined = copy.Underlined;
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance
		/// </summary>
		/// <returns>A new object that is a copy of this instance</returns>
		public TextAppearance CloneTextAppearance ()
		{
			return new TextAppearance(this);
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance
		/// </summary>
		/// <returns>A new object that is a copy of this instance</returns>
		public object Clone ()
		{
			return CloneTextAppearance();
		}

		/// <summary>
		/// Applies the appearance properties to a text object
		/// </summary>
		/// <param name="t">Text object to update</param>
		internal void ApplyTo (SFML.Graphics.Text t)
		{
			t.CharacterSize = Size;
			t.Font  = _font.UnderlyingFont;
			t.Color = Color;

			t.Style = SFML.Graphics.Text.Styles.Regular;
			if(Bold)
				t.Style |= SFML.Graphics.Text.Styles.Bold;
			if(Italic)
				t.Style |= SFML.Graphics.Text.Styles.Italic;
			if(Underlined)
				t.Style |= SFML.Graphics.Text.Styles.Underlined;
		}
	}
}
