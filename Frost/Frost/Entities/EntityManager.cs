using System;
using System.Collections;
using System.Collections.Generic;
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
		/// <exception cref="InvalidOperationException">The <paramref name="entity"/> is already registered to another manager.</exception>
		public bool Register (Entity entity)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");

			if(entity.Registered)
			{// Entity is already registered to a manager
				if(entity.Owner == this)
					return false; // Already registered to this manager
				throw new InvalidOperationException("The entity is already registered to another manager."); // Registered to another manager
			}

			// Not registered at all, register to this manager
			lock(_registeredEntities)
			{
				Guid id;
				int index;
				getNextAvailable(out id, out index);
				_registeredEntities.Add(id, entity);
				entity.SetRegistrationInfo(this, id, index);
				_componentManager.AddEntity(entity);
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
			if(entity.Owner != this)
				return false; // Entity isn't registered to this manager

			// The entity is registered to this manager
			var id = entity.Id;
			lock(_registeredEntities)
			{
				_registeredEntities.Remove(id);
				_freeIndices.Release(entity.Index);
				entity.ClearRegistrationInfo();
				_componentManager.RemoveEntity(entity);
			}

			OnDeregister(new EntityEventArgs(entity));
			return true;
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

		private readonly EntityComponentManager _componentManager = new EntityComponentManager();

		/// <summary>
		/// Retrieves an object that will be able to pull a component from the entities tracked by the manager
		/// </summary>
		/// <typeparam name="T">Type of entity components to map</typeparam>
		/// <returns>A mapping object</returns>
		public EntityComponentMap<T> GetComponentMap<T> () where T : IEntityComponent
		{
			var componentType = typeof(T);
			var componentList = _componentManager.GetEntityComponentList(componentType);
			var mapList       = (IList<T>)componentList; // TODO: Will this cast work?
			return new EntityComponentMap<T>(mapList);
			// TODO: Reuse maps by saving them in a dictionary?
		}

		/// <summary>
		/// Checks whether an entity has a component
		/// </summary>
		/// <typeparam name="T">Type of component</typeparam>
		/// <param name="entity">Entity to check</param>
		/// <returns>True if the entity has the component, false otherwise</returns>
		/// <exception cref="ArgumentNullException">The entity can't be null.</exception>
		internal bool HasComponent<T> (Entity entity) where T : IEntityComponent
		{
			return _componentManager.HasComponent<T>(entity);
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
