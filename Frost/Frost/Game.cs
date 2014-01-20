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
		#region Game components and modules

		/// <summary>
		/// Window used to display graphics
		/// </summary>
		protected readonly Window Window;

		/// <summary>
		/// Manages the state and speed of the game
		/// </summary>
		protected readonly StateManager Manager;

		/// <summary>
		/// Provides access to all of the resources available for the game
		/// </summary>
		protected readonly ResourceManager Resources;
		#endregion

		/// <summary>
		/// Creates the underlying game
		/// </summary>
		protected Game ()
		{
			throw new NotImplementedException();
		}
	}
}
