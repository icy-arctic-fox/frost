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
		/// <param name="e">Entity to add</param>
		/// <exception cref="ArgumentNullException">The entity to add can't be null.</exception>
		public void AddEntity (Entity e)
		{
			if(e == null)
				throw new ArgumentNullException("e");

			extendComponentLists(e.Index);
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
		/// Removes an entity and all of its components
		/// </summary>
		/// <param name="e">Entity to remove</param>
		/// <exception cref="ArgumentNullException">The entity to remove can't be null.</exception>
		public void RemoveEntity (Entity e)
		{
			if(e == null)
				throw new ArgumentNullException("e");

			removeComponentsAt(e.Index);
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

		/// <summary>
		/// Adds a component to an existing entity
		/// </summary>
		/// <param name="e">Entity to attach the component to</param>
		/// <param name="component">Component to add to the entity</param>
		/// <exception cref="ArgumentNullException">The entity and component can't be null.</exception>
		public void AddComponent (Entity e, IEntityComponent component)
		{
			if(e == null)
				throw new ArgumentNullException("e");
			if(component == null)
				throw new ArgumentNullException("component");

			var entityIndex   = e.Index;
			var componentType = component.GetType();
			var typeIndex     = getComponentTypeIndex(componentType);
			List<IEntityComponent> componentList;

			extendComponentLists(entityIndex);

			if(typeIndex >= 0)
			{// Component is known about
				componentList = _componentsByType[typeIndex];
			}
			else
			{// New type of component
				componentList = new List<IEntityComponent>(_maxEntityIndex);
				for(var i = 0; i < _maxEntityIndex; ++i)
					componentList.Add(null);
				_componentTypeMap[componentType.FullName] = _componentsByType.Count;
				_componentsByType.Add(componentList);
			}

			componentList[entityIndex] = component;
		}

		/// <summary>
		/// Retrieves a component from an entity
		/// </summary>
		/// <typeparam name="T">Type of component</typeparam>
		/// <param name="e">Entity to retrieve the component of</param>
		/// <returns>Corresponding component for the entity</returns>
		/// <exception cref="ArgumentNullException">The entity can't be null.</exception>
		public T GetComponent<T> (Entity e) where T : IEntityComponent
		{
			return (T)GetComponent(e, typeof(T));
		}

		/// <summary>
		/// Retrieves a component from an entity
		/// </summary>
		/// <param name="e">Entity to retrieve the component of</param>
		/// <param name="componentType">Type of component to retrieve</param>
		/// <returns>Corresponding component for the entity</returns>
		/// <exception cref="ArgumentNullException">The entity and component type can't be null.</exception>
		public IEntityComponent GetComponent (Entity e, Type componentType)
		{
			if(e == null)
				throw new ArgumentNullException("e");
			if(componentType == null)
				throw new ArgumentNullException("componentType");

			var entityIndex = e.Index;
			var typeIndex   = getComponentTypeIndex(componentType);

			if(typeIndex >= 0)
			{// Component is known about
				var componentList = _componentsByType[typeIndex];
				return componentList[entityIndex];
			}

			return null; // Component doesn't exist for any entity
		}

		/// <summary>
		/// Removes a component from an existing entity
		/// </summary>
		/// <typeparam name="T">Type of component to remove from the entity</typeparam>
		/// <param name="e">Entity to detach the component from</param>
		/// <returns>True if the component was removed or false if the entity is untracked or the entity doesn't have the component</returns>
		/// <exception cref="ArgumentNullException">The entity can't be null.</exception>
		public bool RemoveComponent<T> (Entity e)
		{
			return RemoveComponent(e, typeof(T));
		}

		/// <summary>
		/// Removes a component from an existing entity
		/// </summary>
		/// <param name="e">Entity to detach the component from</param>
		/// <param name="componentType">Type of component to remove from the entity</param>
		/// <returns>True if the component was removed or false if the entity is untracked or the entity doesn't have the component</returns>
		/// <exception cref="ArgumentNullException">The entity and component type can't be null.</exception>
		public bool RemoveComponent (Entity e, Type componentType)
		{
			if(e == null)
				throw new ArgumentNullException("e");
			if(componentType == null)
				throw new ArgumentNullException("componentType");

			var entityIndex = e.Index;
			var typeIndex   = getComponentTypeIndex(componentType);

			if(typeIndex >= 0)
			{// Component is known about
				var componentList = _componentsByType[typeIndex];
				if(componentList[entityIndex] != null)
				{// Entity has the component
					componentList[entityIndex] = null;
					return true;
				}
			}

			return false; // Component doesn't exist for any entity
		}

		/// <summary>
		/// Checks whether an entity has a component
		/// </summary>
		/// <typeparam name="T">Type of component</typeparam>
		/// <param name="e">Entity to check</param>
		/// <returns>True if the entity has the component, false otherwise</returns>
		/// <exception cref="ArgumentNullException">The entity can't be null.</exception>
		public bool HasComponent<T> (Entity e) where T : IEntityComponent
		{
			return HasComponent(e, typeof(T));
		}

		/// <summary>
		/// Checks whether an entity has a component
		/// </summary>
		/// <param name="e">Entity to check</param>
		/// <param name="componentType">Type of component</param>
		/// <returns>True if the entity has the component, false otherwise</returns>
		/// <exception cref="ArgumentNullException">The entity and component type can't be null.</exception>
		public bool HasComponent (Entity e, Type componentType)
		{
			if(e == null)
				throw new ArgumentNullException("e");
			if(componentType == null)
				throw new ArgumentNullException("componentType");

			var entityIndex = e.Index;
			var typeIndex   = getComponentTypeIndex(componentType);

			if(typeIndex >= 0)
			{// Component is known about
				var componentList = _componentsByType[typeIndex];
				return componentList[entityIndex] != null; // Will be null if the entity doesn't have the component
				// TODO: Use bit flag arrays to track which components an entity has
			}

			return false; // Component doesn't exist for any entity
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
	}
}
