﻿using System;
using Frost.Display;
using Frost.Graphics;

namespace Frost.UI
{
	/// <summary>
	/// Top-level interface overlay
	/// </summary>
	public class Interface : IStepableNode, IDrawableNode
	{
		/// <summary>
		/// Updates the state of the interface
		/// </summary>
		/// <param name="prev">Index of the previous state</param>
		/// <param name="next">Index of the next state to update</param>
		public void StepState (int prev, int next)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the state of the interface
		/// </summary>
		/// <param name="display">Display to draw the interface on</param>
		/// <param name="state">Index of the state to draw</param>
		public void DrawState (IDisplay display, int state)
		{
			throw new NotImplementedException();
		}
	}
}
