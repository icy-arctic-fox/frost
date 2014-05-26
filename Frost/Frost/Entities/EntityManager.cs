using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Frost.Utility;

namespace Frost.Entities
{
	/// <summary>
	/// Manages a collection of entities and their components
	/// </summary>
	public class EntityManager : IEnumerable<Entity>
	{
		#region Registration

		private readonly Dictionary<ulong, Entity> _registeredEntities = new Dictionary<ulong, Entity>();
		private ulong _nextId;

		/// <summary>
		/// Gets the next usable entity ID
		/// </summary>
		/// <returns>Next available entity ID</returns>
		private ulong getNextAvailableId ()
		{
			ulong id;
			lock(_registeredEntities)
			{
				do
				{
					id = unchecked(_nextId++);
				} while(_registeredEntities.ContainsKey(id));
			}
			return id;
		}

		/// <summary>
		/// Adds an entity to be tracked by the manager
		/// </summary>
		/// <param name="entity">Entity to register</param>
		/// <returns>True if the entity was registered or false if it has been previously registered with this manager</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="entity"/> to register can't be null.</exception>
		/// <exception cref="ArgumentException">The <paramref name="entity"/> can't be already registered to another manager.</exception>
		public bool Register (Entity entity)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");

			lock(_registeredEntities)
			{
				if(entity.Registered)
				{
					Entity prevEntity;
					if(_registeredEntities.TryGetValue(entity.Id, out prevEntity) && ReferenceEquals(prevEntity, entity))
						return false; // Already registered to this manager
					throw new ArgumentException("entity"); // Registered to another manager
				}

				// Not registered at all, register to this manager
				var id = getNextAvailableId();
				_registeredEntities.Add(id, entity);
				entity.SetId(id);
			}

			OnRegister(new EntityEventArgs(entity));
			return true;
		}

		/// <summary>
		/// Triggered when an entity is registered
		/// </summary>
		public event EventHandler<EntityEventArgs> EntityRegistered;

		/// <summary>
		/// Called when an entity is registered
		/// </summary>
		/// <param name="args">Event arguments</param>
		/// <remarks>This method triggers the <see cref="EntityRegistered"/> event.</remarks>
		protected virtual void OnRegister (EntityEventArgs args)
		{
			EntityRegistered.NotifyThreadedSubscribers(this, args);
		}

		/// <summary>
		/// Removes an entity from being tracked by the manager
		/// </summary>
		/// <param name="entity">Entity to deregister</param>
		/// <returns>True if the entity was deregistered or false if it wasn't registered previously in this manager</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="entity"/> to deregister can't be null.</exception>
		public bool Deregister (Entity entity)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");
			if(!entity.Registered)
				return false;

			var found = false;
			var id = entity.Id;
			lock(_registeredEntities)
			{
				Entity prevEntity;
				if(_registeredEntities.TryGetValue(id, out prevEntity))
					if(ReferenceEquals(prevEntity, entity))
					{// Entity is registered to this manager, remove it
						_registeredEntities.Remove(id);
						found = true;
					}
			}

			if(found)
				OnDeregister(new EntityEventArgs(entity));
			return found;
		}

		/// <summary>
		/// Triggered when an entity is registered
		/// </summary>
		public event EventHandler<EntityEventArgs> EntityDeregistered;

		/// <summary>
		/// Called when an entity is deregistered
		/// </summary>
		/// <param name="args">Event arguments</param>
		/// <remarks>This method triggers the <see cref="EntityDeregistered"/> event.</remarks>
		protected virtual void OnDeregister (EntityEventArgs args)
		{
			EntityDeregistered.NotifyThreadedSubscribers(this, args);
		}
		#endregion

		#region Components

		/// <summary>
		/// Collection of tuples containing (1) the type of component map and (2) a <see cref="EntityComponentMap{T}"/>
		/// </summary>
		private readonly List<Tuple<Type, object>> _componentMaps = new List<Tuple<Type, object>>();

		/// <summary>
		/// Retrieves the index to be used for a component
		/// </summary>
		/// <param name="componentType">Type of <see cref="IEntityComponent"/> to get the index of</param>
		/// <returns>Index corresponding to where the component is stored</returns>
		/// <exception cref="ArgumentNullException">The type of entity component (<paramref name="componentType"/>) to get the index of can't be null.</exception>
		internal int GetComponentIndex (Type componentType)
		{
			if(ReferenceEquals(componentType, null))
				throw new ArgumentNullException("componentType");

			lock(_componentMaps)
			{
				var index = findComponentType(componentType);

				if(index >= 0) // Found it
					return index;

				// Didn't find the component type, create it
				var map   = createComponentMap(componentType);
				var tuple = new Tuple<Type, object>(componentType, map);
				_componentMaps.Add(tuple);

				return _componentMaps.Count - 1;
			}
		}

		/// <summary>
		/// Retrieves a collection of entities that have a component
		/// </summary>
		/// <param name="componentType">Type of <see cref="IEntityComponent"/> to look for</param>
		/// <returns>Collection of entities</returns>
		/// <exception cref="ArgumentNullException">The type of component to look for can't be null.</exception>
		public IEnumerable<Entity> GetEntitiesWith (Type componentType)
		{
			if(componentType == null)
				throw new ArgumentNullException("componentType");

			var entities = new List<Entity>();
			lock(_registeredEntities)
				entities.AddRange(_registeredEntities.Values.Where(entity => entity.HasComponent(componentType)));
			return entities.AsReadOnly();
		}

		/// <summary>
		/// Retrieves an object that will be able to pull a component from the entities tracked by the manager
		/// </summary>
		/// <typeparam name="T">Type of entity components to map</typeparam>
		/// <returns>A mapping object</returns>
		public EntityComponentMap<T> GetComponentMap<T> () where T : IEntityComponent
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Attempts to find an existing component type in <see cref="_componentMaps"/>
		/// </summary>
		/// <param name="componentType">Type of component to look for</param>
		/// <returns>Index of the found item in <see cref="_componentMaps"/>,
		/// or -1 if the component wasn't found</returns>
		private int findComponentType (Type componentType)
		{
			for(var i = 0; i < _componentMaps.Count; ++i)
			{
				var tuple = _componentMaps[i];
				var type  = tuple.Item1;
				if(Portability.CompareTypes(componentType, type))
					return i; // Found the component type
			}

			return -1; // Not found
		}

		private object createComponentMap (Type componentType)
		{
			throw new NotImplementedException();
		}
		#endregion

		/// <summary>
		/// Number of registered entities
		/// </summary>
		public int Count
		{
			get
			{
				lock(_registeredEntities)
					return _registeredEntities.Count;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the entities
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the registered entities</returns>
		public IEnumerator<Entity> GetEnumerator ()
		{
			lock(_registeredEntities)
				return _registeredEntities.Values.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the entities
		/// </summary>
		/// <returns>An enumerator object that can be used to iterate through the registered entities</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}
	}
}
