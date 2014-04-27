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
			const string text = "\\b{Hello!}\nThis is a \\i{test} of the text rendering system.";
			using(var renderer = new LiveTextRenderer(text))
			{
				renderer.MultiLine = true;
				renderer.WordWrap  = true;
				renderer.WrapWidth = 200;
				_sprite = new Sprite(renderer.GetTexture()) {X = 50, Y = 200};
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
