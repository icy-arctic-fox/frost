using System;
using SFML.Graphics;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Optimized for rendering text that changes frequently with a font that doesn't change at all
	/// </summary>
	public class FastText
	{
		private readonly uint _size;
		private readonly SFML.Graphics.Font _font;
		private readonly VertexArray _verts;

		/// <summary>
		/// Creates a new fast text object
		/// </summary>
		/// <param name="font">Font used to render the text</param>
		/// <param name="size">Font size</param>
		/// <param name="color">Font color</param>
		public FastText (Font font, uint size, Color color)
		{
			_font  = font.UnderlyingFont;
			_size  = size;
			_verts = new VertexArray(PrimitiveType.Quads);
			_color = color;
		}

		private string _text;

		/// <summary>
		/// Displayed text
		/// </summary>
		public string Text
		{
			get { return _text ?? String.Empty; }
			set
			{
				_text = value ?? String.Empty;
				constructVertices();
			}
		}

		private SFML.Graphics.Color _color;

		/// <summary>
		/// Color of the text
		/// </summary>
		public Color Color
		{
			get { return _color; }
			set
			{
				_color = value;
				updateVertColor(_color);
			}
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

			var pos = 0f;
			for(var i = 0; i < textLength; ++i)
			{// Construct each set of 4 vertices for each glyph
				var c = _text[i];
				var j = (uint)(i << 2); // x4
				constructFromGlyph(c, j, ref pos);
			}
		}

		/// <summary>
		/// Constructs a set of vertices from a glyph
		/// </summary>
		/// <param name="c">Character to get the texture of</param>
		/// <param name="index">Index of the first vertex</param>
		/// <param name="pos">X-offset to place the character at and position for the next character after returning</param>
		private void constructFromGlyph (char c, uint index, ref float pos)
		{
			var glyph = _font.GetGlyph(c, _size, false);
			var rect  = glyph.TextureRect;

			// Calculate vertex positions
			var h = rect.Height;
			var w = rect.Width;
			var left   = pos;
			var right  = left + w;
			var top    = _size - h;
			var bottom = top + h;

			// Vertex points
			var v1 = new SFML.Window.Vector2f(left,  top);
			var v2 = new SFML.Window.Vector2f(right, top);
			var v3 = new SFML.Window.Vector2f(right, bottom);
			var v4 = new SFML.Window.Vector2f(left,  bottom);

			// Calculate texture positions
			left   = rect.Left;
			right  = left + w;
			top    = rect.Top;
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

			pos += glyph.Advance;
		}

		/// <summary>
		/// Updates the color of each vertex
		/// </summary>
		/// <param name="color">Color to set the vertices to</param>
		private void updateVertColor (SFML.Graphics.Color color)
		{
			for(var i = 0U; i < _verts.VertexCount; ++i)
			{
				var vert = _verts[i];
				_verts[i] = new Vertex(vert.Position, color, vert.TexCoords);
			}
		}

		/// <summary>
		/// Draws the text onto a renderable object
		/// </summary>
		/// <param name="target">Render target</param>
		public void Draw (IRenderTarget target)
		{
			var rs = new RenderStates(_font.GetTexture(_size));
			target.Draw(_verts, rs);
		}
	}
}
