namespace Frost.Entities
{
	public class StaticImageSubsystem : IRenderSubsystem
	{
		private readonly SFML.Graphics.Sprite _sprite = new SFML.Graphics.Sprite();

		private readonly ComponentMap<Positional2DComponent> _pos2DMap;
		private readonly ComponentMap<StaticImageComponent> _imgMap;

		public StaticImageSubsystem (EntityManager manager)
		{
			_pos2DMap = manager.GetComponentMap<Positional2DComponent>();
			_imgMap   = manager.GetComponentMap<StaticImageComponent>();
		}

		/// <summary>
		/// Determines whether the subsystem can process the entity
		/// </summary>
		/// <param name="entity">Entity to check</param>
		/// <returns>True if the entity can be processed by the subsystem</returns>
		public bool CanProcess (Entity entity)
		{
			return entity.HasComponent<Positional2DComponent>() && entity.HasComponent<StaticImageComponent>();
		}

		/// <summary>
		/// Processes a single entity
		/// </summary>
		/// <param name="entity">Entity to process</param>
		/// <param name="args">Render information</param>
		public void Process (Entity entity, FrameDrawEventArgs args)
		{
			var pos2D = _pos2DMap.Get(entity);
			var img   = _imgMap.Get(entity);
			var pos   = pos2D.States[args.StateIndex];

			_sprite.Texture  = img.Texture.InternalTexture;
			_sprite.Position = new SFML.Window.Vector2f(pos.X, pos.Y);
			args.Display.Draw(_sprite);
		}
	}
}
