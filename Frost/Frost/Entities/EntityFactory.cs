using System;

namespace Frost.Entities
{
	/// <summary>
	/// Unspecialized factory that constructs entities.
	/// Multiple entities with the same components can be constructed for the same entity manager.
	/// </summary>
	public class EntityFactory
	{
		private readonly EntityManager _manager;

		/// <summary>
		/// Creates a factory to construct entities
		/// </summary>
		/// <param name="manager">Manager to construct and register the entities for</param>
		/// <exception cref="ArgumentNullException">The <paramref name="manager"/> to construct entities for can't be null.</exception>
		public EntityFactory (EntityManager manager)
		{
			if(manager == null)
				throw new ArgumentNullException("manager");

			_manager = manager;
		}

		/// <summary>
		/// Adds a component that will be a part of the constructed entity
		/// </summary>
		/// <param name="component">Component to add to the entity</param>
		/// <exception cref="ArgumentNullException">The <paramref name="component"/> to add can't be null.</exception>
		public void AddComponent (IEntityComponent component)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Constructs and registers an entity
		/// </summary>
		/// <returns>Newly constructed entity</returns>
		public Entity Construct ()
		{
			throw new NotImplementedException();
		}
	}
}
