using System;
using System.Collections;
using System.Collections.Generic;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Tracks the state of text as it gets drawn while enumerating through live text segments
	/// </summary>
	public class LiveTextEnumerator : IEnumerator<LiveTextSegment>
	{
		/// <summary>
		/// Advances the enumerator to the next segment
		/// </summary>
		/// <returns>True if the enumerator was successfully advanced to the next segment;
		/// false if the enumerator has passed the end of the text</returns>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the text was modified after the enumerator was created</exception>
		public bool MoveNext ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Sets the enumerator to its initial position, which is before the first segment in the live text
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the text was modified after the enumerator was created</exception>
		public void Reset ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the segment in the text at the current position of the enumerator
		/// </summary>
		/// <returns>The segment in the text at the current position of the enumerator</returns>
		public LiveTextSegment Current
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the current element in the collection
		/// </summary>
		/// <returns>The current element in the collection</returns>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the enumerator is positioned before the first element of the collection or after the last element</exception>
		object IEnumerator.Current
		{
			get { return Current; }
		}

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the enumerator has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Frees resources held by the enumerator
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Deconstructs the enumerator
		/// </summary>
		~LiveTextEnumerator ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the resources held by the enumerator
		/// </summary>
		/// <param name="disposing">True to dispose of the inner resources (<see cref="Dispose"/> was called)</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				// TODO
			}
		}
		#endregion
	}
}
