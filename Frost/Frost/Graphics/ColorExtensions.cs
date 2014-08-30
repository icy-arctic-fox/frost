using SfmlColor = SFML.Graphics.Color;
using NetColor  = System.Drawing.Color;

namespace Frost.Graphics
{
	/// <summary>
	/// Additional functionality for colors
	/// </summary>
	public static class ColorExtensions
	{
		/// <summary>
		/// Copies color values from a .net color
		/// </summary>
		/// <param name="color">.net color</param>
		/// <returns>Frost color</returns>
		public static Color FromNetColor (this NetColor color)
		{
			return new Color(color.R, color.G, color.B, color.A);
		}

		/// <summary>
		/// Copies color values from a SFML color
		/// </summary>
		/// <param name="color">SFML color</param>
		/// <returns>Frost color</returns>
		public static Color FromSfmlColor (this SfmlColor color)
		{
			return new Color(color.R, color.G, color.B, color.A);
		}

		/// <summary>
		/// Creates a new color from the name of a color
		/// </summary>
		/// <param name="name">Color name - color names are based on .net's predefined colors.</param>
		/// <returns>Frost color</returns>
		public static Color FromName (string name)
		{
			var color = NetColor.FromName(name);
			return new Color(color.R, color.G, color.B, color.A);
		}

		/// <summary>
		/// Converts a Frost color to a .net color
		/// </summary>
		/// <param name="color">Color to convert</param>
		/// <returns>A .net color</returns>
		public static NetColor ToNetColor (this Color color)
		{
			return NetColor.FromArgb(color.Argb);
		}

		/// <summary>
		/// Converts a Frost color to a SFML color
		/// </summary>
		/// <param name="color">Color to convert</param>
		/// <returns>A SFML color</returns>
		public static SfmlColor ToSfmlColor (this Color color)
		{
			return new SfmlColor(color.Red, color.Green, color.Blue, color.Alpha);
		}
	}
}
