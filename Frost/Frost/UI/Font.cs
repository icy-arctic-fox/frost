using System;
using System.IO;
using Frost.Utility;
using F = SFML.Graphics.Font;

namespace Frost.UI
{
	/// <summary>
	/// Describes how text looks
	/// </summary>
	public sealed class Font : IFullDisposable
	{
		private readonly F _font;

		/// <summary>
		/// Reference to the underlying SFML font
		/// </summary>
		internal F UnderlyingFont
		{
			get { return _font; }
		}

		/// <summary>
		/// Creates a new font
		/// </summary>
		/// <param name="font">Underlying SFML font object</param>
		private Font (F font)
		{
			_font = font;
		}

		/// <summary>
		/// Loads a font from a file
		/// </summary>
		/// <param name="path">Path to the font file</param>
		/// <returns>A <see cref="Font"/> object</returns>
		public static Font LoadFromFile (string path)
		{
			var font = new F(path);
			return new Font(font);
		}

		/// <summary>
		/// Loads a font from a stream
		/// </summary>
		/// <param name="s">Stream containing the font data</param>
		/// <returns>A <see cref="Font"/> object</returns>
		public static Font LoadFromStream (Stream s)
		{
			var font = new F(s);
			return new Font(font);
		}

		#region Disposable
		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the font has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Triggered when the font is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Disposes of the font and the resources it holds
		/// </summary>
		public void Dispose ()
		{
			dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Deconstructs the font
		/// </summary>
		~Font ()
		{
			dispose(false);
		}

		/// <summary>
		/// Disposes of the resources held by the font
		/// </summary>
		/// <param name="disposing">True if internal resources should also be disposed (<see cref="Dispose"/> was called)</param>
		private void dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				Disposing.NotifySubscribers(this, EventArgs.Empty);
				if(disposing)
					_font.Dispose();
			}
		}
		#endregion
	}
}
