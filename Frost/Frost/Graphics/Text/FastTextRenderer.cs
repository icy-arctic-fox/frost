using System;
using System.Collections.Generic;
using System.Text;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Optimized for rendering text that changes frequently with a font that doesn't change at all
	/// </summary>
	public class FastTextRenderer
	{
		private readonly uint _size;
		private readonly SFML.Graphics.Font _font;
		private readonly SFML.Graphics.Sprite _sprite;

		public FastTextRenderer (uint size, Font font)
		{
			_size = size;
			_font = font.UnderlyingFont;
			_sprite = new SFML.Graphics.Sprite(_font.GetTexture(size));
		}

		public string Text { get; set; }

		public void Draw (IRenderTarget target)
		{
			var point = new SFML.Window.Vector2f();
			foreach(var c in Text)
				drawCharacter(c, target, ref point, out point);
		}

		private void drawCharacter (char c, IRenderTarget target, ref SFML.Window.Vector2f curPoint,
									out SFML.Window.Vector2f nextPoint)
		{
			var glyph = _font.GetGlyph(c, _size, false);
			_sprite.TextureRect = glyph.TextureRect;
			var transform = new SFML.Graphics.RenderStates();
			transform.Transform.Translate(curPoint);
			target.Draw(_sprite, transform);
			nextPoint = curPoint;
			nextPoint.X += glyph.Bounds.Width;
		}
	}
}
