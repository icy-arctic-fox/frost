using System.IO;
using Frost.IO.Resources;
using Frost.Modules;

namespace Frost.Graphics
{
	public static class ResourceLoader
	{
		private static Texture textureLoader (ResourcePackageEntry info, Stream s)
		{
			return new Texture(s);
		}

		public static Texture LoadTexture (this ResourceManager manager, string name, bool allowMod = true)
		{
			return manager.GetResource(name, textureLoader, allowMod);
		}
	}
}
