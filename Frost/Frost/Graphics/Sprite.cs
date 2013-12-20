using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frost.Display;
using Frost.Modules.State;

namespace Frost.Graphics
{
	public class Sprite : IDrawableNode
	{
		private readonly SFML.Graphics.Sprite _sprite;

		internal SFML.Graphics.Sprite InternalSprite
		{
			get { return _sprite; }
		}

		public Sprite (Texture texture)
		{
			_sprite = new SFML.Graphics.Sprite(texture.InternalTexture);
		}

		public void DrawState (IDisplay display, int state)
		{
			display.Draw(_sprite);
		}
	}
}
