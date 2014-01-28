using System;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Information about an input source and input value (button/key)
	/// </summary>
	public struct InputDescriptor
	{
		/// <summary>
		/// An input that doesn't have an assigned type and value
		/// </summary>
		public static readonly InputDescriptor Unassigned = new InputDescriptor(InputType.Unassigned, 0);

		private readonly InputType _type;
		private readonly int _value;

		/// <summary>
		/// Type of input
		/// </summary>
		public InputType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Value (button/key) associated with the input
		/// </summary>
		public int Value
		{
			get { return _value; }
		}

		/// <summary>
		/// Indicates whether the input is assigned
		/// </summary>
		public bool Assigned
		{
			get { return _type != InputType.Unassigned; }
		}

		/// <summary>
		/// Creates a new input descriptor
		/// </summary>
		/// <param name="type">Type of input</param>
		/// <param name="value">Value (button/key) associated with the input</param>
		public InputDescriptor (InputType type, int value)
		{
			_type  = type;
			_value = value;
		}

		/// <summary>
		/// Compares two input descriptors to check if they're equal
		/// </summary>
		/// <param name="a">First input descriptor</param>
		/// <param name="b">Second input descriptor</param>
		/// <returns>True if <paramref name="a"/> and <paramref name="b"/> are equal</returns>
		public static bool operator == (InputDescriptor a, InputDescriptor b)
		{
			return ((a._type == b._type) && (a._value == b._value)) ||
					(a._type == InputType.Unassigned && b._type == InputType.Unassigned);
		}

		/// <summary>
		/// Compares two input descriptors to check if they're not equal
		/// </summary>
		/// <param name="a">First input descriptor</param>
		/// <param name="b">Second input descriptor</param>
		/// <returns>True if <paramref name="a"/> and <paramref name="b"/> are not equal</returns>
		public static bool operator != (InputDescriptor a, InputDescriptor b)
		{
			return ((a._type != b._type) || (a._value != b._value)) &&
					(a._type != InputType.Unassigned || b._type != InputType.Unassigned);
		}

		/// <summary>
		/// Checks if an another <see cref="InputDescriptor"/> is equal to the current instance
		/// </summary>
		/// <param name="other">Other <see cref="InputDescriptor"/> to compare against</param>
		/// <returns>True if <paramref name="other"/> is effectively the same</returns>
		public bool Equals (InputDescriptor other)
		{
			return ((_type == other._type) && (_value == other._value)) ||
					(_type == InputType.Unassigned && other._type == InputType.Unassigned);
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
			return (_type == InputType.Unassigned) ? -1 : unchecked(((int)_type * 397) ^ _value);
		}

		/// <summary>
		/// Generates the string representation of the input descriptor
		/// </summary>
		/// <returns>A string in the form: TYPE: VALUE
		/// or "Unassigned" or "Unknown"</returns>
		public override string ToString ()
		{
			string value;
			switch(_type)
			{
			case InputType.Keyboard:
				value = ((Key)_value).ToString();
				break;
			case InputType.Mouse:
				value = ((MouseButton)_value).ToString();
				break;
			case InputType.Unassigned:
				return InputType.Unassigned.ToString();
			default:
				return "Unknown";
			}

			var type = _type.ToString();
			return String.Format("{0}: {1}", type, value);
		}
	}
}
