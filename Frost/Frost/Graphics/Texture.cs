using System.IO;

namespace Frost.Graphics
{
	/// <summary>
	/// An image that lives on the graphics card
	/// </summary>
	public class Texture
	{
		private readonly SFML.Graphics.Texture _texture;

		internal SFML.Graphics.Texture InternalTexture
		{
			get { return _texture; }
		}

		public Texture (string filename)
		{
			_texture = new SFML.Graphics.Texture(filename);
		}

		public Texture (Stream s)
		{
			_texture = new SFML.Graphics.Texture(s);
		}
	}
}
