using System;
using System.Collections.Generic;
using Frost.Utility;

namespace Frost.Entities
{
	/// <summary>
	/// Base class for all entities.
	/// An entity is a fundamental object for a game.
	/// Entities can have any number of components (<see cref="EntityComponent"/>)
	/// which define characteristics of that entity.
	/// </summary>
	/// <seealso cref="Manager"/>
	public partial class Entity : IFullDisposable
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

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the entity has been freed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Triggered just before the entity is freed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Frees the resources held by the entity
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Deconstructs the entity
		/// </summary>
		~Entity ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the entity
		/// </summary>
		/// <param name="disposing">Flag indicating whether internal resources should be freed</param>
		/// <remarks>This method calls the <see cref="Disposing"/> event.</remarks>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				Disposing.NotifySubscribers(this, EventArgs.Empty);

				if(disposing)
				{// Dispose of the internal texture
					// TODO
				}
			}
		}
		#endregion
	}
}
