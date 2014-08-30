using System;

namespace Frost.Graphics
{
	/// <summary>
	/// Information about a color's components.
	/// This structure represents colors as ARGB (alpha, red, green, and blue).
	/// There are also methods for retrieving and setting HSL properties, but these operations take slightly longer.
	/// This structure "unifies" the .net and SFML color objects so that they can be seamlessly converted.
	/// </summary>
	/// <remarks>Since the color components are stored as RGB,
	/// using HSL values may cause color loss or loss of precision if changed repeatedly.</remarks>
	public struct Color
	{
		/// <summary>
		/// Combined values of the color.
		/// This is stored as ARGB with 8 bits per character (32 bits, 4 bytes total).
		/// </summary>
		private int _value;

		#region Constructors

		/// <summary>
		/// Creates a new color from its components
		/// </summary>
		/// <param name="red">Amount of red (0 - 255)</param>
		/// <param name="green">Amount of green (0 - 255)</param>
		/// <param name="blue">Amount of blue (0 - 255)</param>
		/// <param name="alpha">Amount of alpha/opacity (0 - 255).
		/// 0 is fully transparent and 255 is fully opaque.</param>
		public Color (byte red, byte green, byte blue, byte alpha = 255)
		{
			_value = ArgbToInt(red, green, blue, alpha);
		}

		/// <summary>
		/// Creates a new color from an integer value
		/// </summary>
		/// <param name="value">Integer value containing the color components as RGB</param>
		/// <param name="alpha">Amount of alpha/opacity (0 - 255).
		/// 0 is fully transparent and 255 is fully opaque.</param>
		/// <remarks>A trick to using this constructor is to use hexadecimal when specifying <paramref name="value"/>.
		/// For example: Color(0x00ff00)</remarks>
		public Color (int value, byte alpha = 255)
		{
			_value = (value & 0x00ffffff) | (alpha << 24);
		}

		/// <summary>
		/// Copies an existing color
		/// </summary>
		/// <param name="color">Original color</param>
		public Color (Color color)
		{
			_value = color._value;
		}

		/// <summary>
		/// Copies an existing color, but modifies the amount of alpha
		/// </summary>
		/// <param name="color">Original color</param>
		/// <param name="alpha">Amount of alpha/opacity (0 - 255).
		/// 0 is fully transparent and 255 is fully opaque.</param>
		public Color (Color color, byte alpha)
		{
			_value = (color._value & 0x00ffffff) | (alpha << 24);
		}
		#endregion

		#region Color values

		/// <summary>
		/// Amount of red in the color (0 - 255)
		/// </summary>
		public byte Red
		{
			get { return (byte)((_value >> 16) & 0xff); }
			set { _value = (_value & unchecked((int)0xff00ffff)) | (value << 16); }
		}

		/// <summary>
		/// Amount of green in the color (0 - 255)
		/// </summary>
		public byte Green
		{
			get { return (byte)((_value >> 8) & 0xff); }
			set { _value = (_value & unchecked((int)0xffff00ff)) | (value << 8); }
		}

		/// <summary>
		/// Amount of blue in the color (0 - 255)
		/// </summary>
		public byte Blue
		{
			get { return (byte)(_value & 0xff); }
			set { _value = (_value & unchecked((int)0xffffff00)) | value; }
		}

		/// <summary>
		/// Amount of alpha in the color (0 - 255).
		/// 0 is fully transparent and 255 is fully opaque.
		/// </summary>
		public byte Alpha
		{
			get { return (byte)((_value >> 24) & 0xff); }
			set { _value = (_value & 0x00ffffff) | (value << 24); }
		}

		/// <summary>
		/// All color values arranged as alpha, red, green, and blue
		/// </summary>
		public int Argb
		{
			get { return _value; }
			set { _value = value; }
		}

		/// <summary>
		/// Hue degrees of the color (0 - 1)
		/// </summary>
		public float Hue
		{
			get
			{
				var r = Red   / 255f;
				var g = Green / 255f;
				var b = Blue  / 255f;

				var min = (r < g) ? ((r < b) ? r : b) : ((b < g) ? b : g);
				var max = (r > g) ? ((r > b) ? r : b) : ((b > g) ? b : g);

				var diff = max - min;
				if(Math.Abs(diff) < Single.Epsilon) // Achromatic
					return 0f;

				float h;
				if(r > g && r > b)
					h = (g - b) / diff + (g < b ? 6f : 0f);
				else if(g > b)
					h = (b - r) / diff + 2f;
				else
					h = (r - g) / diff + 4f;
				return h / 6f;
			}

			set
			{
				var s = Saturation;
				if(Math.Abs(s) < Single.Epsilon)
				{// Achromatic
					var l = (int)(Lightness * 255f);
					if(l > Byte.MaxValue)
						l = Byte.MaxValue;
					_value = (_value & unchecked((int)0xff000000)) | (l << 16) | (l << 8) | l;
				}
				else
				{
					byte r, g, b;
					float q, p;
					computeQp(s, Lightness, out q, out p);
					computeRgbFromQp(q, p, value, out r, out g, out b);
					_value = (_value & unchecked((int)0xff000000)) | (r << 16) | (g << 8) | b;
				}
			}
		}

