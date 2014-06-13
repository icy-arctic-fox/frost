using Frost;
using Frost.Entities;

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
			var factory = new EntityFactory(Entities);
			factory.AddComponent(new Positional2DComponent());
//			factory.AddComponent(new StaticImageComponent());
			factory.Construct();
		}

		public override bool AllowFallthrough
		{
			get { return false; }
		}
	}
}
