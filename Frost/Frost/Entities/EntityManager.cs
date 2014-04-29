﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frost.Entities
{
	/// <summary>
	/// Manages a collection of entities and their components
	/// </summary>
	public class EntityManager
	{
		#region Registration

		private readonly Dictionary<ulong, Entity> _registeredEntities = new Dictionary<ulong, Entity>();

		/// <summary>
		/// Adds an entity to be tracked by the manager
		/// </summary>
		/// <param name="entity">Entity to register</param>
		/// <returns>True if the entity was registered or false if it has been previously registered with this manager</returns>
		public bool Register (Entity entity)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes an entity from being tracked by the manager
		/// </summary>
		/// <param name="entity">Entity to deregister</param>
		/// <returns>True if the entity was deregistered or false if it wasn't registered previously in this manager</returns>
		public bool Deregister (Entity entity)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Components

		private readonly List<object> _componentMaps = new List<object>();

		/// <summary>
		/// Retrieves a collection of entities that have a component
		/// </summary>
		/// <param name="componentType">Type of <see cref="IEntityComponent"/> to look for</param>
		/// <returns>Collection of entities</returns>
		public IEnumerable<Entity> GetEntitiesWith (Type componentType)
		{
			throw new NotImplementedException();
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
	}
}
