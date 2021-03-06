﻿using System;
using Frost.Geometry;
using Frost.Utility;
using SFML.Graphics;
using SfmlFont  = SFML.Graphics.Font;
using SfmlColor = SFML.Graphics.Color;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Bare minimal text functionality.
	/// This class is optimized for text that changes frequently,
	/// but doesn't allow the font, size, or color to change after initialization.
	/// </summary>
	public class SimpleText : IFullDisposable
	{
		private readonly uint _size;
		private readonly int _spacing;
		private readonly SfmlFont _font;
		private SfmlColor _color;
		private readonly VertexArray _verts = new VertexArray(PrimitiveType.Quads);
		private RenderStates _rs = RenderStates.Default;
		private readonly MutableString _text = new MutableString();

		/// <summary>
		/// Creates a new simple text object
		/// </summary>
		/// <param name="font">Font used to render the text</param>
		/// <param name="size">Font size</param>
		/// <param name="color">Font color</param>
		public SimpleText (Font font, uint size, Color color)
		{
			_font    = font.SfmlFont;
			_size    = size;
			_spacing = _font.GetLineSpacing(size);
			_color   = color.ToSfmlColor();
		}

		/// <summary>
		/// Displayed text
		/// </summary>
		public string Text
		{
			get { return _text ?? String.Empty; }
			set
			{
				_text.Clear();
				_text.Append(value ?? String.Empty);
				Update();
			}
		}

		/// <summary>
		/// Low-level access to each character displayed
		/// </summary>
		/// <remarks><see cref="Update"/> must be called if the string is modified.</remarks>
		public MutableString Contents
		{
			get { return _text; }
		}

		/// <summary>
		/// Color of the text
		/// </summary>
		public Color Color
		{
			get { return _color.FromSfmlColor(); }
			set
			{
				_color = value.ToSfmlColor();
				updateVertColor(_color);
			}
		}

		/// <summary>
		/// Bounds of the text
		/// </summary>
		public Rect2f Bounds { get; private set; }

		/// <summary>
		/// Forces the text display information to update
		/// </summary>
		/// <remarks>This method must be called after modifying <see name="Contents"/>.</remarks>
		public void Update ()
		{
			constructVertices();
		}

		/// <summary>
		/// Updates the vertices that display each glyph
		/// </summary>
		private void constructVertices ()
		{
			// Resize the vertex array if needed
			var textLength = _text.Length;
			var vertCount  = (uint)textLength * 4;
			if(_verts.VertexCount != vertCount)
				_verts.Resize(vertCount);

			var xPos = 0f;
			for(var i = 0; i < textLength; ++i)
			{// Construct each set of 4 vertices for each glyph
				var c = _text[i];
				var j = (uint)(i * 4);
				constructFromGlyph(c, j, ref xPos);
			}

			// Update the bounds
			var cornerVert = _verts[vertCount - 2]; // Bottom-right corner of last quad
			var width  = cornerVert.Position.X;
			var height = cornerVert.Position.Y;
			Bounds = new Rect2f(0f, 0f, width, height);

			// Store the updated texture
			_rs.Texture = _font.GetTexture(_size);
		}

		/// <summary>
		/// Constructs a set of vertices from a glyph
		/// </summary>
		/// <param name="c">Character to get the texture of</param>
		/// <param name="index">Index of the first vertex</param>
		/// <param name="xPos">X-offset to place the character at and position for the next character after returning</param>
		private void constructFromGlyph (char c, uint index, ref float xPos)
		{
			var glyph   = _font.GetGlyph(c, _size, false);
			var srcRect = glyph.TextureRect;
			var bounds  = glyph.Bounds;

			// Calculate vertex positions
			var h = srcRect.Height;
			var w = srcRect.Width;
			var left   = xPos + bounds.Left;
			var right  = left + w;
			var top    = _spacing + bounds.Top;
			var bottom = top + h;

			// Vertex points
			var v1 = new SFML.Window.Vector2f(left,  top);
			var v2 = new SFML.Window.Vector2f(right, top);
			var v3 = new SFML.Window.Vector2f(right, bottom);
			var v4 = new SFML.Window.Vector2f(left,  bottom);

			// Calculate texture positions
			left   = srcRect.Left;
			right  = left + w;
			top    = srcRect.Top;
			bottom = top + h;

			// Texture points
			var t1 = new SFML.Window.Vector2f(left,  top);
			var t2 = new SFML.Window.Vector2f(right, top);
			var t3 = new SFML.Window.Vector2f(right, bottom);
			var t4 = new SFML.Window.Vector2f(left,  bottom);

			// Store the vertex information
			_verts[index]   = new Vertex(v1, _color, t1);
			_verts[++index] = new Vertex(v2, _color, t2);
			_verts[++index] = new Vertex(v3, _color, t3);
			_verts[++index] = new Vertex(v4, _color, t4);

			xPos += glyph.Advance;
		}

		/// <summary>
		/// Updates the color of each vertex
		/// </summary>
		/// <param name="color">Color to set the vertices to</param>
		private void updateVertColor (SFML.Graphics.Color color)
		{
			for(var i = 0U; i < _verts.VertexCount; ++i)
			{
				var vert  = _verts[i];
				_verts[i] = new Vertex(vert.Position, color, vert.TexCoords);
			}
		}

		/// <summary>
		/// Draws the text onto a renderable object
		/// </summary>
		/// <param name="target">Render target</param>
		/// <param name="x">Offset along the x-axis to draw the text at</param>
		/// <param name="y">Offset along the y-axis to draw the text at</param>
		public void Draw (IRenderTarget target, float x, float y)
		{
			var rs = _rs;
			rs.Transform.Translate(x, y);
			target.Draw(_verts, rs);
		}

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the text has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Triggered when the text is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Releases resources held by the text
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Deconstructs the text
		/// </summary>
		~SimpleText ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the text and its resources
		/// </summary>
		/// <param name="disposing">Indicates whether inner resources should be disposed</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);
				if(disposing)
					_verts.Dispose();
			}
		}
		#endregion
	}
}
