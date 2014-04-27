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
			using(var font = Font.LoadFromFile("coolvetica.ttf"))
			{
				var appearance = new TextAppearance(font);
				using(var renderer = new LiveTextRenderer(appearance))
				{
					renderer.MultiLine = true;
					renderer.WordWrap  = true;
					renderer.WrapWidth = 200;
					renderer.Text = new LiveTextString("\\b{Hello!}\nThis is a \\i{test} of the text rendering system.", appearance);
					_sprite = new Sprite(renderer.GetTexture()) {X = 50, Y = 200};
				}
			}
		}

		public override void Step (int prev, int next)
		{
			base.Step(prev, next);
			_sprite.Step(prev, next);
		}

		public override void Draw (IDisplay display, int state, double t)
		{
			base.Draw(display, state, t);
			_sprite.Draw(display, state, t);
		}
	}
}
