using T = SFML.Graphics.Text;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Renders plain text on a single line
	/// </summary>
	public class SingleLinePlainTextRenderer : PlainTextRenderer
	{
		private readonly T _text = new T();

		/// <summary>
		/// Displayed text
		/// </summary>
		public override string Text
		{
			get { return _text.DisplayedString; }
			set
			{
				_text.DisplayedString = value;
				ResetTexture();
			}
		}

		/// <summary>
		/// Prepares the text for drawing.
		/// This method renders the text internally so that it is ready to be quickly drawn.
		/// </summary>
		public override void Prepare ()
		{
			// Calculate the size of the text
			var bounds = _text.GetLocalBounds();
			var width  = (uint)bounds.Width  + 1;
			var height = (uint)bounds.Height + 1;
			PrepareTexture(width, height);

			// Draw the text
			Target.Draw(_text);
		}
	}
}
