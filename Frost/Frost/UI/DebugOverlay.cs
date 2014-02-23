using Frost.Display;
using Frost.Geometry;
using Frost.Graphics;
using Frost.Graphics.Text;
using SFML.Graphics;
using Color = Frost.Graphics.Color;
using Font = Frost.Graphics.Text.Font;

namespace Frost.UI
{
	/// <summary>
	/// Informational overlay that display useful debugging information
	/// </summary>
	public class DebugOverlay : IRenderable
	{
		private const float XPosition = 0f;
		private const float YPosition = 0f;
		private static readonly SFML.Graphics.Color _backgroundColor = new SFML.Graphics.Color(64, 64, 64, 128);
		private static readonly Color _textColor = new Color(0xffffff);
		private const uint FontSize = 12;

		private readonly GameRunner _runner;
		private readonly SFML.Graphics.Sprite _background;
		private readonly SimpleText _frameText;

		/// <summary>
		/// Creates a new debug overlay
		/// </summary>
		/// <param name="runner">Game runner to pull information from</param>
		public DebugOverlay (GameRunner runner)
		{
			_runner     = runner;
			_background = new SFML.Graphics.Sprite();
			var font    = Font.LoadFromFile("../../../../Resources/Fonts/coolvetica.ttf"); // TODO: Replace with parameter
			_frameText  = new SimpleText(font, FontSize, _textColor);
		}

		/// <summary>
		/// Boundaries of the overlay window
		/// </summary>
		public Rect2f Bounds { get; private set; }

		// TODO: Add ability to specify X and Y position

		private volatile bool _resize;

		/// <summary>
		/// Updates the contents of the overlay
		/// </summary>
		public void Update ()
		{
			// Update the text
			_frameText.Text = _runner.ToString();
			var bounds = _frameText.Bounds;
			if(bounds.Width > Bounds.Width || bounds.Height > Bounds.Height)
			{// Bounds need to be updated and the background resized
				Bounds  = bounds;
				_resize = true;
			}
		}

		/// <summary>
		/// Draws the debug overlay onto the display
		/// </summary>
		/// <param name="display">Display to draw on</param>
		/// <param name="state">State index (ignored)</param>
		public void Draw (IDisplay display, int state)
		{
			var bounds = Bounds;
			if(_background.Texture == null || _resize)
			{// The background needs to be resized
				// Calculate the bounds
				var width  = (uint)(bounds.Width + 1);
				var height = (uint)(bounds.Height + 1);

				// Create the new texture
				var background          = new Image(width, height, _backgroundColor);
				_background.Texture     = new SFML.Graphics.Texture(background);
				_background.TextureRect = new IntRect(0, 0, (int)width, (int)height);
				_resize = false;
			}

			// Draw the background
			var rs = RenderStates.Default;
			rs.Transform.Translate(bounds.Left, bounds.Top);
			display.Draw(_background, rs);

			// Draw the text
			_frameText.Draw(display, XPosition, YPosition);
		}
	}
}
