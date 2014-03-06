using Frost.Logic;

namespace Frost.Entities
{
	/// <summary>
	/// Component that allows an entity to be represented in 2D space
	/// </summary>
	public class Position2DEntityComponent : EntityComponent
	{
		/// <summary>
		/// State information
		/// </summary>
		public readonly StateSet<State> States;

		/// <summary>
		/// Creates a new 2D positional component
		/// </summary>
		public Position2DEntityComponent ()
		{
			var initialStates = new State[StateManager.StateCount];
			for(var i = 0; i < initialStates.Length; ++i)
				initialStates[i] = new State();
			States = new StateSet<State>(initialStates);
		}

		/// <summary>
		/// State information for a 2D positional entity
		/// </summary>
		public class State : EntityComponentState
		{
			/// <summary>
			/// Position of the entity in two-dimensional space
			/// </summary>
			public float X, Y;
		}
	}
}
