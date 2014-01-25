using System;

namespace Frost.Modules
{
	/// <summary>
	/// Retrieves input from the user and presents it in a friendly format for games
	/// </summary>
	public class InputModule : IModule
	{
		/// <summary>
		/// Starts up the module and prepares it for usage
		/// </summary>
		public void Initialize ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Polls the input devices for changes.
		/// This must be called for every logic update.
		/// </summary>
		public void Update ()
		{
			throw new NotImplementedException();
		}

		#region Disposable
		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the module has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Disposes of the module by releasing resources held by it
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Destructor - disposes of the module
		/// </summary>
		~InputModule ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the module
		/// </summary>
		/// <param name="disposing">True if inner-resources should be disposed of (<see cref="Dispose"/> was called)</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				if(disposing)
				{
					// ...
				}
			}
		}
		#endregion
	}
}
