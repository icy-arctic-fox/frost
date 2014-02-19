using System;

namespace Frost.Scripting.Compiler
{
	/// <summary>
	/// An exception that occurs when parsing encounters a syntax error
	/// </summary>
	public class ParserException : Exception
	{
		private readonly uint _line, _char;

		/// <summary>
		/// Line number that the error occurred on
		/// </summary>
		public uint Line
		{
			get { return _line; }
		}

		/// <summary>
		/// Character position on the line where the error occurred
		/// </summary>
		public uint Character
		{
			get { return _char; }
		}

		/// <summary>
		/// Creates a new parser exception
		/// </summary>
		/// <param name="line">Line number that the error occurred on</param>
		/// <param name="pos">Character position on the line where the error occurred</param>
		public ParserException (uint line, uint pos)
		{
			_line = line;
			_char = pos;
		}

		/// <summary>
		/// Creates a new parser exception
		/// </summary>
		/// <param name="message">Reason for the parser error</param>
		/// <param name="line">Line number that the error occurred on</param>
		/// <param name="pos">Character position on the line where the error occurred</param>
		public ParserException (string message, uint line, uint pos)
			: base(message)
		{
			_line = line;
			_char = pos;
		}

		/// <summary>
		/// Creates a string representation of the error
		/// </summary>
		/// <returns>A string in the form:
		/// Line #(#): Message</returns>
		public override string ToString ()
		{
			return String.Format("Line {0}({1}): {2}", _line, _char, Message);
		}
	}
}
