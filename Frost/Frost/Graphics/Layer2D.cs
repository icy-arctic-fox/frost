using System;
using System.Collections;
using System.Collections.Generic;
using Frost.Display;

namespace Frost.Graphics
{
	/// <summary>
	/// A layer of graphics that contains 2D objects.
	/// Objects are rendered in the order that they are added.
	/// Objects added last will render on top of other objects.
	/// </summary>
	public class Layer2D : IDisplayContainer<Object2D>
	{
		private readonly List<Object2D> _objects = new List<Object2D>();

		/// <summary>
		/// Draws the state of a layer and all contained objects
		/// </summary>
		/// <param name="display">Display to draw the state onto</param>
		/// <param name="state">Index of the state to draw</param>
		/// <remarks>None of the game states should be modified by this process - including the state indicated by <paramref name="state"/>.
		/// Modifying the game state info during this process would corrupt the game state.</remarks>
		public void Draw (IDisplay display, int state)
		{
			foreach(var obj in _objects)
				obj.Draw(display, state);
		}

		/// <summary>
		/// Returns an enumerator that iterates through each <see cref="Object2D"/>
		/// </summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the objects</returns>
		public IEnumerator<Object2D> GetEnumerator ()
		{
			return _objects.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through each <see cref="Object2D"/>
		/// </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the objects</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Adds an <see cref="Object2D"/> to the layer.
		/// The <see cref="Object2D"/> will be placed on top of all existing objects.
		/// </summary>
		/// <param name="item">The object to add to the layer</param>
		public void Add (Object2D item)
		{
#if DEBUG
			if(item == null)
				throw new ArgumentNullException("item", "The object can't be null.");
#endif
			_objects.Add(item);
		}

		/// <summary>
		/// Removes all objects from the layer
		/// </summary>
		public void Clear ()
		{
			_objects.Clear();
		}

		/// <summary>
		/// Determines whether the layer contains a specific <see cref="Object2D"/>
		/// </summary>
		/// <returns>True if <paramref name="item"/> is found in the layer; otherwise, false</returns>
		/// <param name="item">The object to locate in the layer</param>
		public bool Contains (Object2D item)
		{
			return _objects.Contains(item);
		}

		/// <summary>
		/// Copies the contents of the layer to an <see cref="T:System.Array"/>, starting at a specified <see cref="T:System.Array"/> index
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the <see cref="Object2D"/> copied from the layer.
		/// The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins</param>
		/// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="array"/> is null</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Thrown if <paramref name="arrayIndex"/> is less than 0</exception>
		/// <exception cref="T:System.ArgumentException">Thrown if <paramref name="array"/> is multidimensional or the number of objects in the layer is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/></exception>
		public void CopyTo (Object2D[] array, int arrayIndex)
		{
			_objects.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the layer
		/// </summary>
		/// <returns>True if <paramref name="item"/> was successfully removed from the layer; otherwise, false.
		/// This method also returns false if <paramref name="item"/> is not found in the layer.</returns>
		/// <param name="item">The object to remove from the layer</param>
		public bool Remove (Object2D item)
		{
			return _objects.Remove(item);
		}

		/// <summary>
		/// Number of objects contained in the layer
		/// </summary>
		public int Count
		{
			get { return _objects.Count; }
		}

		/// <summary>
		/// Indicates whether the layer is read-only
		/// </summary>
		/// <remarks>This property's value is always false.</remarks>
		public bool IsReadOnly
		{
			get { return false; }
		}
	}
}
