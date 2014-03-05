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
		private readonly object _locker = new object();

		#region IDs

		/// <summary>
		/// Id of entities that have not been registered
		/// </summary>
		private const ulong UnregisteredId = 0uL;

		private ulong _id = UnregisteredId;

		/// <summary>
		/// Unique entity identifier
		/// </summary>
		public ulong Id
		{
			get
			{
				lock(_locker)
					return _id;
			}
		}

		/// <summary>
		/// Assigns an ID to the entity.
		/// This is part of the registration process.
		/// </summary>
		/// <param name="id">ID to give the entity</param>
		private void assignId (ulong id)
		{
			if(id == UnregisteredId)
				throw new ArgumentException("The entity ID can't be set to " + UnregisteredId);
			lock(_locker)
			{
				if(Registered)
					throw new InvalidOperationException("The entity has already been registered.");
				_id = id;
			}
		}

		/// <summary>
		/// Unassigns an ID from the entity.
		/// This is part of the deregistration process.
		/// </summary>
		private void unassignId ()
		{
			lock(_locker)
				_id = UnregisteredId;
		}

		/// <summary>
		/// Indicates whether the entity has been registered
		/// </summary>
		public bool Registered
		{
			get { return Id != UnregisteredId; }
		}
		#endregion

		private readonly List<Tuple<Type, EntityComponent>> _components = new List<Tuple<Type, EntityComponent>>();

		/// <summary>
		/// Adds a component to the entity.
		/// A component is a piece of information required for some functionality.
		/// </summary>
		/// <param name="component">Entity component to add</param>
		/// <exception cref="ObjectDisposedException">The <see cref="Entity"/> has been disposed.</exception>
		/// <exception cref="InvalidOperationException">Components can't be added after the entity has been registered.</exception>
		public void AddComponent (EntityComponent component)
		{
			if(_disposed)
				throw new ObjectDisposedException(GetType().FullName);

			lock(_locker)
			{
				if(Registered)
					throw new InvalidOperationException("Components can not be added to an entity after it has been registered.");
			}
			throw new NotImplementedException();
		}

		/// <summary>
		/// Checks if the entity contains a component
		/// </summary>
		/// <param name="componentType">Type of <see cref="EntityComponent"/> to look for</param>
		/// <returns>True if the entity has the component, false otherwise</returns>
		/// <exception cref="ObjectDisposedException">A component can't be retrieved after the entity has been disposed.</exception>
		public bool HasComponent (Type componentType)
		{
			if(_disposed)
				throw new ObjectDisposedException(GetType().FullName);

			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the data associated with a component contained in the entity
		/// </summary>
		/// <param name="componentType">Type of <see cref="EntityComponent"/> to look for</param>
		/// <returns>The component data or null if the entity doesn't contain the component specified by <paramref name="componentType"/></returns>
		/// <exception cref="ObjectDisposedException">A component can't be retrieved after the entity has been disposed.</exception>
		public EntityComponent GetComponent (Type componentType)
		{
			if(_disposed)
				throw new ObjectDisposedException(GetType().FullName);

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
