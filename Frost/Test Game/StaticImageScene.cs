using Frost;
using Frost.Entities;
using Frost.Graphics;

namespace Test_Game
{
	class StaticImageScene : Scene
	{
		public override string Name
		{
			get { return "Static Image"; }
		}

		public StaticImageScene ()
		{
			var texture = new Texture("cat.jpg");

			var factory = new EntityFactory(Entities);
			factory.AddComponent(new Positional2DComponent());
			factory.AddComponent(new StaticImageComponent(texture));
			factory.Construct();

			Processors.AddRenderProcessor(new StaticImageProcessor(Entities));
		}
	}
}
