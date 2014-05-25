using System;
using System.Collections.Generic;
using Frost.Utility;

namespace Frost.Entities
{
	/// <summary>
	/// Base game object that consists of a unique ID and component information
	/// </summary>
	public class Entity : IFullDisposable
	{
		#region ID

		private ulong _id;

		/// <summary>
		/// Unique identification number of the entity.
		/// This value can be used to reference an entity from anywhere.
		/// An entity that has an ID of 0 has not been registered and should not be openly used.
		/// </summary>
		public ulong Id
		{
			get { return _id; }
		}

		/// <summary>
		/// Indicates whether the entity has been registered.
		/// An unregistered entity will have an <see cref="Id"/> of 0.
		/// </summary>
		public bool Registered
		{
			get { return _id != 0L; }
		}

		/// <summary>
		/// Sets the unique identification number of the entity
		/// </summary>
		/// <param name="id">Entity ID number</param>
		internal void SetId (ulong id)
		{
			_id = id;
		}
		#endregion

		#region Components

		private readonly List<IEntityComponent> _components = new List<IEntityComponent>(0);

		/// <summary>
		/// Adds component functionality to the entity
		/// </summary>
		/// <param name="index">Index in the component array to store the component at</param>
		/// <param name="component">Component information</param>
		/// <exception cref="InvalidOperationException">Components can't be added to the entity after it has been registered.</exception>
		internal void AddComponent (int index, IEntityComponent component)
		{
			if(Registered)
				throw new InvalidOperationException("Components can't be added to the entity after it has been registered.");

			// Pad with null components until the index is reached
			while(_components.Count < index + 1)
				_components.Add(null);
			_components[index] = component;
		}

		/// <summary>
		/// Retrieves component information from the entity
		/// </summary>
		/// <param name="index">Index in the component array that the component information is stored at</param>
		/// <returns>Component information</returns>
		internal IEntityComponent GetComponent (int index)
		{
			return _components[index];
		}

		/// <summary>
		/// Checks if the entity has a component
		/// </summary>
		/// <param name="componentType">Type of component to look for</param>
		/// <returns>True if the entity has the component or false if it doesn't</returns>
		/// <exception cref="ArgumentNullException">The type of component to look for can't be null.</exception>
		internal bool HasComponent (Type componentType)
		{
			if(componentType == null)
				throw new ArgumentNullException("componentType");

			for(var i = 0; i < _components.Count; ++i)
			{
				var component = _components[i];
				if(component != null && Portability.CompareTypes(componentType, component.GetType()))
					return true;
			}
			return false;
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
