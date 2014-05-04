﻿using System;
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

		/// <summary>
		/// Adds an entity to be tracked by the manager
		/// </summary>
		/// <param name="entity">Entity to register</param>
		/// <returns>True if the entity was registered or false if it has been previously registered with this manager</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="entity"/> to register can't be null.</exception>
		public bool Register (Entity entity)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");

			throw new NotImplementedException();
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

			throw new NotImplementedException();
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

		private readonly List<object> _componentMaps = new List<object>();

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
