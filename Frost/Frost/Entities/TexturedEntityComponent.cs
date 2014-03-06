using Frost.Graphics;
using Frost.Logic;

namespace Frost.Entities
{
	public class TexturedEntityComponent : EntityComponent
	{
		public readonly Texture Texture;
		public readonly StateSet<State> States;

		public TexturedEntityComponent (Texture texture)
		{
			Texture = texture;
			var initialStates = new State[StateManager.StateCount];
			for(var i = 0; i < initialStates.Length; ++i)
				initialStates[i] = new State();
			States = new StateSet<State>(initialStates);
		}

		public class State : EntityComponentState
		{
			
		}
	}
}
