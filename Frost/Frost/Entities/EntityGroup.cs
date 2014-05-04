using System;
using System.Collections;
using System.Collections.Generic;

namespace Frost.Entities
{
	/// <summary>
	/// Collection of entities.
	/// Disposed entities are automatically removed from the collection.
	/// </summary>
	public class EntityGroup : ICollection<Entity>
	{
		private readonly List<Entity> _entities = new List<Entity>();

		/// <summary>
		/// Creates an empty group of entities
		/// </summary>
		public EntityGroup ()
		{
			// ...
		}

		/// <summary>
		/// Creates a pre-populated group of entities
		/// </summary>
		/// <param name="entities">Initial entities</param>
		/// <exception cref="ArgumentNullException">The collection of <paramref name="entities"/> can't be null.</exception>
		public EntityGroup (IEnumerable<Entity> entities)
		{
			if(entities == null)
				throw new ArgumentNullException("entities");

			foreach(var entity in entities)
				if(entity != null)
					_entities.Add(entity);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the group
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the group of entities</returns>
		public IEnumerator<Entity> GetEnumerator ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the group
		/// </summary>
		/// <returns>An enumerator object that can be used to iterate through the group of entities</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Iterates over all of the entities and calls a method for each one
		/// </summary>
		/// <param name="method">Method to call for each entity</param>
		/// <exception cref="ArgumentNullException">The <paramref name="method"/> to call for each entity can't be null.</exception>
		public void Iterate<T> (Action<Entity, T> method)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds an entity to the group
		/// </summary>
		/// <param name="item">The entity to add to the group</param>
		/// <exception cref="NotSupportedException">The collection is read-only.</exception>
		public void Add (Entity item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes all entities from the group
		/// </summary>
		/// <exception cref="NotSupportedException">The collection is read-only.</exception>
		public void Clear ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether the group contains a specific entity
		/// </summary>
		/// <returns>True if <paramref name="item"/> is found in the group; otherwise, false</returns>
		/// <param name="item">The entity to locate in the group</param>
		public bool Contains (Entity item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Copies the elements of the group to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from group -
		/// the <see cref="Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins</param>
		/// <exception cref="ArgumentNullException">The <paramref name="array"/> to copy to can't be null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
		/// <exception cref="ArgumentException">The number of elements in the source group is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
		public void CopyTo (Entity[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes the first occurrence of a specific entity from the group
		/// </summary>
		/// <returns>True if <paramref name="item"/> was successfully removed from the group; otherwise, false.
		/// This method also returns false if <paramref name="item"/> is not found in the original group.</returns>
		/// <param name="item">The entity to remove from the group</param>
		/// <exception cref="NotSupportedException">The collection is read-only.</exception>
		public bool Remove (Entity item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Number of entities contained in the group
		/// </summary>
		public int Count
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Indicates whether the collection is read-only
		/// </summary>
		public bool IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}
	}
}
