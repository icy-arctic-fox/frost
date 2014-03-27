using System;
using System.Text;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Extracts live text tokens and string from a live text string
	/// </summary>
	internal class LiveTextLexer
	{
		private readonly string _str;
		private volatile int _pos;
		private readonly StringBuilder _lexeme = new StringBuilder();

		/// <summary>
		/// Creates a new token lexer
		/// </summary>
		/// <param name="str">Live text string to pull tokens from</param>
		public LiveTextLexer (string str)
		{
			_str = str ?? String.Empty;
			_pos = 0;
		}

		/// <summary>
		/// Gets the next token from the stream
		/// </summary>
		/// <returns>Next token or null if there aren't any token remaining</returns>
		public object /* LiveTextToken */ GetNext ()
		{
			if(!EndOfString)
			{// There are characters remaining
				resetLexeme();
				var c = getNextChar();
				return initialState(c);
			}

			return null; // No tokens left
		}

		#region Lexer states

		/// <summary>
		/// Starting state for the lexer
		/// </summary>
		/// <param name="c">First character</param>
		/// <returns>A live text token</returns>
		private object /* LiveTextToken */ initialState (char c)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Lexer utilities

		/// <summary>
		/// Gets the next character from the string
		/// </summary>
		/// <returns>Next character from the string</returns>
		private char getNextChar ()
		{
			var c = _str[_pos++];
			_lexeme.Append(c);
			return c;
		}

		/// <summary>
		/// Indicates if the end of the string has been reached
		/// </summary>
		private bool EndOfString
		{
			get { return _pos >= _str.Length; }
		}

		/// <summary>
		/// Goes back a character in the string
		/// </summary>
		private void goBack ()
		{
			_lexeme.Remove(_lexeme.Length - 1, 1); // Remove last character from lexeme
			--_pos;
		}

		/// <summary>
		/// Clears the contents of the lexeme
		/// </summary>
		private void resetLexeme ()
		{
			_lexeme.Clear();
		}

		/// <summary>
		/// String for the current token
		/// </summary>
		private string Lexeme
		{
			get { return _lexeme.ToString(); }
		}
		#endregion
	}
}
