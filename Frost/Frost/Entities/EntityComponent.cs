using System;
using Frost.Logic;
using Frost.Utility;

namespace Frost.Entities
{
	/// <summary>
	/// Base class for all entity components.
	/// An entity component is a piece of functionality that an entity can have.
	/// </summary>
	public abstract class EntityComponent<T> where T : EntityComponentState<T>
	{
		public abstract StateSet<T> GetInitialStates ();
	}
}
