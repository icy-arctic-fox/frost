using System;
using System.Collections.Generic;

namespace Frost.Entities
{
	/// <summary>
	/// Base class for all entities.
	/// An entity is a fundamental object for a game.
	/// Entities can have any number of components (<see cref="EntityComponent"/>)
	/// which define characteristics of that entity.
	/// </summary>
	/// <seealso cref="Manager"/>
	public partial class Entity
	{
		/// <summary>
		/// Id of entities that have not been registered
		/// </summary>
		private const ulong UnregisteredId = 0uL;

		/// <summary>
		/// Unique entity identifier
		/// </summary>
		public ulong Id { get; private set; }

		/// <summary>
		/// Indicates whether the entity has been registered
		/// </summary>
		public bool Registered
		{
			get { return Id != UnregisteredId; }
		}

		private readonly List<EntityComponent> _components = new List<EntityComponent>();

		public void AddComponent (EntityComponent component)
		{
			throw new NotImplementedException();
		}

		public EntityComponent GetComponent (Type componentType)
		{
			throw new NotImplementedException();
		}
	}
}
