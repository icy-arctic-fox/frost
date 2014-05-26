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
		/// Constructor used for cloning
		/// </summary>
		/// <param name="initialStates">Initial states for the component</param>
		private Positional2DComponent (IList<State> initialStates)
		{
			_states = new StateSet<State>(initialStates);
		}

		/// <summary>
		/// Creates a copy of the information in the component
		/// </summary>
		/// <returns>Copy of the component</returns>
		public IEntityComponent CloneComponent ()
		{
			var clonedStates = new List<State>(StateManager.StateCount);
			for(var i = 0; i < StateManager.StateCount; ++i)
				clonedStates.Add(_states[i].CloneState());
			return new Positional2DComponent(clonedStates);
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

			/// <summary>
			/// Creates a new state with a location at the origin
			/// </summary>
			public State ()
			{
				X = 0f;
				Y = 0f;
			}

			/// <summary>
			/// Creates a new state with the provided location
			/// </summary>
			/// <param name="x">Offset of the entity from the origin along the x-axis</param>
			/// <param name="y">Offset of the entity from the origin along the y-axis</param>
			public State (float x, float y)
			{
				X = x;
				Y = y;
			}

			/// <summary>
			/// Creates a copy of the entity component state
			/// </summary>
			/// <returns>Copy of the state</returns>
			public State CloneState ()
			{
				return new State(X, Y);
			}
		}
	}
}
