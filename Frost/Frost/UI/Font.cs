using System;
using F = SFML.Graphics.Font;

namespace Frost.UI
{
	public class Font : IDisposable
	{
		private readonly F _font;

		#region Disposabled
		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the font has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Disposes of the font and the resources it holds
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Deconstructs the font
		/// </summary>
		~Font ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the resources held by the font
		/// </summary>
		/// <param name="disposing">True if internal resources should also be disposed (<see cref="Dispose"/> was called)</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				if(disposing)
					_font.Dispose();
			}
		}
		#endregion
	}
}
