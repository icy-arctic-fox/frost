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
		/// Prepares the text for drawing.
		/// This method renders the text internally so that it is ready to be quickly drawn.
		/// </summary>
		public override void Prepare ()
		{
			throw new System.NotImplementedException();
		}
	}
}
