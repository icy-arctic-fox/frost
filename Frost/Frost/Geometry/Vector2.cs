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
		/// <remarks>Rotation angles larger than 360° or smaller than 0° are automatically corrected.</remarks>
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
		/// <returns>Resulting <see cref="Vector2"/></returns>
		public Vector2 Rotate (float angle)
		{
			return new Vector2(_rot + angle, _len);
		}

		/// <summary>
		/// Creates a new vector by scaling the length
		/// </summary>
		/// <param name="factor">Scaling factor</param>
		/// <returns>Resulting <see cref="Vector2"/></returns>
		public Vector2 Scale (float factor)
		{
			return new Vector2(_rot, _len * factor);
		}

		#region Operators
		#region Equality

		/// <summary>
		/// Compares two vectors for equality
		/// </summary>
		/// <param name="left">First vector to compare</param>
		/// <param name="right">Second vector to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are equal vectors</returns>
		/// <remarks>Vectors are considered equal if their <see cref="Rotation"/> and <see cref="Length"/> properties are the same.</remarks>
		public static bool operator == (Vector2 left, Vector2 right)
		{
			return (Math.Abs(left._len - right._len) < Single.Epsilon) &&
				(Math.Abs(left._rot - right._rot) < Single.Epsilon);
		}

		/// <summary>
		/// Compares two vectors for inequality
		/// </summary>
		/// <param name="left">First vector to compare</param>
		/// <param name="right">Second vector to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are not equal vectors</returns>
		/// <remarks>Vectors are considered equal if their <see cref="Rotation"/> and <see cref="Length"/> properties are the same.</remarks>
		public static bool operator != (Vector2 left, Vector2 right)
		{
			return (Math.Abs(left._len - right._len) >= Single.Epsilon) ||
				(Math.Abs(left._rot - right._rot) >= Single.Epsilon);
		}
		#endregion

		#region Mathematical

		/// <summary>
		/// Adds two vectors together
		/// </summary>
		/// <param name="left">First vector</param>
		/// <param name="right">Second vector</param>
		/// <returns>Combined <see cref="Vector2"/></returns>
		public static Vector2 operator + (Vector2 left, Vector2 right)
		{
			var rot = left._rot + right._rot;
			var len = left._len + right._len;
			return new Vector2(rot, len);
		}

		/// <summary>
		/// Subtracts one vector from another
		/// </summary>
		/// <param name="left">Original vector</param>
		/// <param name="right"><see cref="Vector2"/> to subtract from <paramref name="left"/></param>
		/// <returns>Resulting <see cref="Vector2"/></returns>
		public static Vector2 operator - (Vector2 left, Vector2 right)
		{
			var rot = left._rot - right._rot;
			var len = left._len - right._len;
			return new Vector2(rot, len);
		}

		/// <summary>
		/// Scales the length of a vector
		/// </summary>
		/// <param name="vector">Original vector</param>
		/// <param name="factor">Factor to scale <paramref name="vector"/> by</param>
		/// <returns>Resulting <see cref="Vector2"/></returns>
		public static Vector2 operator * (Vector2 vector, float factor)
		{
			var len = vector._len * factor;
			return new Vector2(vector._rot, len);
		}

		/// <summary>
		/// Performs a dot product calculation on two vectors
		/// </summary>
		/// <param name="left">First vector</param>
		/// <param name="right">Second vector</param>
		/// <returns>Dot product of <paramref name="left"/> * <paramref name="right"/></returns>
		public static float operator * (Vector2 left, Vector2 right)
		{
			var p1 = (Point2f)left;
			var p2 = (Point2f)right;

			var x = p1.X * p2.X;
			var y = p2.Y * p2.Y;

			return x + y;
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

		/// <summary>
		/// Creates the string representation of the vector
		/// </summary>
		/// <returns>A string in the form:
		/// LENGTH ROTATION°</returns>
		public override string ToString ()
		{
			return String.Format("{0} {1}°", _len, _rot);
		}

		/// <summary>
		/// Determines whether another object is equal to the current vector
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if <paramref name="obj"/> is equal</returns>
		/// <remarks><paramref name="obj"/> is considered equal if it is a <see cref="Vector2"/> with the same <see cref="Rotation"/> and <see cref="Length"/> properties
		/// or if a <see cref="Vector2"/> formed from a <see cref="Point2f"/> is equivalent.</remarks>
		public override bool Equals (object obj)
		{
			if(obj is Vector2)
				return this == (Vector2)obj;
			if(obj is Point2f)
				return this == (Vector2)(Point2f)obj;
			return false;
		}

		/// <summary>
		/// Creates a hash code from the values of the vector
		/// </summary>
		/// <returns>A hash code</returns>
		public override int GetHashCode ()
		{
			var hash = 17;
			hash = 31 * hash + _rot.GetHashCode();
			hash = 31 * hash + _len.GetHashCode();
			return hash;
		}
	}
}
