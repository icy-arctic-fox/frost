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

		private readonly Dictionary<Guid, Entity> _registeredEntities = new Dictionary<Guid, Entity>();
		private readonly FreeList _freeIndices = new FreeList();

		/// <summary>
		/// Gets a new entity ID and next usable entity index
		/// </summary>
		/// <param name="id">Unique entity ID</param>
		/// <param name="index">Next usable entity index</param>
		private void getNextAvailable (out Guid id, out int index)
		{
			lock(_registeredEntities)
			{
				do
				{
					id = Guid.NewGuid();
				} while(_registeredEntities.ContainsKey(id));
				index = _freeIndices.GetNext();
			}
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
				Guid id;
				int index;
				getNextAvailable(out id, out index);
				_registeredEntities.Add(id, entity);
				entity.SetRegistrationInfo(this, id, index);
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
						_freeIndices.Release(entity.Index);
						entity.ClearRegistrationInfo();
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
				throw new NotImplementedException(); // entities.AddRange(_registeredEntities.Values.Where(entity => entity.HasComponent(componentType))));
			return entities.AsReadOnly();
		}

		/// <summary>
		/// Retrieves an object that will be able to pull a component from the entities tracked by the manager
		/// </summary>
		/// <typeparam name="T">Type of entity components to map</typeparam>
		/// <returns>A mapping object</returns>
		public EntityComponentMap<T> GetComponentMap<T> () where T : IEntityComponent
		{
			lock(_componentMaps)
			{
				var index = 0; // TODO: findComponentType(typeof(T));

				if(index >= 0)
				{// Found it
					var tuple = _componentMaps[index];
					var map   = tuple.Item2;
					return (EntityComponentMap<T>)map;
				}

				// Not found, create it
				return createComponentMap<T>();
			}
		}

		/// <summary>
		/// Creates an entity component map using the next index from <see cref="_componentMaps"/>
		/// </summary>
		/// <typeparam name="T">An <see cref="IEntityComponent"/> to map to</typeparam>
		/// <returns>A <see cref="EntityComponentMap{T}"/> of the corresponding type</returns>
		private EntityComponentMap<T> createComponentMap<T> () where T : IEntityComponent
		{
			// Create the map
			var index = _componentMaps.Count;
			var map   = new EntityComponentMap<T>(index);

			// Add the map to the collection
			var tuple = new Tuple<Type, object>(typeof(T), map);
			_componentMaps.Add(tuple);

			return map;
		}

		/// <summary>
		/// Creates an entity component map using the next index from <see cref="_componentMaps"/>
		/// </summary>
		/// <param name="componentType">Type of component to map for</param>
		/// <returns>A <see cref="EntityComponentMap{T}"/> of the corresponding type</returns>
		private object createComponentMap (Type componentType)
		{
			// Get type information
			var baseType    = typeof(EntityComponentMap<>);
			var genericType = baseType.MakeGenericType(componentType);

			// Create the map
			var index = _componentMaps.Count;
			var map   = Activator.CreateInstance(genericType, index);

			// Add the map to the collection
			var tuple = new Tuple<Type, object>(componentType, map);
			_componentMaps.Add(tuple);

			return map;
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
