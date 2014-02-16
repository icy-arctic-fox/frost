using Frost.Display;
using Frost.Graphics;
using Frost.Graphics.Text;
using SFML.Graphics;
using Color = Frost.Graphics.Color;
using Font = Frost.Graphics.Text.Font;

namespace Frost.UI
{
	public class DebugOverlay : IRenderable
	{
		private readonly GameRunner _runner;
		private readonly Shape _background;
		private readonly SimpleText _frameText;

		public DebugOverlay (GameRunner runner)
		{
			_runner = runner;
			_background = new RectangleShape(new SFML.Window.Vector2f(200f, 125f)) {
				FillColor = new SFML.Graphics.Color(0, 0, 0, 128)
			};
			var font   = Font.LoadFromFile("../../../../Resources/Fonts/coolvetica.ttf");
			var color  = new Color(255, 255, 255);
			_frameText = new SimpleText(font, 12, color);
		}

		public void Update ()
		{
			_frameText.Text = _runner.ToString();
		}

		public void Draw (IDisplay display, int state)
		{
			// TODO: Draw background
			_frameText.Draw(display);
		}
	}
}
