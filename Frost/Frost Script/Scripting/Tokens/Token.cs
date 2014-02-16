using System;

namespace Frost.Scripting.Tokens
{
	/// <summary>
	/// Base class for all tokens.
	/// A token is one or more characters taken from a stream that symbolize something.
	/// </summary>
	public abstract class Token
	{
		private readonly string _value;

		/// <summary>
		/// String value of the token
		/// </summary>
		public string Value
		{
			get { return _value; }
		}

		private readonly uint _line, _char;

		/// <summary>
		/// Line number the token appeared on
		/// </summary>
		public uint Line
		{
			get { return _line; }
		}

		/// <summary>
		/// Position of the character on the line where the token starts
		/// </summary>
		public uint Character
		{
			get { return _char; }
		}

		/// <summary>
		/// Creates a new token
		/// </summary>
		/// <param name="value">String value of the token</param>
		/// <param name="line">Line number that the token appeared on</param>
		/// <param name="pos">Position of the character on the line where the token starts</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
		protected Token (string value, uint line, uint pos)
		{
			if(value == null)
				throw new ArgumentNullException("value", "The token's string value can't be null.");

			_value = value;
			_line  = line;
			_char  = pos;
		}

		/// <summary>
		/// Creates a string representation of the token
		/// </summary>
		/// <returns>The token as a string in the form:
		/// Line #:# Value</returns>
		public override string ToString ()
		{
			return String.Format("Line {0}:{1} {2}", _line, _char, _value);
		}
	}
}
