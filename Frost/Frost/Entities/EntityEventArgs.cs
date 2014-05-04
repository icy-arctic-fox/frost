using System;

namespace Frost.Entities
{
	/// <summary>
	/// Describes an event that involves an entity
	/// </summary>
	public class EntityEventArgs : EventArgs
	{
		private readonly Entity _entity;

		/// <summary>
		/// Entity involved with the event
		/// </summary>
		public Entity Entity
		{
			get { return _entity; }
		}

		/// <summary>
		/// Creates an entity event description
		/// </summary>
		/// <param name="entity">Entity involved with the event</param>
		/// <exception cref="ArgumentNullException">The <paramref name="entity"/> involved with the event can't be null.</exception>
		public EntityEventArgs (Entity entity)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");

			_entity = entity;
		}
	}
}
