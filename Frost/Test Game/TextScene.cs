using System;
using Frost.Display;
using Frost.Graphics;
using Frost.Graphics.Text;
using Frost.Logic;

namespace Test_Game
{
	class TextScene : Scene
	{
		public override string Name
		{
			get { return "Text"; }
		}

		private readonly Sprite _sprite;

		/// <summary>
		/// Creates the base of the scene
		/// </summary>
		public TextScene ()
		{
			using(var renderer = new PlainTextRenderer())
			{
				renderer.Font = Font.LoadFromFile("coolvetica.ttf");
				renderer.Text = "Hello!\nThis is a test of the text rendering system.";
				_sprite = new Sprite(renderer.GetTexture()) {X = 50, Y = 200};
			}
		}

		public override void Draw (IDisplay display, int state, double t)
		{
			base.Draw(display, state, t);
			_sprite.Draw(display, state, t);
		}
	}
}
