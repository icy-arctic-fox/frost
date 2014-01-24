using System;

namespace Frost
{
	/// <summary>
	/// Runs the game loop and controls the flow of the game between states and scenes
	/// </summary>
	public class GameRunner : IDisposable
	{
		#region Disposable

		/// <summary>
		/// Flag that indicates whether the game runner has been disposed
		/// </summary>
		public bool Disposed { get; private set; }

		/// <summary>
		/// Frees the resources held by the runner
		/// </summary>
		/// <remarks>Disposing of the game runner will stop it if it is still running.</remarks>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Tears down the game runner
		/// </summary>
		~GameRunner ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Underlying method that releases the resources held by the runner
		/// </summary>
		/// <param name="disposing">True if <see cref="Dispose"/> was called and inner resources should be disposed as well</param>
		protected void Dispose (bool disposing)
		{
			if(!Disposed)
			{// Don't do anything if the runner is already disposed
				Disposed = true;
				if(disposing)
				{// Dispose of the resources this object holds
					// TODO
				}
			}
		}
		#endregion
	}
}
