using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frost.Entities
{
	/// <summary>
	/// Component that allows an entity to be represented in 2D space
	/// </summary>
	public class Position2DEntityComponent : EntityComponent<Position2DEntityComponent.State>
	{
		public override State[] GetInitialStates ()
		{
			throw new NotImplementedException();
		}

		public class State : EntityComponentState
		{
			private float _x, _y;
		}
	}
}
