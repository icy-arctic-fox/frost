using System;
using System.Collections.Generic;
using Frost.Logic;

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
		private readonly ulong _id;

		/// <summary>
		/// Unique entity identifier
		/// </summary>
		public ulong Id
		{
			get { return _id; }
		}

		/// <summary>
		/// Creates a new entity
		/// </summary>
		/// <param name="id">Unique identifier returned from <see cref="Manager.NextAvailableId"/></param>
		internal Entity (ulong id)
		{
			_id = id;
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
