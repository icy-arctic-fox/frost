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
		private readonly Vertex[] _verts;

		private readonly double _span, _height;
		private readonly uint _width;
		private float _pos;

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

			// Construct vertices
			_verts    = new Vertex[3];
			_verts[0] = new Vertex(new SFML.Window.Vector2f(0f, 0f), new SFML.Graphics.Color(0x2e, 0xb8, 0x2e));
			_verts[1] = new Vertex(new SFML.Window.Vector2f(0f, 0f), new SFML.Graphics.Color(0x2e, 0xb8, 0x2e, 0x00));
			_verts[2] = new Vertex(new SFML.Window.Vector2f(0f, height), new SFML.Graphics.Color(0x00, 0x00, 0x00, 0x00));
		}

		/// <summary>
		/// Adds a measurement to the graph
		/// </summary>
		/// <param name="value">Value of the measurement</param>
		public void AddMeasurement (double value)
		{
			var height = value / _span * _height;
			_verts[0].Position = new SFML.Window.Vector2f(_pos, (float)_height);
			_verts[1].Position = new SFML.Window.Vector2f(_pos, (float)(_height - height));
			_verts[2].Position = new SFML.Window.Vector2f(_pos, 0f);
			_texture.Draw(_verts, PrimitiveType.LinesStrip);

			_pos += 1f;
			if(_pos > _width)
			{// Loop around
				_pos = 0f;
				_texture.Clear(new SFML.Graphics.Color(0, 0, 0, 0));
			}
		}

		/// <summary>
		/// Draws the graph
		/// </summary>
		/// <param name="display">Display to draw the graph onto</param>
		/// <param name="state">Index of the state to draw (ignored)</param>
		public void Draw (IDisplay display, int state)
		{
			_texture.Display();
			display.Draw(_sprite);
		}
	}
}
