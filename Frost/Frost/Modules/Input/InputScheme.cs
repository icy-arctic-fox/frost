using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Maps input to easier to recognize values
	/// </summary>
	public abstract class InputScheme
	{
		/// <summary>
		/// Assigns an ID to an input
		/// </summary>
		/// <param name="id">ID to assign</param>
		/// <param name="input">Description of the input</param>
		protected void AssignInput (int id, InputDescriptor input)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes an assignment of an ID to an input
		/// </summary>
		/// <param name="id">ID of the input to un-assign</param>
		protected void UnassignInput (int id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reusable event arguments since input is triggered frequently
		/// </summary>
		private readonly InputEventArgs _inputEventArgs = new InputEventArgs();

		/// <summary>
		/// Called when an assigned input is detected
		/// </summary>
		/// <param name="args">Arguments for the input</param>
		protected void OnInputReceived (InputEventArgs args)
		{
			throw new NotImplementedException();
		}
	}
}
