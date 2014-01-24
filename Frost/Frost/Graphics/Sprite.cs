using Frost.Display;
using Frost.Logic;

namespace Frost.Graphics
{
	/// <summary>
	/// Most basic type used to display a 2D graphic on the screen
	/// </summary>
	public class Sprite : IStepableNode, IDrawableNode
	{
		/// <summary>
		/// Underlying SFML implementation of the sprite
		/// </summary>
		private readonly SFML.Graphics.Sprite _sprite;

		/// <summary>
		/// Underlying SFML implementation of the sprite
		/// </summary>
		internal SFML.Graphics.Sprite InternalSprite
		{
			get { return _sprite; }
		}

		/// <summary>
		/// Creates a new sprite
		/// </summary>
		/// <param name="texture">Texture to apply to the sprite</param>
		public Sprite (Texture texture)
		{
			_sprite    = new SFML.Graphics.Sprite(texture.InternalTexture);
			var states = new[] {InitialState, InitialState, InitialState};
			_states    = new StateSet<SFML.Graphics.RenderStates>(states);
		}

		#region Properties

		/// <summary>
		/// Offset of the sprite along the x and y-axis.
		/// </summary>
		private float _x, _y;

		/// <summary>
		/// X-coordinate of the sprite
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
		/// Y-coordinate of the sprite
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

		#region Update and render

		/// <summary>
		/// The initial state of the sprite
		/// </summary>
		private static SFML.Graphics.RenderStates InitialState
		{
			get { return SFML.Graphics.RenderStates.Default; }
		}

		/// <summary>
		/// Multiple states of the sprite.
		/// Each state contains information about blending, transformation, rotation, etc.
		/// </summary>
		private readonly StateSet<SFML.Graphics.RenderStates> _states;

		/// <summary>
		/// Flag that indicates if the sprite's properties have changed since the last update
		/// </summary>
		private bool _dirty;

		/// <summary>
		/// Updates the state of the sprite
		/// </summary>
		/// <param name="prev">Index of the previous state</param>
		/// <param name="next">Index of the state to update</param>
		public void StepState (int prev, int next)
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
		public void DrawState (IDisplay display, int state)
		{
			display.Draw(_sprite, _states[state]);
		}
		#endregion
	}
}
