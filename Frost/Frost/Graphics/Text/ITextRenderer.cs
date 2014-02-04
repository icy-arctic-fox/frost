namespace Frost.Graphics.Text
{
	/// <summary>
	/// An object that renders text onto a texture
	/// </summary>
	public interface ITextRenderer
	{
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
		/// Draws the text onto a texture at a given position
		/// </summary>
		/// <param name="target">Object to draw the text onto</param>
		/// <param name="x">X-offset at which to draw the text</param>
		/// <param name="y">Y-offset at which to draw the text</param>
		/// <remarks>If the text hasn't been prepared by <see cref="Prepare"/> prior to calling this method,
		/// <see cref="Prepare"/> will be called before drawing the text.</remarks>
		void Draw (IRenderTarget target, int x = 0, int y = 0);
	}
}
