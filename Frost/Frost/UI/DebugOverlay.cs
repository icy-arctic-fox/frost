using System;
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
		private static readonly SFML.Graphics.Color _backgroundColor = new SFML.Graphics.Color(64, 64, 64, 128);
		private static readonly Color _textColor = new Color(0xffffff);

		private readonly GameRunner _runner;
		private readonly SFML.Graphics.Sprite _background;
		private readonly SimpleText _frameText, _stateText;

		/// <summary>
		/// Creates a new debug overlay
		/// </summary>
		/// <param name="runner">Game runner to pull information from</param>
		/// <param name="font">Font used to display text</param>
		/// <param name="fontSize">Font size</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="runner"/> or <paramref name="font"/> is null</exception>
		public DebugOverlay (GameRunner runner, Font font, uint fontSize)
		{
			if(runner == null)
				throw new ArgumentNullException("runner", "The game runner can't be null.");
			if(font == null)
				throw new ArgumentNullException("font", "The font used to display the debug information can't be null.");

			_runner     = runner;
			_background = new SFML.Graphics.Sprite();
			_frameText  = new SimpleText(font, fontSize, _textColor);
			_stateText  = new SimpleText(font, fontSize, _textColor);
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
			_stateText.Text = _runner.Scenes.StateManager.ToString();
			
			// Calculate the bounds
			var bounds = _frameText.Bounds;
			var width  = bounds.Width;
			var height = bounds.Height;
			bounds = _stateText.Bounds;
			if(bounds.Width > width)
				width = bounds.Width;
			height += bounds.Height;

			if(width > Bounds.Width || height > Bounds.Height)
			{// Bounds need to be updated and the background resized
				Bounds  = new Rect2f(0f, 0f, width, height);
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
			var yPos = bounds.Top;
			_frameText.Draw(display, bounds.Left, yPos);
			yPos += _frameText.Bounds.Height;
			_stateText.Draw(display, bounds.Left, yPos);
		}
	}
}
