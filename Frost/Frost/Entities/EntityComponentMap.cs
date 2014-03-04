using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frost.Entities
{
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
		public T GetComponent (Entity e)
		{
			lock(_components)
			{
				T component;
				if(_components.TryGetValue(e, out component))
					return component; // Component has been accessed before and is cached
				// Component isn't cached, retrieve it from the entity
				component = e.GetComponent(_componentType) as T;
				if(component != null)
					_components[e] = component;
				// TODO: Possibly throw exception if the entity doesn't contain the component
				return component;
			}
		}
	}
}
