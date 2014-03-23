namespace Frost.Graphics.Text
{
	/// <summary>
	/// A text segment/word that can have word wrapping applied to it
	/// </summary>
	internal interface ITextSize
	{
		/// <summary>
		/// Computes the width and height of the text segment
		/// </summary>
		/// <param name="width">Width</param>
		/// <param name="height">Height</param>
		void GetSize (out int width, out int height);

		/// <summary>
		/// Computes the width and height of the text segment after trimming trailing whitespace
		/// </summary>
		/// <param name="width">Width</param>
		/// <param name="height">Height</param>
		void GetTrimmedSize (out int width, out int height);
	}
}
