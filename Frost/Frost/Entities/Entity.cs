using System.Collections.Generic;
using Frost.Logic;

namespace Frost.Entities
{
	/// <summary>
	/// Base class for all entities.
	/// An entity is a fundamental object for a game.
	/// Entities can have any number of components (<see cref="EntityComponent"/>)
	/// which define characteristics of that entity.
	/// </summary>
	/// <seealso cref="EntityManager"/>
	public abstract class Entity : IStepable
	{
		private readonly ulong _id;

		/// <summary>
		/// Unique entity identifier
		/// </summary>
		public ulong Id
		{
			get { return _id; }
		}

		/// <summary>
		/// Creates a new entity
		/// </summary>
		/// <param name="id">Unique identifier returned from <see cref="EntityManager.NextAvailableId"/></param>
		protected Entity (ulong id)
		{
			_id = id;
		}

//		private readonly List<StateSet<EntityComponentState>> _componentStates = new List<StateSet<EntityComponentState>>();

		/// <summary>
		/// Updates the state of the component by a single step
		/// </summary>
		/// <param name="prev">Index of the previous state that was updated</param>
		/// <param name="next">Index of the next state that should be updated</param>
		/// <remarks>The only game state that should be modified during this process is the state indicated by <paramref name="next"/>.
		/// The state indicated by <paramref name="prev"/> can be used for reference (if needed), but should not be modified.
		/// Modifying any other game state info during this process would corrupt the game state.</remarks>
		public void Step (int prev, int next)
		{
/*			for(var i = 0; i < _componentStates.Count; ++i)
			{
				var states = _componentStates[i];
				var prevState = states[prev];
				var nextState = states[next];
				nextState.Step(prevState);
			} */
		}
	}
}
