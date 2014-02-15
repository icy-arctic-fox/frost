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
		public FastText (Font font, uint size)
		{
			_font  = font.UnderlyingFont;
			_size  = size;
			_verts = new VertexArray(PrimitiveType.Quads);
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
			var vOff  = _size - rect.Height;

			// Quad points
			var v1 = new SFML.Window.Vector2f(pos,              vOff);
			var v2 = new SFML.Window.Vector2f(pos + rect.Width, vOff);
			var v3 = new SFML.Window.Vector2f(pos + rect.Width, vOff + rect.Height);
			var v4 = new SFML.Window.Vector2f(pos,              vOff + rect.Height);

			// Texture points
			var t1 = new SFML.Window.Vector2f(rect.Left,              rect.Top);
			var t2 = new SFML.Window.Vector2f(rect.Left + rect.Width, rect.Top);
			var t3 = new SFML.Window.Vector2f(rect.Left + rect.Width, rect.Top + rect.Height);
			var t4 = new SFML.Window.Vector2f(rect.Left,              rect.Top + rect.Height);

			// Store the vertex information
			// TODO: Add support for color
			_verts[index++] = new Vertex(v1, t1);
			_verts[index++] = new Vertex(v2, t2);
			_verts[index++] = new Vertex(v3, t3);
			_verts[index]   = new Vertex(v4, t4);

			pos += glyph.Advance;
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
