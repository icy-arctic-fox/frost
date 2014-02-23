using Frost.Display;
using Frost.Geometry;
using Frost.Graphics;
using Frost.Graphics.Text;
using SFML.Graphics;
using Color = Frost.Graphics.Color;
using Font = Frost.Graphics.Text.Font;

namespace Frost.UI
{
	public class DebugOverlay : IRenderable
	{
		private const float XPosition = 0f;
		private const float YPosition = 0f;
		private static readonly SFML.Graphics.Color _backgroundColor = new SFML.Graphics.Color(64, 64, 64, 128);

		private readonly GameRunner _runner;
		private readonly SFML.Graphics.Sprite _sprite;
		private readonly SimpleText _frameText;

		public DebugOverlay (GameRunner runner)
		{
			_runner = runner;
			_sprite = new SFML.Graphics.Sprite();
			var font   = Font.LoadFromFile("../../../../Resources/Fonts/coolvetica.ttf"); // TODO: Replace with parameter
			var color  = new Color(255, 255, 255);
			_frameText = new SimpleText(font, 12, color);
		}

		public Rect2f Bounds { get; private set; }

		private volatile bool _resize;

		public void Update ()
		{
			// Update the text
			_frameText.Text = _runner.ToString();
			var bounds = _frameText.Bounds;
			if(bounds.Width > Bounds.Width || bounds.Height > Bounds.Height)
			{
				Bounds = bounds;
				_resize = true;
			}
		}

		public void Draw (IDisplay display, int state)
		{
			if(_sprite.Texture == null || _resize)
			{// The background needs to be resized
				// Calculate the bounds
				var bounds = Bounds;
				var width  = (uint)(bounds.Width + 1);
				var height = (uint)(bounds.Height + 1);

				// Create the new texture
				var background  = new SFML.Graphics.Image(width, height, _backgroundColor);
				_sprite.Texture = new SFML.Graphics.Texture(background);
				_resize = false;
			}

			// Draw the background
			var rs = RenderStates.Default;
			rs.Transform.Translate(XPosition, YPosition);
			display.Draw(_sprite, rs);

			// Draw the text
			_frameText.Draw(display, XPosition, YPosition);
		}
	}
}
