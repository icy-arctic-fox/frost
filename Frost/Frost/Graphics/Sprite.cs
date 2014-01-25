using Frost.Display;
using Frost.Logic;

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
		/// Underlying SFML implementation of the sprite
		/// </summary>
		internal SFML.Graphics.Sprite InternalSprite
		{
			get { return _sprite; }
		}

		/// <summary>
		/// Creates a new sprite
		/// </summary>
		/// <param name="texture">Texture to apply to the sprite</param>
		public Sprite (Texture texture)
		{
			_sprite = new SFML.Graphics.Sprite(texture.InternalTexture);
		}
	}
}
