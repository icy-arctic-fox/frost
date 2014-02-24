using System;

namespace Frost.Geometry
{
	/// <summary>
	/// Defines a rotation and length in two-dimensions
	/// </summary>
	public struct Vector2
	{
		/// <summary>
		/// Vector with no rotation or length
		/// </summary>
		public static readonly Vector2 Empty = new Vector2(0f, 0f);

		private readonly float _rot, _len;

		/// <summary>
		/// Rotation in degrees
		/// </summary>
		public float Rotation
		{
			get { return _rot; }
		}

		/// <summary>
		/// Length of the vector
		/// </summary>
		public float Length
		{
			get { return _len; }
		}

		/// <summary>
		/// Creates a new two-dimensional vector
		/// </summary>
		/// <param name="rotation">Rotation measured in degrees</param>
		/// <param name="length">Length of the vector</param>
		public Vector2 (float rotation, float length)
		{
			_rot = rotation % 360f;
			if(_rot < 0f)
				_rot += 360f;
			_len = length;
		}

		/// <summary>
		/// Creates a new vector by applying a rotation
		/// </summary>
		/// <param name="angle">Angle to add in degrees</param>
		/// <returns>A rotated vector</returns>
		public Vector2 Rotate (float angle)
		{
			return new Vector2(_rot + angle, _len);
		}

		/// <summary>
		/// Creates a new vector by scaling the length
		/// </summary>
		/// <param name="factor">Scaling factor</param>
		/// <returns>A scaled vector</returns>
		public Vector2 Scale (float factor)
		{
			return new Vector2(_rot, _len * factor);
		}

		#region Operators
		#region Equality

		public static bool operator == (Vector2 left, Vector2 right)
		{
			throw new NotImplementedException();
		}

		public static bool operator != (Vector2 left, Vector2 right)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Mathematical

		public static Vector2 operator + (Vector2 left, Vector2 right)
		{
			throw new NotImplementedException();
		}

		public static Vector2 operator - (Vector2 left, Vector2 right)
		{
			throw new NotImplementedException();
		}

		public static Vector2 operator * (Vector2 vector, float factor)
		{
			throw new NotImplementedException();
		}

		public static float operator * (Vector2 left, Vector2 right)
		{
			throw new NotImplementedException();
		}

		public static Vector2 operator / (Vector2 vector, float factor)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Explicit conversions

		public static explicit operator Vector2 (Point2f point)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Point2f (Vector2 vector)
		{
			throw new NotImplementedException();
		}
		#endregion
		#endregion

		public override bool Equals (object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode();
		}

		public override string ToString ()
		{
			return base.ToString();
		}
	}
}
