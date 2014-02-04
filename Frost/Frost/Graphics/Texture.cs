using System.IO;

namespace Frost.Graphics
{
	/// <summary>
	/// An image that lives on the graphics card
	/// </summary>
	public class Texture // TODO: Implement IDisposable
	{
		private readonly SFML.Graphics.Texture _texture;

		/// <summary>
		/// Underlying SFML texture
		/// </summary>
		internal SFML.Graphics.Texture InternalTexture
		{
			get { return _texture; }
		}

		/// <summary>
		/// Creates a new texture from an image file
		/// </summary>
		/// <param name="filename">Path to the image file to load</param>
		public Texture (string filename)
		{
			_texture = new SFML.Graphics.Texture(filename);
		}

		/// <summary>
		/// Creates a new texture from an image stream
		/// </summary>
		/// <param name="s">Stream containing an image to load into the texture</param>
		public Texture (Stream s)
		{
			_texture = new SFML.Graphics.Texture(s);
		}

		/// <summary>
		/// Creates a new blank texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		public Texture (uint width, uint height)
		{
			_texture = new SFML.Graphics.Texture(width, height);
		}
	}
}
