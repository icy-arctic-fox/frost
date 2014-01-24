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

		private readonly UpdateMethod _update;
		private readonly DrawMethod _render;

		/// <summary>
		/// Creates the base of the scene
		/// </summary>
		/// <param name="update">Object to update every logic tick</param>
		/// <param name="render">Object to render every frame</param>
		protected Scene (IStepable update, IRenderable render)
		{
			if(update == null)
				throw new ArgumentNullException("update", "The scene update object can't be null.");
			if(render == null)
				throw new ArgumentNullException("render", "The scene render object can't be null.");

			_update = update.Step;
			_render = render.Draw;
		}

		/// <summary>
		/// Updates the state of the scene by a single step
		/// </summary>
		/// <param name="prev">Index of the previous state that was updated</param>
		/// <param name="next">Index of the next state that should be updated</param>
		/// <remarks>The only game state that should be modified during this process is the state indicated by <paramref name="next"/>.
		/// The state indicated by <paramref name="prev"/> can be used for reference (if needed), but should not be modified.
		/// Modifying any other game state info during this process would corrupt the game state.</remarks>
		public void Step (int prev, int next)
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
		public void Draw (IDisplay display, int state)
		{
			_render(display, state);
		}
	}
}
