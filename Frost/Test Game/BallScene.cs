using System;
using Frost;
using Frost.Display;
using Frost.Graphics;
using Frost.Logic;

namespace Test_Game
{
	class BallScene : Scene
	{
		/// <summary>
		/// Creates the base of the scene
		/// </summary>
		public BallScene (Game game)
		{
			var texture = new Texture("ball-6x6.png");
			var ball = new Ball(texture);
			SetUpdateRoot(ball);
			SetRenderRoot(ball);
		}

		private class Ball : IStepable, IRenderable
		{
			private readonly Sprite _sprite;
			private float _x;

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
				if(_sprite.X > 500f)
					_sprite.X = 0f;
				else
					_sprite.X += 3f;
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
