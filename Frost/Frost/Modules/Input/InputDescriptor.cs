namespace Frost.Modules.Input
{
	/// <summary>
	/// Information about an input source and input ID (button/key)
	/// </summary>
	public struct InputDescriptor
	{
		private readonly InputType _type;
		private readonly int _id;

		/// <summary>
		/// Type of input
		/// </summary>
		public InputType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// ID associated with the input
		/// </summary>
		public int Id
		{
			get { return _id; }
		}

		/// <summary>
		/// Creates a new input ID
		/// </summary>
		/// <param name="type">Type of input</param>
		/// <param name="id">ID (button/key) associated with the input</param>
		public InputDescriptor (InputType type, int id)
		{
			_type = type;
			_id   = id;
		}

		/// <summary>
		/// Compares two input descriptors to check if they're equal
		/// </summary>
		/// <param name="a">First input descriptor</param>
		/// <param name="b">Second input descriptor</param>
		/// <returns>True if <paramref name="a"/> and <paramref name="b"/> are equal</returns>
		public static bool operator == (InputDescriptor a, InputDescriptor b)
		{
			return (a._type == b._type) && (a._id == b._id);
		}

		/// <summary>
		/// Compares two input descriptors to check if they're not equal
		/// </summary>
		/// <param name="a">First input descriptor</param>
		/// <param name="b">Second input descriptor</param>
		/// <returns>True if <paramref name="a"/> and <paramref name="b"/> are not equal</returns>
		public static bool operator != (InputDescriptor a, InputDescriptor b)
		{
			return (a._type != b._type) || (a._id != b._id);
		}

		/// <summary>
		/// Checks if an another <see cref="InputDescriptor"/> is equal to the current instance
		/// </summary>
		/// <param name="other">Other <see cref="InputDescriptor"/> to compare against</param>
		/// <returns>True if <paramref name="other"/> is effectively the same</returns>
		public bool Equals (InputDescriptor other)
		{
			return (_type == other._type) && (_id == other._id);
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal
		/// </summary>
		/// <returns>True if <paramref name="obj"/> and this instance are the same type (an <see cref="InputDescriptor"/>) and represent the same value; otherwise, false</returns>
		/// <param name="obj">Another object to compare to</param>
		public override bool Equals (object obj)
		{
			if(!ReferenceEquals(null, obj))
				return obj is InputDescriptor && Equals((InputDescriptor)obj);
			return false;
		}

		/// <summary>
		/// Returns the hash code for this instance
		/// </summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance</returns>
		public override int GetHashCode ()
		{
			unchecked
			{
				return ((int)_type * 397) ^ _id;
			}
		}
	}
}
