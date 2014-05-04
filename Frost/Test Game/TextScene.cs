using Frost;
using Frost.Display;
using Frost.Graphics;
using Frost.Graphics.Text;

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
			const string text = "\\b{Hello!}\nThis is a \\i{test} of the \\u{text rendering system.}";
			var appearance    = new TextAppearance(Font.GetDefaultFont(), 20);
			var liveText      = new LiveTextString(text, appearance);
			using(var renderer = new LiveTextRenderer(liveText))
			{
				renderer.MultiLine = true;
				renderer.WordWrap  = true;
				renderer.WrapWidth = 200;
				_sprite = new Sprite(renderer.GetTexture()) {X = 50, Y = 200};
			}
		}

		public override bool AllowFallthrough
		{
			get { return false; }
		}

		public override void Step (FrameStepEventArgs args)
		{
			base.Step(args);
			_sprite.Step(args);
		}

		public override void Draw (IDisplay display, FrameDrawEventArgs args)
		{
			base.Draw(display, args);
			_sprite.Draw(display, args);
		}
	}
}
