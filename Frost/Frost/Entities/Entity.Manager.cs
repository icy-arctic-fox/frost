using System;
using System.Collections.Generic;

namespace Frost.Entities
{
	public partial class Entity
	{
		/// <summary>
		/// Tracks game entities
		/// </summary>
		/// <seealso cref="Entity"/>
		public class Manager
		{
			private readonly Dictionary<ulong, Entity> _registeredEntities = new Dictionary<ulong, Entity>();
			private ulong _nextId;

			public Entity CreateEntity ()
			{
				lock(_registeredEntities)
				{
					var id = getNextAvailableId();
					var e = new Entity(id);
					registerEntity(e);
					return e;
				}
			}

			/// <summary>
			/// Retrieves the next available ID for an entity
			/// </summary>
			private ulong getNextAvailableId ()
			{
				lock(_registeredEntities)
				{
					while(_registeredEntities.ContainsKey(_nextId))
						++_nextId;
					_registeredEntities.Add(_nextId, null); // Reserve the ID by setting the value to null
					return _nextId;
				}
			}

			/// <summary>
			/// Assigns an entity object to its ID so that it can be accessed from the manager
			/// </summary>
			/// <param name="e">Entity to register</param>
			/// <exception cref="InvalidOperationException">Thrown if another entity with the same <see cref="Entity.Id"/> has already been registered</exception>
			/// <exception cref="ArgumentException">Thrown if the <see cref="Entity.Id"/> of <paramref name="e"/> has not been previously reserved by getting an ID from <see cref="NextAvailableId"/></exception>
			private void registerEntity (Entity e)
			{
				var id = e.Id;
				lock(_registeredEntities)
				{
					Entity existing;
					if(_registeredEntities.TryGetValue(id, out existing))
					{ // ID has been reserved
						if(ReferenceEquals(existing, null)) // No other entity assigned to this ID, register it
							_registeredEntities[id] = e;
						else if(!ReferenceEquals(existing, e)) // Another entity already has this ID
							throw new InvalidOperationException("The ID is already assigned to another entity.");
					}
					else // ID has not been reserved
						throw new ArgumentException("The entity does not have a reserved ID.");
				}
			}

			/// <summary>
			/// Removes an entity from the manager so that it is no longer tracked
			/// </summary>
			/// <param name="e">Entity to deregister</param>
			/// <exception cref="InvalidOperationException">Thrown if the <see cref="Entity.Id"/> has been registered under a different <see cref="Entity"/></exception>
			/// <remarks>This method is meant to be called from an entity's destructor.</remarks>
			internal void DeregisterEntity (Entity e)
			{
				var id = e.Id;
				lock(_registeredEntities)
				{
					Entity existing;
					if(_registeredEntities.TryGetValue(id, out existing))
					{ // ID has been reserved
						if(ReferenceEquals(e, existing)) // This ID is reserved for e
							_registeredEntities.Remove(id);
						else // This ID is reserved for another entity
							throw new InvalidOperationException("The ID has been reserved for another entity.");
					}
				}
			}

			/// <summary>
			/// Retrieves an entity with a specified ID
			/// </summary>
			/// <param name="id">The entity's unique identifier</param>
			/// <returns>The entity if it was found or null if no entity exists with the provided <paramref name="id"/></returns>
			public Entity this [ulong id]
			{
				get
				{
					lock(_registeredEntities)
					{
						Entity e;
						if(_registeredEntities.TryGetValue(id, out e))
							return e;
					}
					return null;
				}
			}

			/// <summary>
			/// Total number of registered entities
			/// </summary>
			public int EntityCount
			{
				get
				{
					lock(_registeredEntities)
						return _registeredEntities.Count;
				}
			}

			/// <summary>
			/// Finds all entities that have a given component
			/// </summary>
			/// <param name="componentType">Type of component to look for in each entity</param>
			/// <returns>A collection of entities that have a component</returns>
			public IEnumerable<Entity> GetEntitiesWithComponent (Type componentType)
			{
				throw new NotImplementedException();
			}
		}
	}
}
