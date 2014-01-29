using System;
using System.IO;
using Frost.Graphics;
using Frost.IO.Resources;
using Frost.UI;

namespace Frost
{
	/// <summary>
	/// Simplifies loading various graphical resources
	/// </summary>
	public static class ResourceLoader
	{
		/// <summary>
		/// Passes the resource stream to a texture constructor
		/// </summary>
		/// <param name="info">Information about the resource</param>
		/// <param name="s">Stream containing the texture resource</param>
		/// <returns>The produced texture</returns>
		private static Texture textureLoader (ResourcePackageEntry info, Stream s)
		{
			return new Texture(s);
		}

		/// <summary>
		/// Loads a texture from resource packages
		/// </summary>
		/// <param name="manager">Manager that tracks the resource packages to load from</param>
		/// <param name="name">Name of the texture to load</param>
		/// <param name="allowMod">True to allow modded resources</param>
		/// <returns>The loaded texture</returns>
		public static Texture LoadTexture (this ResourceManager manager, string name, bool allowMod = true)
		{
			return manager.GetResource(name, textureLoader, allowMod);
		}

		/// <summary>
		/// Passes the resource stream to a font builder
		/// </summary>
		/// <param name="info">Information about the resource</param>
		/// <param name="s">Stream containing the font resource</param>
		/// <returns>The produced font</returns>
		private static Font fontLoader (ResourcePackageEntry info, Stream s)
		{
			return Font.LoadFromStream(s);
		}

		/// <summary>
		/// Loads a font from resource packages
		/// </summary>
		/// <param name="manager">Manager that tracks the resource packages to load from</param>
		/// <param name="name">Name of the font to load</param>
		/// <param name="allowMod">True to allow modded resources</param>
		/// <returns>The loaded font</returns>
		public static Font LoadFont (this ResourceManager manager, string name, bool allowMod = true)
		{
			return manager.GetResource(name, fontLoader, allowMod);
		}
	}
}
