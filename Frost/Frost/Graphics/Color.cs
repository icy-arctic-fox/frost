using System;

namespace Frost.Graphics
{
	/// <summary>
	/// Information about a color's components.
	/// This structure represents colors as ARGB (alpha, red, green, and blue).
	/// There are also methods for retrieving and setting HSL properties, but these operations take slightly longer.
	/// This structure "unifies" the .net and SFML color objects so that they can be seamlessly converted.
	/// </summary>
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
		/// Copies an existing color
		/// </summary>
		/// <param name="color">Original color</param>
		public Color (Color color)
		{
			_value = color._value;
		}

		/// <summary>
		/// Copies color values from a .net color
		/// </summary>
		/// <param name="color">.net color</param>
		public Color (System.Drawing.Color color)
		{
			_value = color.ToArgb();
		}

		/// <summary>
		/// Copies color values from a SFML color
		/// </summary>
		/// <param name="color">SFML color</param>
		public Color (SFML.Graphics.Color color)
		{
			_value = ArgbToInt(color.R, color.G, color.B, color.A);
		}

		/// <summary>
		/// Creates a new color from the name of a color
		/// </summary>
		/// <param name="name">Color name - color names are based on .net's predefined colors.</param>
		public Color (string name)
		{
			_value = System.Drawing.Color.FromName(name).ToArgb();
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
		/// Hue degrees of the color (0 - 1)
		/// </summary>
		/// <remarks>This property calculates the hue value which is not an instantaneous operation (try to reduce usage).
		/// Repeatedly changing the hue can cause color loss.</remarks>
		public float Hue
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Amount of saturation in the color (0 - 1)
		/// </summary>
		public float Saturation
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Lightness of the color (0 - 1)
		/// </summary>
		public float Lightness
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}
		#endregion

		#region Conversions

		/// <summary>
		/// Converts a .net color to this color structure
		/// </summary>
		/// <param name="color">.net color</param>
		/// <returns>A color structure</returns>
		public static implicit operator Color (System.Drawing.Color color)
		{
			return new Color(color);
		}

		/// <summary>
		/// Converts a SFML color to this color structure
		/// </summary>
		/// <param name="color">SFML color</param>
		/// <returns>A color structure</returns>
		public static implicit operator Color (SFML.Graphics.Color color)
		{
			return new Color(color);
		}

		/// <summary>
		/// Converts this color structure to a .net color
		/// </summary>
		/// <param name="color">Color to convert</param>
		/// <returns>A .net color</returns>
		public static implicit operator System.Drawing.Color (Color color)
		{
			return System.Drawing.Color.FromArgb(color._value);
		}

		/// <summary>
		/// Converts this color structure to a SFML color
		/// </summary>
		/// <param name="color">Color to convert</param>
		/// <returns>A SFML color</returns>
		public static implicit operator SFML.Graphics.Color (Color color)
		{
			return new SFML.Graphics.Color(color.Red, color.Green, color.Blue, color.Alpha);
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// Subtracts a color from another
		/// </summary>
		/// <param name="a">First color</param>
		/// <param name="b">Second color</param>
		/// <returns>Resulting color</returns>
		public static Color operator - (Color a, Color b)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Multiplies two colors together
		/// </summary>
		/// <param name="a">First color</param>
		/// <param name="b">Second color</param>
		/// <returns>Resulting color</returns>
		public static Color operator * (Color a, Color b)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}
		#endregion
	}
}
