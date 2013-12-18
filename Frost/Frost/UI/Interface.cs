using System;
using Frost.Modules.State;

namespace Frost.UI
{
	/// <summary>
	/// Top-level
	/// </summary>
	public class Interface : IStepableNode, IDrawableNode
	{
		/// <summary>
		/// Updates the state of the interface
		/// </summary>
		/// <param name="prev"></param>
		/// <param name="next"></param>
		public void StepState (int prev, int next)
		{
			throw new NotImplementedException();
		}

		public void DrawState (int state)
		{
			throw new NotImplementedException();
		}
	}
}
