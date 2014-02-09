namespace Frost.Graphics.Text
{
	/// <summary>
	/// Draws text onto the screen.
	/// Rendering time for text is reduced by drawing to an off-screen texture
	/// and then copying it to an on-screen texture.
	/// </summary>
	public class PlainTextRenderer : TextRenderer
	{
		public string Text
		{
			get { return TextObject.DisplayedString; }
			set { TextObject.DisplayedString = value; }
		}

		/// <summary>
		/// Prepares the text for drawing.
		/// This method renders the text internally so that it is ready to be quickly drawn.
		/// </summary>
		public override void Prepare ()
		{
			// Calculate the size of the text
			var bounds = TextObject.GetLocalBounds();
			var width  = (uint)(bounds.Width + bounds.Left) + 1;
			var height = (uint)(bounds.Height + bounds.Top) + 1;
			PrepareTexture(width, height);

			// Draw the text
			Buffer.Draw(TextObject);
		}
	}
}
