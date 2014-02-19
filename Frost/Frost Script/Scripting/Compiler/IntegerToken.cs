using System;

namespace Frost.Scripting.Compiler
{
	/// <summary>
	/// A literal integer token with a value
	/// </summary>
	public class IntegerToken : Token
	{
		private readonly int _value;

		/// <summary>
		/// Parsed integer value
		/// </summary>
		public int Value
		{
			get { return _value; }
		}

		private readonly Base _base;

		/// <summary>
		/// Base of the original token
		/// </summary>
		public Base OriginalBase
		{
			get { return _base; }
		}

		/// <summary>
		/// Creates a new integer token
		/// </summary>
		/// <param name="value">Parsed integer value</param>
		/// <param name="b">Base of the original token</param>
		/// <param name="line">Line number the token appeared on</param>
		/// <param name="pos">Position of the first character on the line where the token starts</param>
		public IntegerToken (int value, Base b, uint line, uint pos)
			: base(TokenTag.Integer, line, pos)
		{
			_value = value;
			_base  = b;
		}

		/// <summary>
		/// Creates a string representation of the token
		/// </summary>
		/// <returns>A string in the form:
		/// Line LINE(CHAR) TAG: VALUE</returns>
		public override string ToString ()
		{
			return String.Format("{0}: {1}", base.ToString(), _value); // TODO: Display in original base
		}

		/// <summary>
		/// Different supported integer base formats
		/// </summary>
		public enum Base
		{
			/// <summary>
			/// Base 2
			/// </summary>
			Binary = 2,

			/// <summary>
			/// Base 8
			/// </summary>
			Octal = 8,

			/// <summary>
			/// Base 10
			/// </summary>
			Decimal = 10,

			/// <summary>
			/// Base 16
			/// </summary>
			Hexadecimal = 16
		}
	}
}