		/// <summary>
		/// Amount of saturation in the color (0 - 1)
		/// </summary>
		public float Saturation
		{
			get
			{
				var r = Red   / 255f;
				var g = Green / 255f;
				var b = Blue  / 255f;

				var min = (r < g) ? ((r < b) ? r : b) : ((b < g) ? b : g);
				var max = (r > g) ? ((r > b) ? r : b) : ((b > g) ? b : g);

				var diff = max - min;
				if(Math.Abs(diff) < Single.Epsilon) // Achromatic
					return 0f;
				var span = max + min;
				var l = span / 2f;
				return diff / ((l > 0.5f) ? 2f - span : span);
			}

			set
			{
				if(Math.Abs(value) < Single.Epsilon)
				{// Achromatic
					var l = (int)(Lightness * 255f);
					if(l > Byte.MaxValue)
						l = Byte.MaxValue;
					_value = (_value & unchecked((int)0xff000000)) | (l << 16) | (l << 8) | l;
				}
				else
				{
					byte r, g, b;
					float q, p, h = Hue;
					computeQp(value, Lightness, out q, out p);
					computeRgbFromQp(q, p, h, out r, out g, out b);
					_value = (_value & unchecked((int)0xff000000)) | (r << 16) | (g << 8) | b;
				}
			}
		}

		/// <summary>
		/// Lightness of the color (0 - 1)
		/// </summary>
		public float Lightness
		{
			get
			{
				var r = Red   / 255f;
				var g = Green / 255f;
				var b = Blue  / 255f;

				var min = (r < g) ? ((r < b) ? r : b) : ((b < g) ? b : g);
				var max = (r > g) ? ((r > b) ? r : b) : ((b > g) ? b : g);
				return (max + min) / 2f;
			}

			set
			{
				var s = Saturation;
				if(Math.Abs(s) < Single.Epsilon)
				{// Achromatic
					var l = (int)(value * 255f);
					if(l > Byte.MaxValue)
						l = Byte.MaxValue;
					_value = (_value & unchecked((int)0xff000000)) | (l << 16) | (l << 8) | l;
				}
				else
				{
					byte r, g, b;
					float q, p, h = Hue;
					computeQp(s, value, out q, out p);
					computeRgbFromQp(q, p, h, out r, out g, out b);
					_value = (_value & unchecked((int)0xff000000)) | (r << 16) | (g << 8) | b;
				}
			}
		}
		#endregion

		#region Operators

		/// <summary>
		/// Adds two colors together
		/// </summary>
		/// <param name="a">First color</param>
		/// <param name="b">Second color</param>
		/// <returns>Combined color</returns>
		public static Color operator + (Color a, Color b)
		{
			var red   = a.Red   + b.Red;
			var green = a.Green + b.Green;
			var blue  = a.Blue  + b.Blue;
			var alpha = a.Alpha + b.Alpha;

			if(red > Byte.MaxValue)
				red = Byte.MaxValue;
			if(green > Byte.MaxValue)
				green = Byte.MaxValue;
			if(blue > Byte.MaxValue)
				blue = Byte.MaxValue;
			if(alpha > Byte.MaxValue)
				alpha = Byte.MaxValue;

			return new Color((byte)red, (byte)green, (byte)blue, (byte)alpha);
		}

		/// <summary>
		/// Subtracts a color from another
		/// </summary>
		/// <param name="a">First color</param>
		/// <param name="b">Second color</param>
		/// <returns>Resulting color</returns>
		public static Color operator - (Color a, Color b)
		{
			var red   = a.Red   - b.Red;
			var green = a.Green - b.Green;
			var blue  = a.Blue  - b.Blue;
			var alpha = a.Alpha - b.Alpha;

			if(red < Byte.MinValue)
				red = Byte.MinValue;
			if(green < Byte.MinValue)
				green = Byte.MinValue;
			if(blue < Byte.MinValue)
				blue = Byte.MinValue;
			if(alpha < Byte.MinValue)
				alpha = Byte.MinValue;

			return new Color((byte)red, (byte)green, (byte)blue, (byte)alpha);
		}

		/// <summary>
		/// Multiplies two colors together
		/// </summary>
		/// <param name="a">First color</param>
		/// <param name="b">Second color</param>
		/// <returns>Resulting color</returns>
		public static Color operator * (Color a, Color b)
		{
			var red   = a.Red   * b.Red   / Byte.MaxValue;
			var green = a.Green * b.Green / Byte.MaxValue;
			var blue  = a.Blue  * b.Blue  / Byte.MaxValue;
			var alpha = a.Alpha * b.Alpha / Byte.MaxValue;

			return new Color((byte)red, (byte)green, (byte)blue, (byte)alpha);
		}
		#endregion

		#region Utility methods

