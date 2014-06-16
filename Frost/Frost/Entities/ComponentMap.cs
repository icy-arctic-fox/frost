using System;
using System.Collections.Generic;

namespace Frost.Entities
{
	/// <summary>
	/// Handles retrieving component information from an entity
	/// </summary>
	/// <typeparam name="T">Type of component information that the instance will retrieve</typeparam>
	public class ComponentMap<T> where T : IEntityComponent
	{
		private readonly IList<IEntityComponent> _componentList;

		/// <summary>
		/// Creates a component mapping
		/// </summary>
		/// <param name="componentList">Dynamic list of components, each index referencing a different entity</param>
		/// <exception cref="ArgumentNullException">The component list can't be null.</exception>
		internal ComponentMap (IList<IEntityComponent> componentList)
		{
			if(componentList == null)
				throw new ArgumentNullException("componentList");

			_componentList = componentList;
		}

		/// <summary>
		/// Retrieves component information from an entity
		/// </summary>
		/// <param name="entity">Entity to retrieve component from</param>
		/// <returns>Component information</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="entity"/> to get the component from can't be null.</exception>
		public T Get (Entity entity)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");

			var entityIndex = entity.Index;
			return (T)_componentList[entityIndex];
			// TODO: Safe access
		}
	}
}
