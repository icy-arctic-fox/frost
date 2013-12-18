using System;
using System.Collections.Generic;

namespace Frost.Modules.State
{
	/// <summary>
	/// Collection of state data.
	/// Each updatable object needs to have multiple states.
	/// This class aids the management of those states.
	/// </summary>
	/// <typeparam name="TStateData">Type used to store state information</typeparam>
	public class StateSet<TStateData>
	{
		/// <summary>
		/// References to the state data
		/// </summary>
		private readonly TStateData[] _stateData = new TStateData[StateManager.StateCount];

		/// <summary>
		/// Creates a new collection of states
		/// </summary>
		/// <param name="initialStates">Initial states</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="initialStates"/> is null.
		/// The collection of states to initially used cannot be null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="initialStates"/> does not have enough states.
		/// The collection must have enough states for the <see cref="StateManager"/>.</exception>
		public StateSet (IList<TStateData> initialStates)
		{
			if(null == initialStates)
				throw new ArgumentNullException("initialStates", "The collection of initial states cannot be null.");
			if(initialStates.Count < _stateData.Length)
				throw new ArgumentOutOfRangeException("initialStates", initialStates.Count, "The number of initial states should be " + _stateData.Length);

			for(var i = 0; i < _stateData.Length; ++i)
				_stateData[i] = initialStates[i];
		}

		/// <summary>
		/// Access to the states
		/// </summary>
		/// <param name="index">Index of the state to access</param>
		/// <returns>State information</returns>
		/// <exception cref="IndexOutOfRangeException">Thrown if <paramref name="index"/> is an invalid state index</exception>
		public TStateData this[int index]
		{
			get
			{
#if DEBUG
				if(index < 0 || index >= _stateData.Length)
					throw new IndexOutOfRangeException("Out of range state index");
#endif
				return _stateData[index];
			}
			
			set
			{
#if DEBUG
				if(index < 0 || index >= _stateData.Length)
					throw new IndexOutOfRangeException("Out of range state index");
#endif
				_stateData[index] = value;
			}
		}
	}
}
