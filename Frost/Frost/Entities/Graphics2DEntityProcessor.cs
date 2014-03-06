using System;
using Frost.Graphics;

namespace Frost.Entities
{
	/// <summary>
	/// Draws 2D graphical entities
	/// </summary>
	public class Graphics2DEntityProcessor : IEntityRenderer
	{
		private readonly EntityComponentMap<Position2DEntityComponent> _position2DMap;
		private readonly EntityComponentMap<TexturedEntityComponent> _texturedMap;

		private readonly SFML.Graphics.Sprite _sprite = new SFML.Graphics.Sprite();

		/// <summary>
		/// Creates a new 2D entity graphics processor
		/// </summary>
		/// <param name="manager">Manager containing the entities to process</param>
		/// <exception cref="ArgumentNullException">The entity <paramref name="manager"/> can't be null.</exception>
		public Graphics2DEntityProcessor (Entity.Manager manager)
		{
			if(manager == null)
				throw new ArgumentNullException("manager");

			_position2DMap = manager.GetComponentMap<Position2DEntityComponent>();
			_texturedMap   = manager.GetComponentMap<TexturedEntityComponent>();
		}

		/// <summary>
		/// Draws a 2D entity
		/// </summary>
		/// <param name="e">Entity to process</param>
		/// <param name="target">Target to draw the entity to</param>
		/// <param name="stateIndex">Index of the entity state to draw</param>
		/// <param name="t">Amount of interpolation</param>
		public void DrawEntity (Entity e, IRenderTarget target, int stateIndex, double t)
		{
			var posComp = _position2DMap.GetComponent(e);
			var texComp = _texturedMap.GetComponent(e);

			if(posComp != null && texComp != null)
			{// Process the entity if it has the components
				var pos = posComp.States[stateIndex];
				var tex = texComp.States[stateIndex];

				_sprite.Texture  = texComp.Texture.InternalTexture;
				_sprite.Position = new SFML.Window.Vector2f(pos.X, pos.Y);
				target.Draw(_sprite);
			}
		}
	}
}
