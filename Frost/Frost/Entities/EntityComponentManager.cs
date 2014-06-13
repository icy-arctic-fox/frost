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
		/// Maximum entity index.
		/// Length minus 1 of the sub-lists in <see cref="_componentsByType"/>.
		/// </summary>
		private int _maxEntityIndex = -1;

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
		/// <param name="entity">Entity to add</param>
		/// <exception cref="ArgumentNullException">The entity to add can't be null.</exception>
		public void AddEntity (Entity entity)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");

			extendComponentLists(entity.Index);
		}

		/// <summary>
		/// Removes an entity and all of its components
		/// </summary>
		/// <param name="entity">Entity to remove</param>
		/// <exception cref="ArgumentNullException">The entity to remove can't be null.</exception>
		public void RemoveEntity (Entity entity)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");

			removeComponentsAt(entity.Index);
		}

		/// <summary>
		/// Adds a component to an existing entity
		/// </summary>
		/// <param name="entity">Entity to attach the component to</param>
		/// <param name="component">Component to add to the entity</param>
		/// <exception cref="ArgumentNullException">The entity and component can't be null.</exception>
		public void AddComponent (Entity entity, IEntityComponent component)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");
			if(component == null)
				throw new ArgumentNullException("component");

			var entityIndex   = entity.Index;
			var componentType = component.GetType();
			var componentList = getComponentList(componentType, true);

			extendComponentLists(entityIndex); // Just in case the entity is new too
			componentList[entityIndex] = component;
		}

		/// <summary>
		/// Retrieves a component from an entity
		/// </summary>
		/// <typeparam name="T">Type of component</typeparam>
		/// <param name="entity">Entity to retrieve the component of</param>
		/// <returns>Corresponding component for the entity</returns>
		/// <exception cref="ArgumentNullException">The entity can't be null.</exception>
		public T GetComponent<T> (Entity entity) where T : IEntityComponent
		{
			return (T)GetComponent(entity, typeof(T));
		}

		/// <summary>
		/// Retrieves a component from an entity
		/// </summary>
		/// <param name="entity">Entity to retrieve the component of</param>
		/// <param name="componentType">Type of component to retrieve</param>
		/// <returns>Corresponding component for the entity</returns>
		/// <exception cref="ArgumentNullException">The entity and component type can't be null.</exception>
		public IEntityComponent GetComponent (Entity entity, Type componentType)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");
			if(componentType == null)
				throw new ArgumentNullException("componentType");

			var entityIndex   = entity.Index;
			var componentList = getComponentList(componentType);

			return (componentList != null) ? componentList[entityIndex] : null;
		}

		/// <summary>
		/// Removes a component from an existing entity
		/// </summary>
		/// <typeparam name="T">Type of component to remove from the entity</typeparam>
		/// <param name="entity">Entity to detach the component from</param>
		/// <returns>True if the component was removed or false if the entity is untracked or the entity doesn't have the component</returns>
		/// <exception cref="ArgumentNullException">The entity can't be null.</exception>
		public bool RemoveComponent<T> (Entity entity)
		{
			return RemoveComponent(entity, typeof(T));
		}

		/// <summary>
		/// Removes a component from an existing entity
		/// </summary>
		/// <param name="entity">Entity to detach the component from</param>
		/// <param name="componentType">Type of component to remove from the entity</param>
		/// <returns>True if the component was removed or false if the entity is untracked or the entity doesn't have the component</returns>
		/// <exception cref="ArgumentNullException">The entity and component type can't be null.</exception>
		public bool RemoveComponent (Entity entity, Type componentType)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");
			if(componentType == null)
				throw new ArgumentNullException("componentType");

			var entityIndex   = entity.Index;
			var componentList = getComponentList(componentType);

			if(componentList != null)
			{// Component is known about
				if(componentList[entityIndex] != null)
				{// Entity has the component
					componentList[entityIndex] = null;
					return true;
				}

				// else - component doesn't exist for the entity (fall through)
			}

			return false; // Component doesn't exist for any entity
		}

		/// <summary>
		/// Checks whether an entity has a component
		/// </summary>
		/// <typeparam name="T">Type of component</typeparam>
		/// <param name="entity">Entity to check</param>
		/// <returns>True if the entity has the component, false otherwise</returns>
		/// <exception cref="ArgumentNullException">The entity can't be null.</exception>
		public bool HasComponent<T> (Entity entity) where T : IEntityComponent
		{
			return HasComponent(entity, typeof(T));
		}

		/// <summary>
		/// Checks whether an entity has a component
		/// </summary>
		/// <param name="entity">Entity to check</param>
		/// <param name="componentType">Type of component</param>
		/// <returns>True if the entity has the component, false otherwise</returns>
		/// <exception cref="ArgumentNullException">The entity and component type can't be null.</exception>
		public bool HasComponent (Entity entity, Type componentType)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");
			if(componentType == null)
				throw new ArgumentNullException("componentType");

			var entityIndex   = entity.Index;
			var componentList = getComponentList(componentType);

			if(componentList != null) // Component is known about
				return componentList[entityIndex] != null; // Will be null if the entity doesn't have the component
			// TODO: Use bit flag arrays to track which components an entity has

			return false; // Component doesn't exist for any entity
		}

		/// <summary>
		/// Gets a list that will be updated with entity components
		/// </summary>
		/// <param name="componentType">Type of component</param>
		/// <returns>List of entity components</returns>
		internal IList<IEntityComponent> GetEntityComponentList (Type componentType)
		{
			return getComponentList(componentType, true).AsReadOnly();
		}

		/// <summary>
		/// Gets the index for a component type
		/// </summary>
		/// <param name="componentType">Type of component</param>
		/// <returns>Index of the component type or -1 if the type is unknown at this point</returns>
		private int getComponentTypeIndex (Type componentType)
		{
			var typeName = componentType.FullName;
			int index;
			if(_componentTypeMap.TryGetValue(typeName, out index))
				return index;
			return -1;
		}

		/// <summary>
		/// Gets the component list for a component type
		/// </summary>
		/// <param name="componentType">Type of component</param>
		/// <param name="create">Flag indicating whether the list should be created if it doesn't exist</param>
		/// <returns>The component list or null if it doesn't exist (and <paramref name="create"/> was false)</returns>
		private List<IEntityComponent> getComponentList (Type componentType, bool create = false)
		{
			var typeIndex = getComponentTypeIndex(componentType);
			if(typeIndex >= 0) // Type is known, list exists
				return _componentsByType[typeIndex];

			if(create)
			{// Type is new and a list should be created for it
				// Create the list
				var componentList = new List<IEntityComponent>(_maxEntityIndex);
				for(var i = 0; i <= _maxEntityIndex; ++i)
					componentList.Add(null);

				// Add it to the mapping
				typeIndex = _componentsByType.Count;
				_componentsByType.Add(componentList);
				_componentTypeMap.Add(componentType.FullName, typeIndex);

				return componentList;
			}

			return null; // Type unknown and don't create it
		}

		/// <summary>
		/// Extends the component lists to accommodate slots for entity components
		/// </summary>
		/// <param name="index">Index to ensure is available in the lists</param>
		private void extendComponentLists (int index)
		{
			if(index <= _maxEntityIndex)
				return; // Don't need to extend

			// Extend each component list
			for(var i = 0; i < _componentsByType.Count; ++i)
			{
				var componentList = _componentsByType[i];
				for(var j = _maxEntityIndex; j < index; ++j)
					componentList.Add(null);
			}

			_maxEntityIndex = index;
		}

		/// <summary>
		/// Removes all components at a specified entity index
		/// </summary>
		/// <param name="index">Index of the entity to remove components for</param>
		private void removeComponentsAt (int index)
		{
			if(index > _maxEntityIndex)
				return; // Don't need to remove anything, entity isn't in lists

			// Entity has its components stored in the lists
			for(var i = 0; i < _componentsByType.Count; ++i)
			{
				var componentList    = _componentsByType[i];
				componentList[index] = null;
			}
		}
	}
}
