using System;
using System.Collections.Generic;

namespace Frost.Entities
{
	/// <summary>
	/// Unspecialized factory that constructs entities.
	/// Multiple entities with the same components can be constructed for the same entity manager.
	/// </summary>
	public class EntityFactory
	{
		private readonly EntityManager _manager;
		private readonly List<IEntityComponent> _components = new List<IEntityComponent>();

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
			if(component == null)
				throw new ArgumentNullException("component");
			
			_components.Add(component);
		}

		/// <summary>
		/// Constructs and registers an entity
		/// </summary>
		/// <returns>Newly constructed entity</returns>
		public Entity Construct ()
		{
			var entity = new Entity();

			// Add each component
			for(var i = 0; i < _components.Count; ++i)
			{
				var component = _components[i].CloneComponent();
				var index     = _manager.GetComponentIndex(component.GetType());
				entity.AddComponent(index, component);
			}

			// Register and return the entity
			_manager.Register(entity);
			return entity;
		}
	}
}
