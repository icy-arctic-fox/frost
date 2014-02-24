using System;

namespace Frost.Geometry
{
	/// <summary>
	/// Defines a rotation and length
	/// </summary>
	public struct Vector2
	{
		public static readonly Vector2 Empty = new Vector2(0f, 0f);

		private readonly float _rot, _len;

		public float Rotation
		{
			get { return _rot; }
		}

		public float Length
		{
			get { return _len; }
		}

		public Vector2 (float rotation, float length)
		{
			throw new NotImplementedException();
		}

		public Vector2 Rotate (float angle)
		{
			throw new NotImplementedException();
		}

		public Vector2 Scale (float factor)
		{
			throw new NotImplementedException();
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
