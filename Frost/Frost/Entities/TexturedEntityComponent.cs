using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frost.Entities
{
	public class TexturedEntityComponent : EntityComponent<TexturedEntityComponent.State>
	{
		public override State[] GetInitialStates ()
		{
			throw new NotImplementedException();
		}

		public class State : EntityComponentState
		{
			
		}
	}
}
