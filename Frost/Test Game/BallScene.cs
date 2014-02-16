using Frost.Display;
using Frost.Graphics;
using Frost.Logic;
using Frost.Modules.Input;

namespace Test_Game
{
	class BallScene : Scene
	{
		private const float Speed = 3f;

		private readonly Ball _ball;

		/// <summary>
		/// Creates the base of the scene
		/// </summary>
		public BallScene ()
		{
			var texture = new Texture("ball-6x6.png");
			_ball = new Ball(texture);
			SetUpdateRoot(_ball);
			SetRenderRoot(_ball);
		}

		public void SetController (BasicController controller)
		{
			controller.UpArrowPressed    += (sender, input) => { _ball.SpeedY = -Speed; };
			controller.DownArrowPressed  += (sender, input) => { _ball.SpeedY =  Speed; };
			controller.LeftArrowPressed  += (sender, input) => { _ball.SpeedX = -Speed; };
			controller.RightArrowPressed += (sender, input) => { _ball.SpeedX =  Speed; };

			controller.UpArrowReleased    += controller_VertReleased;
			controller.DownArrowReleased  += controller_VertReleased;
			controller.LeftArrowReleased  += controller_HorizReleased;
			controller.RightArrowReleased += controller_HorizReleased;
		}

		void controller_VertReleased (object sender, InputEventArgs e)
		{
			if(Keyboard.IsKeyPressed(Key.Up))
				_ball.SpeedY = -Speed;
			else if(Keyboard.IsKeyPressed(Key.Down))
				_ball.SpeedY = Speed;
			else
				_ball.SpeedY = 0f;
		}

		void controller_HorizReleased (object sender, InputEventArgs e)
		{
			if(Keyboard.IsKeyPressed(Key.Left))
				_ball.SpeedX = -Speed;
			else if(Keyboard.IsKeyPressed(Key.Right))
				_ball.SpeedX = Speed;
			else
				_ball.SpeedX = 0f;
		}

		private class Ball : IStepable, IRenderable
		{
			private readonly Sprite _sprite;

			public float SpeedX { get; set; }

			public float SpeedY { get; set; }

			public Ball (Texture texture)
			{
				_sprite = new Sprite(texture);
			}

			/// <summary>
			/// Updates the state of the component by a single step
			/// </summary>
			/// <param name="prev">Index of the previous state that was updated</param>
			/// <param name="next">Index of the next state that should be updated</param>
			/// <remarks>The only game state that should be modified during this process is the state indicated by <paramref name="next"/>.
			/// The state indicated by <paramref name="prev"/> can be used for reference (if needed), but should not be modified.
			/// Modifying any other game state info during this process would corrupt the game state.</remarks>
			public void Step (int prev, int next)
			{
				_sprite.X += SpeedX;
				_sprite.Y += SpeedY;
				_sprite.Step(prev, next);
			}

			/// <summary>
			/// Draws the state of a component
			/// </summary>
			/// <param name="display">Display to draw the state onto</param>
			/// <param name="state">Index of the state to draw</param>
			/// <remarks>None of the game states should be modified by this process - including the state indicated by <paramref name="state"/>.
			/// Modifying the game state info during this process would corrupt the game state.</remarks>
			public void Draw (IDisplay display, int state)
			{
				_sprite.Draw(display, state);
			}
		}
	}
}
