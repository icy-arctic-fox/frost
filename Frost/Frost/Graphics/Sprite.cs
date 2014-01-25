﻿using Frost.Display;

namespace Frost.Graphics
{
	/// <summary>
	/// Most basic type used to display a 2D graphic on the screen
	/// </summary>
	public class Sprite : Object2D
	{
		/// <summary>
		/// Underlying SFML implementation of the sprite
		/// </summary>
		private readonly SFML.Graphics.Sprite _sprite;

		/// <summary>
		/// Creates a new sprite
		/// </summary>
		/// <param name="texture">Texture to apply to the sprite</param>
		public Sprite (Texture texture)
		{
			_sprite = new SFML.Graphics.Sprite(texture.InternalTexture);
		}

		/// <summary>
		/// Underlying implementation that draws the sprite
		/// </summary>
		/// <param name="display">Display to draw on</param>
		/// <param name="transform">Transformation to apply to the sprite</param>
		protected override void DrawObject (IDisplay display, SFML.Graphics.RenderStates transform)
		{
			display.Draw(_sprite, transform);
		}
	}
}
