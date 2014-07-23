using Frost.Display;
using SFML.Graphics;

namespace Frost.Graphics
{
	/// <summary>
	/// A two-dimensional object that drawn
	/// </summary>
	public abstract class Object2D
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
			set { _x = value; }
		}

		/// <summary>
		/// Y-coordinate of the object
		/// </summary>
		public float Y
		{
			get { return _y; }
			set { _y = value; }
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
			set { _sx = value; }
		}

		/// <summary>
		/// Vertical scaling.
		/// Less than 1 will shrink the object, greater than 1 will stretch the object.
		/// </summary>
		public float ScaleY
		{
			get { return _sy; }
			set { _sy = value; }
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
			}
		}

		private float _rot;

		/// <summary>
		/// Clockwise rotation of the object in degrees
		/// </summary>
		public float Rotation
		{
			get { return _rot; }
			set { _rot = value; }
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
		}

		/// <summary>
		/// Draws the underlying implementation of the 2D object
		/// </summary>
		/// <param name="display">Display to draw to</param>
		/// <param name="transform">Transformation to apply to the object</param>
		/// <param name="t">Interpolation value</param>
		public abstract void DrawObject (IDisplay display, RenderStates transform, double t);
	}
}
