using System;
using Frost.Utility;

namespace Frost.Entities
{
	/// <summary>
	/// Base game object that consists of a unique ID and component information
	/// </summary>
	public class Entity : IFullDisposable
	{
		#region Registration

		private Guid _id;

		/// <summary>
		/// Unique identification number of the entity.
		/// This value can be used to reference an entity from anywhere.
		/// An entity that has an empty ID (<see cref="Guid.Empty"/>) has not been registered and should not be openly used.
		/// </summary>
		public Guid Id
		{
			get { return _id; }
		}

		private int _index;

		/// <summary>
		/// Internal entity index
		/// </summary>
		internal int Index
		{
			get { return _index; }
		}

		private EntityManager _owner;

		/// <summary>
		/// Manager that owns and controls the entity
		/// </summary>
		/// <remarks>This property's value will be null if the entity isn't registered.</remarks>
		public EntityManager Owner
		{
			get { return _owner; }
		}

		/// <summary>
		/// Indicates whether the entity has been registered.
		/// An unregistered entity will have an empty <see cref="Id"/> and a null <see cref="Owner"/>.
		/// </summary>
		public bool Registered
		{
			get { return _owner != null; }
		}

		/// <summary>
		/// Sets the unique identification number of the entity
		/// </summary>
		/// <param name="owner">Manager that owns and controls the entity</param>
		/// <param name="id">Unique ID of the entity</param>
		/// <param name="index">Internal entity index</param>
		internal void SetRegistrationInfo (EntityManager owner, Guid id, int index)
		{
			_owner = owner;
			_id    = id;
			_index = index;
		}

		/// <summary>
		/// Clears all registration information - effectively marks the entity as unregistered
		/// </summary>
		internal void ClearRegistrationInfo ()
		{
			_owner = null;
			_id    = Guid.Empty;
			_index = -1;
		}
		#endregion

		#region Components

		/// <summary>
		/// Checks whether the entity has a component
		/// </summary>
		/// <typeparam name="T">Type of component</typeparam>
		/// <returns>True if the entity has the component, false otherwise</returns>
		/// <exception cref="InvalidOperationException">The entity must be registered before accessing entity components.</exception>
		public bool HasComponent<T> () where T : IComponent
		{
			if(_owner == null)
				throw new InvalidOperationException("The entity must be registered before accessing entity components.");

			return _owner.HasComponent<T>(this);
		}

		/// <summary>
		/// Adds a component to the entity
		/// </summary>
		/// <param name="component">Component to add to the entity</param>
		/// <exception cref="InvalidOperationException">The entity must be registered before accessing entity components.</exception>
		/// <exception cref="ArgumentNullException">The <paramref name="component"/> can't be null.</exception>
		public void AddComponent (IComponent component)
		{
			if(_owner == null)
				throw new InvalidOperationException("The entity must be registered before accessing entity components.");

			_owner.AddComponent(this, component);
		}
		#endregion

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the entity has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Releases resources held by the entity
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Deconstructs the entity
		/// </summary>
		~Entity ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Releases resources held by the entity
		/// </summary>
		/// <param name="disposing">Indicates whether inner-resources should be freed</param>
		protected virtual void Dispose (bool disposing)
		{
			if(_disposed)
				return; // Already disposed

			Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);
			_disposed = true;
		}

		/// <summary>
		/// Triggered just before the entity is disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;
		#endregion
	}
}
