using Frost.Display;
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

		/// <summary>
		/// Amount of scaling
		/// </summary>
		private float _sx = 1f, _sy = 1f;

		/// <summary>
		/// Horizontal scaling.
		/// Less than 1 will shrink the object, greater than 1 will stretch the object.
		/// </summary>
		public float ScaleX
		{
			get { return _sx; }
			set
			{
				_sx = value;
				_dirty = true;
			}
		}

		/// <summary>
		/// Vertical scaling.
		/// Less than 1 will shrink the object, greater than 1 will stretch the object.
		/// </summary>
		public float ScaleY
		{
			get { return _sy; }
			set
			{
				_sy = value;
				_dirty = true;
			}
		}

		/// <summary>
		/// Adjusts the horizontal and vertical scaling to the same value
		/// </summary>
		public float Scale
		{
			set
			{
				_sx = value;
				_sy = value;
				_dirty = true;
			}
		}

		private float _rot;

		/// <summary>
		/// Clockwise rotation of the object in degrees
		/// </summary>
		public float Rotation
		{
			get { return _rot; }
			set
			{
				_rot = value;
				_dirty = true;
			}
		}

		/// <summary>
		/// Width of the object in pixels (before scaling)
		/// </summary>
		public abstract float Width { get; }

		/// <summary>
		/// Height of the object in pixels (before scaling)
		/// </summary>
		public abstract float Height { get; }

		// TODO: Add bounds property
		#endregion

		protected Object2D ()
		{
			var states = new[] { InitialState, InitialState, InitialState };
			_states    = new StateSet<RenderStates>(states);
		}
		
		#region Update and render

		/// <summary>
		/// The initial transformation state of a 2D object
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
		/// Updates the state of the object
		/// </summary>
		/// <param name="args">Update information</param>
		/// <remarks>The only game state that should be modified during this process is the state indicated by <see cref="FrameStepEventArgs.NextStateIndex"/>.
		/// The state indicated by <see cref="FrameStepEventArgs.PreviousStateIndex"/> can be used for reference (if needed), but should not be modified.
		/// Modifying any other game state info during this process could corrupt the game state.</remarks>
		public void Step (FrameStepEventArgs args)
		{
			var prev = args.PreviousStateIndex;
			var next = args.NextStateIndex;

			if(_dirty)
			{// Apply transformations to the next state
				var state = InitialState;
				state.Transform.Scale(_sx, _sy);
				state.Transform.Rotate(_rot);
				state.Transform.Translate(_x, _y);

				_states[next] = state;
				_dirty = false;
			}
			else // No changes, copy the previous state
				_states[next] = _states[prev];
		}

		/// <summary>
		/// Draws the underlying implementation of the 2D object
		/// </summary>
		/// <param name="display">Display to draw to</param>
		/// <param name="transform">Transformation to apply to the object</param>
		/// <param name="t">Interpolation value</param>
		protected abstract void DrawObject (IDisplay display, RenderStates transform, double t);

		/// <summary>
		/// Draws a object's state to a display
		/// </summary>
		/// <param name="display">Display to draw to</param>
		/// <param name="args">Render information</param>
		/// <remarks>None of the game states should be modified by this process - including the state indicated by <see cref="FrameDrawEventArgs.StateIndex"/>.
		/// Modifying the game state info during this process would corrupt the game state.</remarks>
		public void Draw (IDisplay display, FrameDrawEventArgs args)
		{
			DrawObject(display, _states[args.StateIndex], args.Interpolation);
		}
		#endregion
	}
}
