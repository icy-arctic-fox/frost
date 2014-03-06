using Frost.Display;
using Frost.Entities;
using Frost.Graphics;
using Frost.Logic;

namespace Test_Game
{
	class BallScene : Scene
	{
		public override string Name
		{
			get { return "Ball"; }
		}

		private const int BallCount = 5000;

		private readonly Entity[] _balls;
		private readonly Graphics2DEntityProcessor _processor;

		/// <summary>
		/// Creates the base of the scene
		/// </summary>
		public BallScene ()
		{
			var texture = new Texture("ball-6x6.png");
			_processor = new Graphics2DEntityProcessor(Entities);

			_balls = new Entity[BallCount];
			for(var i = 0; i < _balls.Length; ++i)
			{
				var entity = new Entity();
				entity.AddComponent(new Position2DEntityComponent());
				entity.AddComponent(new TexturedEntityComponent(texture));
				for(var j = 0; j < StateManager.StateCount; ++j)
					((Position2DEntityComponent)entity.GetComponent(typeof(Position2DEntityComponent))).States[j].X = 10 * i;

				Entities.RegisterEntity(entity);
				_balls[i] = entity;
			}
		}

		public override void Draw (IDisplay display, int state, double t)
		{
			base.Draw(display, state, t);
			for(var i = 0; i < _balls.Length; ++i)
				_processor.DrawEntity(_balls[i], display, state, t);
		}
	}
}
