using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frost.Display;
using Frost.Graphics;
using SFML.Graphics;

namespace Frost.UI
{
	/// <summary>
	/// A horizontal graph with each bar 1 pixel in size.
	/// This graph is useful for diagnostics.
	/// </summary>
	public class PixelGraph : IRenderable
	{
		private readonly SFML.Graphics.Sprite _sprite;
		private readonly RenderTexture _texture;
		private readonly RectangleShape _bar;

		private readonly double _span, _height;
		private readonly uint _width;
		private uint _pos;

		/// <summary>
		/// Creates a new pixel graph
		/// </summary>
		/// <param name="width">Width of the graph (in pixels) and number of measurements</param>
		/// <param name="height">Height of the graph (in pixels)</param>
		/// <param name="min">Lower value bound</param>
		/// <param name="max">Upper value bound</param>
		public PixelGraph (uint width, uint height, double min, double max)
		{
			if(min > max)
				throw new ArgumentOutOfRangeException("max", "The upper bound must be larger than the lower bound.");

			_span    = max - min;
			_width   = width;
			_height  = height;
			_texture = new RenderTexture(width, height);
			_sprite  = new SFML.Graphics.Sprite(_texture.Texture);
			_bar     = new RectangleShape();
			_bar.FillColor = new SFML.Graphics.Color(0, 0xff, 0);
			_bar.OutlineColor = new SFML.Graphics.Color(0, 0xff, 0);
			_bar.OutlineThickness = 1;
		}

		/// <summary>
		/// Adds a measurement to the graph
		/// </summary>
		/// <param name="value">Value of the measurement</param>
		public void AddMeasurement (double value)
		{
			var height    = (float)(value / _span * _height);
			_bar.Size     = new SFML.Window.Vector2f(1f, height);
			_bar.Position = new SFML.Window.Vector2f(_pos, (float)_height);
			_bar.Draw(_texture, RenderStates.Default);

			++_pos;
			if(_pos >= _width)
				_pos = 0; // Loop around
		}

		/// <summary>
		/// Draws the graph
		/// </summary>
		/// <param name="display">Display to draw the graph onto</param>
		/// <param name="state">Index of the state to draw (ignored)</param>
		public void Draw (IDisplay display, int state)
		{
			display.Draw(_sprite);
		}
	}
}
