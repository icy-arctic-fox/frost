using System;
using System.Collections.Generic;

namespace Frost.Entities
{
	/// <summary>
	/// Holds information and provides access to the components that entities contain.
	/// Component data is not stored in the <see cref="Entity"/> class.
	/// Instead, it is held here, in a format more efficient for bulk processing.
	/// </summary>
	internal class EntityComponentManager
	{
		/// <summary>
		/// Collection of components organized by type and then by entity index
		/// </summary>
		private readonly List<List<IEntityComponent>> _componentsByType = new List<List<IEntityComponent>>();

		/// <summary>
		/// Maps component type names to their index in <see cref="_componentsByType"/>
		/// </summary>
		private readonly Dictionary<string, int> _componentTypeMap = new Dictionary<string, int>();

		/// <summary>
		/// Adds an empty entity (an entity with no components)
		/// </summary>
		/// <param name="e">Entity to add</param>
		public void AddEntity (Entity e)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes an entity and all of its components
		/// </summary>
		/// <param name="e">Entity to remove</param>
		/// <returns>True if the entity was removed or false if the entity is untracked</returns>
		public bool RemoveEntity (Entity e)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds a component to an existing entity
		/// </summary>
		/// <param name="e">Entity to attach the component to</param>
		/// <param name="component">Component to add to the entity</param>
		public void AddComponent (Entity e, IEntityComponent component)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a component from an entity
		/// </summary>
		/// <typeparam name="T">Type of component</typeparam>
		/// <param name="e">Entity to retrieve the component of</param>
		/// <returns>Corresponding component for the entity</returns>
		public T GetComponent<T> (Entity e)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a component from an entity
		/// </summary>
		/// <param name="e">Entity to retrieve the component of</param>
		/// <param name="componentType">Type of component to retrieve</param>
		/// <returns>Corresponding component for the entity</returns>
		public IEntityComponent GetComponent (Entity e, Type componentType)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a component from an existing entity
		/// </summary>
		/// <param name="e">Entity to detach the component from</param>
		/// <param name="componentType">Type of component to remove from the entity</param>
		/// <returns>True if the component was removed or false if the entity is untracked or the entity doesn't have the component</returns>
		public bool RemoveComponent (Entity e, Type componentType)
		{
			throw new NotImplementedException();
		}
	}
}
