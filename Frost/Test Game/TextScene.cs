using System;
using Frost.Display;
using Frost.Graphics;
using Frost.Graphics.Text;
using Frost.Logic;

namespace Test_Game
{
	class TextScene : Scene
	{
		private readonly TextSprite _ts;

		/// <summary>
		/// Creates the base of the scene
		/// </summary>
		public TextScene ()
		{
			_ts = new TextSprite("Hello!");
			SetUpdateRoot(_ts);
			SetRenderRoot(_ts);
		}

		private class TextSprite : IStepable, IRenderable
		{
			private readonly SimpleText _text;

			public TextSprite (string text)
			{
				var font = Font.LoadFromFile("../../../../Resources/Fonts/coolvetica.ttf");
				_text = new SimpleText(font, 16, new Color(0, 0, 255)) { Text = String.Join(" ", text, text, text) };
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
				// ...
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
				_text.Draw(display);
			}
		}
	}
}
