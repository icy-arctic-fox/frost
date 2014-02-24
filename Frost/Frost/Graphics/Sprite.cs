using Frost.Display;

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
		/// Width of the sprite in pixels (before scaling)
		/// </summary>
		public override float Width
		{
			get { return _sprite.Texture.Size.X; }
		}

		/// <summary>
		/// Height of the sprite in pixels (before scaling)
		/// </summary>
		public override float Height
		{
			get { return _sprite.Texture.Size.Y; }
		}

		public float SpeedX { get; set; }
		public float SpeedY { get; set; }

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
		/// <param name="t">Interpolation value</param>
		protected override void DrawObject (IDisplay display, SFML.Graphics.RenderStates transform, double t)
		{
			var xOff = (float)(SpeedX * t);
			var yOff = (float)(SpeedY * t);
			transform.Transform.Translate(xOff, yOff);
			display.Draw(_sprite, transform);
		}
	}
}
