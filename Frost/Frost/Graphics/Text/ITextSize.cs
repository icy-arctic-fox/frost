namespace Frost.Graphics.Text
{
	/// <summary>
	/// A text segment/word that can have word wrapping applied to it
	/// </summary>
	internal interface ITextSize
	{
		/// <summary>
		/// Computes the width of the text segment
		/// </summary>
		/// <returns>Width</returns>
		int GetWidth ();

		/// <summary>
		/// Computes the height of the text segment
		/// </summary>
		/// <returns>Height</returns>
		int GetHeight ();
	}
}
