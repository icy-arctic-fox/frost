using System;
using System.IO;
using System.Reflection;
using Frost.Utility;
using F = SFML.Graphics.Font;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Describes how text looks
	/// </summary>
	public sealed class Font : IFullDisposable
	{
		private const string DefaultFontResourceName = "Frost.Resources.Sansation_Regular.ttf";
		private const string DebugFontResourceName   = "Frost.Resources.crystal.ttf";

		private static readonly object _defaultLocker = new object();
		private static Font _defaultFont, _debugFont;

		/// <summary>
		/// Retrieves the default font that is embedded in Frost
		/// </summary>
		/// <returns>Default font information</returns>
		/// <exception cref="BadImageFormatException">The font wasn't found embedded in the dll.</exception>
		public static Font GetDefaultFont()
		{
			lock(_defaultLocker)
			{
				if(_defaultFont == null)
				{// Default font hasn't be loaded yet
					var fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DefaultFontResourceName);
					if(fontStream == null)
						throw new BadImageFormatException("Failed to load the embedded default font", DefaultFontResourceName);
					_defaultFont = LoadFromStream(fontStream);
				}
				return _defaultFont;
			}
		}

		/// <summary>
		/// Retrieves the debug font that is embedded in Frost
		/// </summary>
		/// <returns>Debug font information</returns>
		/// <remarks>The debug font is a bitmap-based.</remarks>
		/// <exception cref="BadImageFormatException">The font wasn't found embedded in the dll.</exception>
		public static Font GetDebugFont ()
		{
			lock(_defaultLocker)
			{
				if(_debugFont == null)
				{// Debug font hasn't be loaded yet
					var fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DebugFontResourceName);
					if(fontStream == null)
						throw new BadImageFormatException("Failed to load the embedded debug font", DebugFontResourceName);
					_debugFont = LoadFromStream(fontStream);
				}
				return _debugFont;
			}
		}

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
		/// <exception cref="ArgumentNullException">The path to the file containing the font information can't be null.</exception>
		public static Font LoadFromFile (string path)
		{
			if(path == null)
				throw new ArgumentNullException("path");

			var font = new SFML.Graphics.Font(path);
			return new Font(font);
		}

		/// <summary>
		/// Loads a font from a stream
		/// </summary>
		/// <param name="s">Stream containing the font data</param>
		/// <returns>A <see cref="Font"/> object</returns>
		/// <exception cref="ArgumentNullException">The stream to read font data from can't be null.</exception>
		public static Font LoadFromStream (Stream s)
		{
			if(s == null)
				throw new ArgumentNullException("s");

			var font = new SFML.Graphics.Font(s);
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
		/// <exception cref="InvalidOperationException">The embedded fonts can't be disposed of manually.</exception>
		/// <seealso cref="GetDefaultFont"/>
		public void Dispose ()
		{
			lock(_defaultLocker)
				if(ReferenceEquals(this, _defaultFont) || ReferenceEquals(this, _debugFont))
					throw new InvalidOperationException("The embedded fonts can't be disposed of manually.");

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