		/// <summary>
		/// Converts color values to an integer
		/// </summary>
		/// <param name="red">Amount of red (0 - 255)</param>
		/// <param name="green">Amount of green (0 - 255)</param>
		/// <param name="blue">Amount of blue (0 - 255)</param>
		/// <param name="alpha">Amount of alpha (0 - 255).
		/// 0 is fully transparent and 255 is fully opaque.</param>
		/// <returns>An integer containing the color values</returns>
		public static int ArgbToInt (byte red, byte green, byte blue, byte alpha = 255)
		{
			return (alpha << 24) | (red << 16) | (green << 8) | blue;
		}

		/// <summary>
		/// Converts an integer to its color values
		/// </summary>
		/// <param name="value">Integer containing the color values</param>
		/// <param name="red">Amount of red (0 - 255)</param>
		/// <param name="green">Amount of green (0 - 255)</param>
		/// <param name="blue">Amount of blue (0 - 255)</param>
		/// <param name="alpha">Amount of alpha (0 - 255).
		/// 0 is fully transparent and 255 is fully opaque.</param>
		public static void IntToArgb (int value, out byte red, out byte green, out byte blue, out byte alpha)
		{
			red   = (byte)((value >> 16) & 0xff);
			green = (byte)((value >> 8) & 0xff);
			blue  = (byte)(value & 0xff);
			alpha = (byte)((value >> 24) & 0xff);
		}
		#endregion

		#region HSL

		/// <summary>
		/// Converts RGB color components to HSL color components
		/// </summary>
		/// <param name="red">Amount of red (0 - 255)</param>
		/// <param name="green">Amount of green (0 - 255)</param>
		/// <param name="blue">Amount of blue (0 - 255)</param>
		/// <param name="hue">Hue degrees (0 - 1)</param>
		/// <param name="saturation">Amount of saturation (0 - 1)</param>
		/// <param name="lightness">Lightness (0 - 1)</param>
		public static void RgbToHsl (byte red, byte green, byte blue, out float hue, out float saturation, out float lightness)
		{
			var r = red   / 255f;
			var g = green / 255f;
			var b = blue  / 255f;

			var min = (red < green) ? ((red < blue) ? r : b) : ((blue < green) ? b : g);
			var max = (red > green) ? ((red > blue) ? r : b) : ((blue > green) ? b : g);

			var span = min + max;
			var diff = max - min;
			lightness = span / 2f;
			if(Math.Abs(diff) < Single.Epsilon) // Achromatic
				hue = saturation = 0f;
			else
			{
				saturation = diff / ((lightness > 0.5f)
					                   ? 2f - span
					                   : span);
				if(red > green && red > blue)
					hue = (g - b) / diff + (green < blue ? 6f : 0f);
				else if(green > blue)
					hue = (b - r) / diff + 2f;
				else
					hue = (r - g) / diff + 4f;
				hue /= 6f;
			}
		}

		/// <summary>
		/// Converts HSL color components to RGB color components
		/// </summary>
		/// <param name="hue">Hue degrees (0 - 1)</param>
		/// <param name="saturation">Amount of saturation (0 - 1)</param>
		/// <param name="lightness">Lightness (0 - 1)</param>
		/// <param name="red">Amount of red (0 - 255)</param>
		/// <param name="green">Amount of green (0 - 255)</param>
		/// <param name="blue">Amount of blue (0 - 255)</param>
		public static void HslToRgb (float hue, float saturation, float lightness, out byte red, out byte green, out byte blue)
		{
			if(Math.Abs(saturation) < Single.Epsilon)
			{// Achromatic
				var l = (int)(lightness * 255f);
				if(l > Byte.MaxValue)
					l = Byte.MaxValue;
				red = green = blue = (byte)l;
			}
			else
			{
				float q, p;
				computeQp(saturation, lightness, out q, out p);
				computeRgbFromQp(q, p, hue, out red, out green, out blue);
			}
		}

		private static void computeQp (float s, float l, out float q, out float p)
		{
			q = l < 0.5f ? l * (1f + s) : l + s - l * s;
			p = 2f * l - q;
		}

		private static void computeRgbFromQp (float q, float p, float h, out byte red, out byte green, out byte blue)
		{
			var r = hueToRgb(q, p, h + 1f / 3f);
			var g = hueToRgb(q, p, h);
			var b = hueToRgb(q, p, h - 1f / 3f);

			var value = (int)(r * 255f);
			red   = value > Byte.MaxValue ? Byte.MaxValue : (byte)value;
			value = (int)(g * 255f);
			green = value > Byte.MaxValue ? Byte.MaxValue : (byte)value;
			value = (int)(b * 255f);
			blue  = value > Byte.MaxValue ? Byte.MaxValue : (byte)value;
		}

		private static float hueToRgb (float q, float p, float t)
		{
			if(t < 0f)
				t += 1f;
			else if(t > 1f)
				t -= 1f;

			if(t < 1f / 6f)
				return p + (q - p) * 6f * t;
			if(t < 1f / 2f)
				return q;
			if(t < 2f / 3f)
				return p + (q - p) * (2f / 3f - t) * 6f;
			return p;
		}
		#endregion
	}
}
