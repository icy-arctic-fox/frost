using Frost;
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

		protected override void OnStep (FrameStepEventArgs args)
		{
			base.OnStep(args);
			_sprite.Step(args);
		}

		protected override void OnDraw (FrameDrawEventArgs args)
		{
			base.OnDraw(args);
			_sprite.Draw(args);
		}
	}
}
