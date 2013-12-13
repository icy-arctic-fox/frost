using System;
using Frost.Display;
using Frost.Modules;

namespace Frost
{
	/// <summary>
	/// Base class for all games that use the Frost engine
	/// </summary>
	public abstract class Game
	{
		protected readonly Window Window;

		protected readonly StateManager Manager;

		public Game ()
		{
			throw new NotImplementedException();
		}
	}
}
