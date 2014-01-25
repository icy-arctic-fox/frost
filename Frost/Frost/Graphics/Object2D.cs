using Frost.Display;
using Frost.Logic;
using SFML.Graphics;

namespace Frost.Graphics
{
	/// <summary>
	/// A two-dimensional object that can be updated and drawn
	/// </summary>
	public abstract class Object2D : IStepable, IRenderable
	{
		#region Properties

		/// <summary>
		/// Offset of the object along the x and y-axis.
		/// </summary>
		private float _x, _y;

		/// <summary>
		/// X-coordinate of the object
		/// </summary>
		public float X
		{
			get { return _x; }
			set
			{
				_x = value;
				_dirty = true;
			}
		}

		/// <summary>
		/// Y-coordinate of the object
		/// </summary>
		public float Y
		{
			get { return _y; }
			set
			{
				_y = value;
				_dirty = true;
			}
		}
		#endregion

		protected Object2D ()
		{
			var states = new[] { InitialState, InitialState, InitialState };
			_states    = new StateSet<RenderStates>(states);
		}
		
		#region Update and render

		/// <summary>
		/// The initial state of the sprite
		/// </summary>
		private static RenderStates InitialState
		{
			get { return RenderStates.Default; }
		}

		/// <summary>
		/// Multiple states of the sprite.
		/// Each state contains information about blending, transformation, rotation, etc.
		/// </summary>
		private readonly StateSet<RenderStates> _states;

		/// <summary>
		/// Flag that indicates if the sprite's properties have changed since the last update
		/// </summary>
		private bool _dirty;

		/// <summary>
		/// Updates the state of the sprite
		/// </summary>
		/// <param name="prev">Index of the previous state</param>
		/// <param name="next">Index of the state to update</param>
		public void Step (int prev, int next)
		{
			if(_dirty)
			{// Apply transformations to the next state
				var state = InitialState;
				state.Transform.Translate(_x, _y);

				_states[next] = state;
				_dirty = false;
			}
			else // No changes, copy the previous state
				_states[next] = _states[prev];
		}

		/// <summary>
		/// Draws a sprite's state to a display
		/// </summary>
		/// <param name="display">Display to draw to</param>
		/// <param name="state">Index of the state to draw</param>
		public void Draw (IDisplay display, int state)
		{
			display.Draw(/* TODO: _sprite */ null, _states[state]);
		}
		#endregion
	}
}
