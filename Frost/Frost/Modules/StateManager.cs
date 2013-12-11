using System;

namespace Frost.Modules
{
	/// <summary>
	/// Tracks the frames to be updated and rendered
	/// </summary>
	public class StateManager
	{
		/// <summary>
		/// Creates a new state manager with the update and render root nodes
		/// </summary>
		/// <param name="updateRoot">Root node for updating the game state</param>
		/// <param name="renderRoot">Root node for rendering the game state</param>
		/// <remarks>Generally, <paramref name="updateRoot"/> and <paramref name="renderRoot"/> are the same object.
		/// They can be different for more complex situations.</remarks>
		public StateManager (IStepableNode updateRoot, IDrawableNode renderRoot)
		{
			throw new NotImplementedException();
		}
	}
}
