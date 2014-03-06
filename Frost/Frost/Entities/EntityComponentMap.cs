using System;
using System.Collections.Generic;

namespace Frost.Entities
{
	// TODO: Speed up access to entity components by fixing their position in an array

	/// <summary>
	/// Maps component types to their collection of states (component data)
	/// </summary>
	public class EntityComponentMap<T> where T : EntityComponent
	{
		/// <summary>
		/// Type information for the component
		/// </summary>
		private readonly Type _componentType = typeof(T);

		/// <summary>
		/// Caches components already accessed in entities
		/// </summary>
		private readonly Dictionary<Entity, T> _components = new Dictionary<Entity, T>();

		/// <summary>
		/// Retrieves the component from an entity
		/// </summary>
		/// <param name="e">Entity to get the component from</param>
		/// <returns>Component data</returns>
		/// <exception cref="ArgumentNullException">The entity (<paramref name="e"/>) to get component data from can't be null.</exception>
		public T GetComponent (Entity e)
		{
			if(e == null)
				throw new ArgumentNullException("e");

			lock(_components)
			{
				T component;
				if(_components.TryGetValue(e, out component))
					return component; // Component has been accessed before and is cached
				// Component isn't cached, retrieve it from the entity
				component = e.GetComponent(_componentType) as T;
				if(component != null)
				{
					e.Disposing += entityDisposing;
					_components[e] = component;
				}
				// TODO: Possibly throw exception if the entity doesn't contain the component
				return component;
			}
		}

		/// <summary>
		/// Called when an entity that has component data cached in this instance is being disposed
		/// </summary>
		/// <param name="sender">Entity being disposed</param>
		/// <param name="e">Event arguments</param>
		private void entityDisposing (object sender, EventArgs e)
		{
			var entity = (Entity)sender;
			lock(_components)
				_components.Remove(entity);
		}
	}
}
