using System;
using System.Collections.Generic;
using Frost;
using Frost.Logic;
using Frost.Modules.Input;

namespace Test_Game
{
	class TestGame : Game
	{
		/// <summary>
		/// Visible name of the game
		/// </summary>
		/// <remarks>This text is displayed on the window title</remarks>
		public override string GameTitle
		{
			get { return "Test Game"; }
		}

		/// <summary>
		/// Short name of the game.
		/// This is used internally and for file paths.
		/// </summary>
		/// <remarks>This string should contain no whitespace or symbols.
		/// Lowercase is also preferred.</remarks>
		public override string ShortGameName
		{
			get { return "testgame"; }
		}

		/// <summary>
		/// Number of game updates processed per second
		/// </summary>
		protected override double UpdateRate
		{
			get { return 60d; }
		}

		private BallScene _scene;

		/// <summary>
		/// Creates the initial scene
		/// </summary>
		/// <returns>A scene</returns>
		protected override Scene CreateInitialScene ()
		{
			return _scene = new BallScene();
		}

		protected override void InitializeModules ()
		{
			base.InitializeModules();
			Window.MouseMove    += Mouse_Move;
			Keyboard.KeyPress   += Keyboard_KeyPress;
			Keyboard.KeyRelease += Keyboard_KeyRelease;

			var controller = new BasicController {
				UpArrow    = new InputDescriptor(InputType.Keyboard, (int)Key.Up),
				DownArrow  = new InputDescriptor(InputType.Keyboard, (int)Key.Down),
				LeftArrow  = new InputDescriptor(InputType.Keyboard, (int)Key.Left),
				RightArrow = new InputDescriptor(InputType.Keyboard, (int)Key.Right)
			};

			_scene.SetController(controller);
		}

		private readonly HashSet<Key> _keys = new HashSet<Key>();

		void Keyboard_KeyRelease (object sender, KeyboardEventArgs e)
		{
			_keys.Remove(e.Key);
			Window.Title = String.Join(", ", _keys);
		}

		void Keyboard_KeyPress (object sender, KeyboardEventArgs e)
		{
			_keys.Add(e.Key);
			Window.Title = String.Join(", ", _keys);
		}

		void Mouse_Move (object sender, MouseEventArgs e)
		{
			Window.Title = e.Position.ToString();
		}
	}
}
