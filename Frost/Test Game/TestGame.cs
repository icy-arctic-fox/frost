﻿using Frost;
using Frost.Logic;

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

		/// <summary>
		/// Creates the initial scene
		/// </summary>
		/// <returns>A scene</returns>
		protected override Scene CreateInitialScene ()
		{
			return new BallScene(this);
		}
	}
}