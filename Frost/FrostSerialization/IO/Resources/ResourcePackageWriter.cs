using System;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Creates resource package files
	/// </summary>
	public class ResourcePackageWriter : IDisposable
	{
		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the resource package writer has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Disposes of the resource package writer by flushing output, closing the file, and freeing resources
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Destructor - disposes of the resource package writer
		/// </summary>
		~ResourcePackageWriter ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the resource package writer
		/// </summary>
		/// <param name="disposing">True if inner-resources should be disposed of (<see cref="Dispose"/> was called)</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
/*				if(disposing)
					_bw.Dispose(); */
				_disposed = true;
			}
		}
		#endregion
	}
}
