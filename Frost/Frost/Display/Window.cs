using System;

namespace Frost.Display
{
	/// <summary>
	/// Interface for displaying graphics to the user
	/// </summary>
	public class Window : IDisposable
	{
		#region Disposable

		private bool _disposed;

		/// <summary>
		/// Indicates whether or not the window has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Closes the window and disposes of the resources held by it
		/// </summary>
		/// <remarks>If the window has already been disposed, then this method will do nothing.</remarks>
		/// <seealso cref="Disposed"/>
		public void Dispose ()
		{
			if(!_disposed)
			{// Window hasn't been disposed of yet
				_disposed = true;
				throw new NotImplementedException();
			}
		}
		#endregion
	}
}
