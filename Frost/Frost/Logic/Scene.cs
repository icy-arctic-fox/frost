using System;
using Frost.Display;
using Frost.Graphics;

namespace Frost.Logic
{
	/// <summary>
	/// Logically separate section of the game
	/// </summary>
	public abstract class Scene : IStepable, IRenderable
	{
		/// <summary>
		/// Describes a method that updates the state of the game
		/// </summary>
		/// <param name="prev">Index of the previous state that was updated</param>
		/// <param name="next">Index of the next state that should be updated</param>
		private delegate void UpdateMethod (int prev, int next);

		/// <summary>
		/// Describes a method that draws a state to a display
		/// </summary>
		/// <param name="display">Display to draw the state onto</param>
		/// <param name="state">Index of the state to draw</param>
		private delegate void DrawMethod (IDisplay display, int state);

		private UpdateMethod _update;
		private DrawMethod _render;

		/// <summary>
		/// Sets the root scene object that will be updated every tick
		/// </summary>
		/// <param name="updateRoot">Updatable root object</param>
		protected void SetUpdateRoot (IStepable updateRoot)
		{
			if(updateRoot == null)
				throw new ArgumentNullException("updateRoot", "The root scene update object can't be null.");
			_update = updateRoot.Step;
		}

		/// <summary>
		/// Sets the root scene object that will be rendered every frame
		/// </summary>
		/// <param name="renderRoot">Renderable root object</param>
		protected void SetRenderRoot (IRenderable renderRoot)
		{
			if(renderRoot == null)
				throw new ArgumentNullException("renderRoot", "The root scene render object can't be null.");
			_render = renderRoot.Draw;
		}

		/// <summary>
		/// Updates the state of the scene by a single step
		/// </summary>
		/// <param name="prev">Index of the previous state that was updated</param>
		/// <param name="next">Index of the next state that should be updated</param>
		/// <remarks>The only game state that should be modified during this process is the state indicated by <paramref name="next"/>.
		/// The state indicated by <paramref name="prev"/> can be used for reference (if needed), but should not be modified.
		/// Modifying any other game state info during this process would corrupt the game state.</remarks>
		public virtual void Step (int prev, int next)
		{
			_update(prev, next);
		}

		/// <summary>
		/// Draws the state of the scene
		/// </summary>
		/// <param name="display">Display to draw the scene onto</param>
		/// <param name="state">Index of the state to draw</param>
		/// <remarks>None of the game states should be modified by this process - including the state indicated by <paramref name="state"/>.
		/// Modifying the game state info during this process would corrupt the game state.</remarks>
		public virtual void Draw (IDisplay display, int state)
		{
			_render(display, state);
		}
	}
}
