using System;

namespace Frost.Scripting.Compiler
{
	/// <summary>
	/// A literal floating-point token with a value
	/// </summary>
	public class FloatToken : Token
	{
		private readonly float _value;

		/// <summary>
		/// Parsed floating-point value
		/// </summary>
		public float Value
		{
			get { return _value; }
		}

		/// <summary>
		/// Creates a new floating-point token
		/// </summary>
		/// <param name="value">Parsed floating-point value</param>
		/// <param name="line">Line number the token appeared on</param>
		/// <param name="pos">Position of the first character on the line where the token starts</param>
		public FloatToken (float value, uint line, uint pos)
			: base(TokenTag.Float, line, pos)
		{
			_value = value;
		}

		/// <summary>
		/// Creates a string representation of the token
		/// </summary>
		/// <returns>A string in the form:
		/// Line LINE(CHAR) TAG: VALUE</returns>
		public override string ToString ()
		{
			return String.Format("{0}: {1}", base.ToString(), _value);
		}
	}
}
