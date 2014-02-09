using T = SFML.Graphics.Text;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Renders plain text on a single line
	/// </summary>
	public class SingleLinePlainTextRenderer : PlainTextRenderer
	{
		/// <summary>
		/// Prepares the text for drawing.
		/// This method renders the text internally so that it is ready to be quickly drawn.
		/// </summary>
		public override void Prepare ()
		{
			// Calculate the size of the text
			var bounds = TextObject.GetLocalBounds();
			var width  = (uint)(bounds.Width  + bounds.Left) + 1;
			var height = (uint)(bounds.Height + bounds.Top)  + 1;
			PrepareTexture(width, height);

			// Draw the text
			Buffer.Draw(TextObject);
		}
	}
}
