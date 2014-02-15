namespace Frost.Graphics.Text
{
	/// <summary>
	/// Optimized for rendering text that changes frequently with a font that doesn't change at all
	/// </summary>
	public class FastText
	{
		private readonly uint _size;
		private readonly SFML.Graphics.Font _font;

		public FastText (uint size, Font font)
		{
			_size = size;
			_font = font.UnderlyingFont;
		}

		public string Text { get; set; }

		public void Draw (IRenderTarget target)
		{
			var point = new SFML.Window.Vector2f();
			for(var i = 0; i < Text.Length; ++i)
				drawCharacter(Text[i], target, ref point, out point);
		}

		private void drawCharacter (char c, IRenderTarget target, ref SFML.Window.Vector2f curPoint,
									out SFML.Window.Vector2f nextPoint)
		{
			var glyph = _font.GetGlyph(c, _size, false);
			var transform = SFML.Graphics.RenderStates.Default;
			transform.Texture = _font.GetTexture(_size);
			transform.Transform.Translate(curPoint);
			var verts = new SFML.Graphics.Vertex[4];
			var rect = glyph.TextureRect;
			verts[0] = new SFML.Graphics.Vertex(new SFML.Window.Vector2f(0, 0), new SFML.Window.Vector2f(rect.Left, rect.Top));
			verts[1] = new SFML.Graphics.Vertex(new SFML.Window.Vector2f(rect.Width, 0), new SFML.Window.Vector2f(rect.Left + rect.Width, rect.Top));
			verts[2] = new SFML.Graphics.Vertex(new SFML.Window.Vector2f(rect.Width, rect.Height), new SFML.Window.Vector2f(rect.Left + rect.Width, rect.Top + rect.Height));
			verts[3] = new SFML.Graphics.Vertex(new SFML.Window.Vector2f(0, rect.Height), new SFML.Window.Vector2f(rect.Left, rect.Top + rect.Height));
			target.Draw(verts, transform);
			nextPoint = curPoint;
			nextPoint.X += glyph.Advance;
		}
	}
}
