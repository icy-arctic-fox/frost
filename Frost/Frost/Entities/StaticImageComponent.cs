using Frost.Graphics;

namespace Frost.Entities
{
	/// <summary>
	/// Displays a static 2D image
	/// </summary>
	public class StaticImageComponent : IEntityComponent
	{
		private readonly Texture _texture;

		/// <summary>
		/// Creates a new component for an entity to exist in 2D space
		/// </summary>
		public StaticImageComponent (Texture texture)
		{
			_texture = texture;
		}

		/// <summary>
		/// Texture displayed for the entity
		/// </summary>
		public Texture Texture
		{
			get { return _texture; }
		}

		/// <summary>
		/// Creates a copy of the information in the component
		/// </summary>
		/// <returns>Copy of the component</returns>
		public IEntityComponent CloneComponent ()
		{
			return new StaticImageComponent(_texture);
		}
	}
}
