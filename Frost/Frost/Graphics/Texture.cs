using System;
using System.IO;
using Frost.Utility;
using SfmlTexture = SFML.Graphics.Texture;

namespace Frost.Graphics
{
	/// <summary>
	/// An image that lives on the graphics card
	/// </summary>
	public class Texture : IFullDisposable
	{
		private SfmlTexture _texture;

		/// <summary>
		/// Underlying SFML texture
		/// </summary>
		/// <exception cref="ArgumentNullException">The internal texture can't be set to null.</exception>
		internal SfmlTexture SfmlTexture
		{
			get { return _texture; }
			set
			{
#if DEBUG
				if(value == null)
					throw new ArgumentNullException("value");
#endif
				_texture = value;
			}
		}

		/// <summary>
		/// Creates a texture from an existing SFML texture
		/// </summary>
		/// <param name="texture">SFML texture</param>
		/// <exception cref="ArgumentNullException">The internal <paramref name="texture"/> can't be null.</exception>
		internal Texture (SfmlTexture texture)
		{
			if(texture == null)
				throw new ArgumentNullException("texture");
			_texture = texture;
		}

		/// <summary>
		/// Creates a new texture from an image file
		/// </summary>
		/// <param name="filename">Path to the image file to load</param>
		public Texture (string filename)
		{
			_texture = new SfmlTexture(filename);
		}

		/// <summary>
		/// Creates a new texture from an image stream
		/// </summary>
		/// <param name="s">Stream containing an image to load into the texture</param>
		public Texture (Stream s)
		{
			_texture = new SfmlTexture(s);
		}

		/// <summary>
		/// Creates a new blank texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		public Texture (uint width, uint height)
		{
			_texture = new SfmlTexture(width, height);
		}

		/// <summary>
		/// Clones the existing texture to a new instance.
		/// This allows the two textures to be used independently of each other.
		/// </summary>
		/// <returns>A copy of the texture</returns>
		public Texture Clone ()
		{
			var image = _texture.CopyToImage();
			var tex   = new SfmlTexture(image);
			return new Texture(tex);
		}

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the texture has been freed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Triggered when the texture is being freed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Frees the resources held by the texture
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Deconstructs the texture
		/// </summary>
		~Texture ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the texture
		/// </summary>
		/// <param name="disposing">Flag indicating whether internal resources should be freed</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);

				if(disposing) // Dispose of the internal texture
					_texture.Dispose();
			}
		}
		#endregion
	}
}
