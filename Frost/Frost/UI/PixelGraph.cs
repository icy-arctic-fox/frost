﻿using System;
using Frost.Geometry;
using Frost.Graphics;
using Frost.Utility;
using SFML.Graphics;
using SfmlColor  = SFML.Graphics.Color;
using SfmlSprite = SFML.Graphics.Sprite;

namespace Frost.UI
{
	/// <summary>
	/// A horizontal graph with each bar 1 pixel in size.
	/// This graph is useful for diagnostics.
	/// </summary>
	public class PixelGraph : IFrameRender, IFullDisposable
	{
		private const byte DefaultColorRed   = 0x2e;
		private const byte DefaultColorGreen = 0xb8;
		private const byte DefaultColorBlue  = 0x2e;

		private readonly SfmlSprite _sprite;
		private readonly RenderTexture _texture;
		private readonly Vertex[] _verts;

		private readonly double _span, _height;
		private readonly uint _width;
		private float _pos;

		/// <summary>
		/// Location that the graph will be rendered at when drawn
		/// </summary>
		public Point2f Position { get; set; }

		private Graphics.Color _color = new Graphics.Color(DefaultColorRed, DefaultColorGreen, DefaultColorBlue);

		/// <summary>
		/// Color of the bars on the graph
		/// </summary>
		public Graphics.Color Color
		{
			get { return _color; }
			set
			{
				_color = value;
				_verts[0].Color = value.ToSfmlColor();
				_verts[1].Color = new SfmlColor(value.Red, value.Green, value.Blue, 0);
				_verts[2].Color = new SfmlColor(0, 0, 0, 0);
			}
		}

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
			_sprite  = new SfmlSprite(_texture.Texture);

			// Start at an offset to correct gaps due to rounding errors
			_pos = 0.1f;

			// Construct vertices
			_verts    = new Vertex[3];
			_verts[0] = new Vertex(new SFML.Window.Vector2f(0f, height), new SfmlColor(DefaultColorRed, DefaultColorGreen, DefaultColorBlue));
			_verts[1] = new Vertex(new SFML.Window.Vector2f(0f, 0f), new SfmlColor(DefaultColorRed, DefaultColorGreen, DefaultColorBlue, 0));
			_verts[2] = new Vertex(new SFML.Window.Vector2f(0f, 0f), new SfmlColor(0, 0, 0, 0));
		}

		/// <summary>
		/// Adds a measurement to the graph
		/// </summary>
		/// <param name="value">Value of the measurement</param>
		public void AddMeasurement (double value)
		{
			var height = value / _span * _height;
			_verts[1].Position = new SFML.Window.Vector2f(0f, (float)(_height - height)); // Only need to update the y-position of the middle vector
			var rs = RenderStates.Default;
			rs.Transform.Translate(_pos, 0f); // Draw the line at the next x-offset
			_texture.Draw(_verts, PrimitiveType.LinesStrip, rs);

			_pos += 1f;
			if(_pos > _width)
			{// Loop around
				_pos = 0.1f;
				_texture.Clear(new SfmlColor(0, 0, 0, 0));
			}
		}

		/// <summary>
		/// Draws the graph
		/// </summary>
		/// <param name="args">Render information</param>
		public void Render (FrameDrawEventArgs args)
		{
			_texture.Display();
			var rs = RenderStates.Default;
			rs.Transform.Translate(Position.X, Position.Y);
			args.Display.Draw(_sprite, rs);
		}

		#region Disposable

		/// <summary>
		/// Flag that indicates whether the graph has been disposed
		/// </summary>
		public bool Disposed { get; private set; }

		/// <summary>
		/// Triggered when the graph is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Frees the resources held by the graph
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Tears down the pixel graph
		/// </summary>
		~PixelGraph ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Underlying method that releases the resources held by the graph
		/// </summary>
		/// <param name="disposing">True if <see cref="Dispose"/> was called and inner resources should be disposed as well</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!Disposed)
			{// Don't do anything if the graph is already disposed
				Disposed = true;
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);
				if(disposing)
				{// Dispose of the resources this object holds
					_sprite.Dispose();
					_texture.Dispose();
				}
			}
		}
		#endregion
	}
}
