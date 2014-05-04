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
		private readonly List<Entity> _entities;
		private readonly bool _ro;

		/// <summary>
		/// Creates an empty group of entities
		/// </summary>
		public EntityGroup ()
		{
			_entities = new List<Entity>();
		}

		/// <summary>
		/// Creates a pre-populated group of entities
		/// </summary>
		/// <param name="entities">Initial entities</param>
		/// <exception cref="ArgumentNullException">The collection of <paramref name="entities"/> can't be null.</exception>
		/// <exception cref="ArgumentException">An entity in <paramref name="entities"/> is null or previously disposed.</exception>
		public EntityGroup (IEnumerable<Entity> entities)
		{
			if(entities == null)
				throw new ArgumentNullException("entities");

			_entities = new List<Entity>();
			foreach(var entity in entities)
			{
				if(entity == null || entity.Disposed)
					throw new ArgumentException();
				addEntity(entity);
			}
		}

		/// <summary>
		/// Private constructor for creating a read-only entity group
		/// </summary>
		/// <param name="entities">Original list of entities</param>
		/// <param name="ro">Flag indicating whether the group is read-only</param>
		private EntityGroup (List<Entity> entities, bool ro)
		{
			_entities = entities;
			_ro       = ro;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the group
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the group of entities</returns>
		public IEnumerator<Entity> GetEnumerator ()
		{
			return _entities.GetEnumerator();
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
		/// <exception cref="ArgumentNullException">The <paramref name="item"/> to add can't be null.</exception>
		/// <exception cref="ArgumentException">The <paramref name="item"/> to add can't be disposed.</exception>
		public void Add (Entity item)
		{
			if(_ro)
				throw new NotSupportedException("The collection is read-only.");
			if(item == null)
				throw new ArgumentNullException("item");
			if(item.Disposed)
				throw new ArgumentException("item");

			lock(_entities)
				addEntity(item);
		}

		/// <summary>
		/// Actually adds an entity to the list (sans the locking)
		/// </summary>
		/// <param name="entity">Entity to add</param>
		private void addEntity (Entity entity)
		{
			_entities.Add(entity);
			entity.Disposing += entityDisposing;
		}

		/// <summary>
		/// Called when an entity is being disposed.
		/// Removes the disposing entity from the group.
		/// </summary>
		/// <param name="sender">Entity being disposed</param>
		/// <param name="e">Event arguments (unused)</param>
		private void entityDisposing (object sender, EventArgs e)
		{
			Remove((Entity)sender);
		}

		/// <summary>
		/// Removes all entities from the group
		/// </summary>
		/// <exception cref="NotSupportedException">The collection is read-only.</exception>
		public void Clear ()
		{
			if(_ro)
				throw new NotSupportedException("The collection is read-only.");

			lock(_entities)
				_entities.Clear();
		}

		/// <summary>
		/// Determines whether the group contains a specific entity
		/// </summary>
		/// <returns>True if <paramref name="item"/> is found in the group; otherwise, false</returns>
		/// <param name="item">The entity to locate in the group</param>
		/// <exception cref="ArgumentNullException">The <paramref name="item"/> to look for can't be null.</exception>
		public bool Contains (Entity item)
		{
			if(item == null)
				throw new ArgumentNullException("item");

			lock(_entities)
				return _entities.Contains(item);
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
			if(array == null)
				throw new ArgumentNullException("array");
			if(arrayIndex < 0)
				throw new ArgumentOutOfRangeException("arrayIndex");

			lock(_entities)
			{
				if(_entities.Count > array.Length - arrayIndex)
					throw new ArgumentException();
				_entities.CopyTo(array, arrayIndex);
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific entity from the group
		/// </summary>
		/// <returns>True if <paramref name="item"/> was successfully removed from the group; otherwise, false.
		/// This method also returns false if <paramref name="item"/> is not found in the original group.</returns>
		/// <param name="item">The entity to remove from the group</param>
		/// <exception cref="ArgumentNullException">The <paramref name="item"/> to remove can't be null.</exception>
		/// <exception cref="NotSupportedException">The collection is read-only.</exception>
		public bool Remove (Entity item)
		{
			if(_ro)
				throw new NotSupportedException("The collection is read-only.");
			if(item == null)
				throw new ArgumentNullException("item");

			lock(_entities)
				return _entities.Remove(item);
		}

		/// <summary>
		/// Number of entities contained in the group
		/// </summary>
		public int Count
		{
			get
			{
				lock(_entities)
					return _entities.Count;
			}
		}

		/// <summary>
		/// Indicates whether the collection is read-only
		/// </summary>
		public bool IsReadOnly
		{
			get { return _ro; }
		}

		/// <summary>
		/// Creates another entity group that will maintain the same collection of entities,
		/// but will not allow modifications to the collection
		/// </summary>
		/// <returns>A read-only entity group</returns>
		public EntityGroup AsReadOnly ()
		{
			return new EntityGroup(_entities, true);
		}
	}
}
