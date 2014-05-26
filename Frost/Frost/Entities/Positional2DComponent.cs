using System.Collections.Generic;

namespace Frost.Entities
{
	/// <summary>
	/// Information about an entity's location in 2D space
	/// </summary>
	public class Positional2DComponent : IEntityComponent
	{
		private readonly StateSet<State> _states;

		/// <summary>
		/// Information about the entity's location for each state
		/// </summary>
		public StateSet<State> States
		{
			get { return _states; }
		}

		/// <summary>
		/// Creates a new component for an entity to exist in 2D space
		/// </summary>
		public Positional2DComponent ()
		{
			var initialStates = new List<State>(StateManager.StateCount) { new State(), new State(), new State() };
			_states = new StateSet<State>(initialStates);
		}

		/// <summary>
		/// Information about the position of an entity for a given state
		/// </summary>
		public class State
		{
			/// <summary>
			/// Offset of the entity from the origin along the x-axis
			/// </summary>
			public float X { get; set; }

			/// <summary>
			/// Offset of the entity from the origin along the y-axis
			/// </summary>
			public float Y { get; set; }
		}
	}
}
