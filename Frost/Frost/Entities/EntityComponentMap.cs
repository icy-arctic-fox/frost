using System;

namespace Frost.Entities
{
	/// <summary>
	/// Handles retrieving component information from an entity
	/// </summary>
	/// <typeparam name="T">Type of component information that the instance will retrieve</typeparam>
	public class EntityComponentMap<T> where T : IEntityComponent
	{
		private readonly int _index;

		/// <summary>
		/// Creates a component mapping
		/// </summary>
		/// <param name="index">Index of the component in the entity's component array</param>
		internal EntityComponentMap (int index)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves component information from an entity
		/// </summary>
		/// <param name="entity">Entity to retrieve component from</param>
		/// <returns>Component information</returns>
		public T GetComponentFromEntity (Entity entity)
		{
			throw new NotImplementedException();
		}
	}
}
