using Frost.Display;
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

		private readonly Ball _ball;

		/// <summary>
		/// Creates the base of the scene
		/// </summary>
		public BallScene ()
		{
			var texture = new Texture("ball-6x6.png");
			_ball = new Ball(texture);
		}

		private class Ball : IStepable, IRenderable
		{
			private readonly Sprite _sprite;

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
				_sprite.Step(prev, next);
			}

			/// <summary>
			/// Draws the state of a component
			/// </summary>
			/// <param name="display">Display to draw the state onto</param>
			/// <param name="state">Index of the state to draw</param>
			/// <param name="t">Interpolation value</param>
			/// <remarks>None of the game states should be modified by this process - including the state indicated by <paramref name="state"/>.
			/// Modifying the game state info during this process would corrupt the game state.</remarks>
			public void Draw (IDisplay display, int state, double t)
			{
				_sprite.Draw(display, state, t);
			}
		}
	}
}
