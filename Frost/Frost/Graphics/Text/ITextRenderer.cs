namespace Frost.Graphics.Text
{
	/// <summary>
	/// An object that renders text onto a texture
	/// </summary>
	public interface ITextRenderer
	{
		#region Text properties

		/// <summary>
		/// Displayed text
		/// </summary>
		string Text { get; set; }
		#endregion

		#region Preparation and drawing

		/// <summary>
		/// Indicates whether the text has been prepared (rendered) internally so that is is ready to be quickly drawn.
		/// </summary>
		bool Prepared { get; }

		/// <summary>
		/// Prepares the text for drawing.
		/// This method renders the text internally so that it is ready to be quickly drawn.
		/// </summary>
		void Prepare ();

		/// <summary>
		/// Draws and retrieves the texture that contains the text
		/// </summary>
		/// <remarks>If the text hasn't been prepared by <see cref="Prepare"/> prior to calling this method,
		/// <see cref="Prepare"/> will be called before drawing the text.
		/// However, if it has been, then no drawing needs to be done.
		/// The texture only needs to be redrawn when a property is changed.</remarks>
		Texture DrawTexture ();
		#endregion
	}
}
