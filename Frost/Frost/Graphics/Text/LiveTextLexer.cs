﻿using System;
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
		public LiveTextToken GetNext ()
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
		private LiveTextToken initialState (char c)
		{
			switch(c)
			{
			case '\\':
				return escapeState();
			case '}':
				return endFormatState();
			default:
				return otherState();
			}
		}

		/// <summary>
		/// State when an escape character (\) is encountered
		/// </summary>
		/// <returns>A live text token</returns>
		private LiveTextToken escapeState ()
		{
			if(EndOfString) // Nothing after \
				return new LiveTextToken(Lexeme);
			
			var c = getNextChar();
			if(c == '\\')
			{// Escape slash \\
				goBack();
				++_pos; // Skip over second slash
				return otherState();
			}

			if(c == '}') // Escape end format \}
				return new LiveTextToken("}");

			// Start of formatting token
			var sb = new StringBuilder();
			while(!EndOfString)
			{ // Continue until a { is found
				c = getNextChar();
				switch(c)
				{
				case '{':
					return new LiveTextStartFormatToken(Lexeme, sb.ToString(), null);
				case '[':
					var extra = extraFormatInfoState();
					if(EndOfString || getNextChar() != '{')
						return endFormatState(); // Invalid formatter, transform into a string
					return new LiveTextStartFormatToken(Lexeme, sb.ToString(), extra);
				default:
					sb.Append(c);
					break;
				}
			}

			return otherState(); // A { was never found, not a valid formatter
		}

		/// <summary>
		/// State when a formatter has been found and extra information about formatting is being supplied.
		/// \foo[ &lt;-- HERE
		/// </summary>
		/// <returns>Additional information for formatting (text between [ and ])</returns>
		private string extraFormatInfoState ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// State when a format terminating character (}) is encountered
		/// </summary>
		/// <returns>A live text token</returns>
		private LiveTextToken endFormatState ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// State when any non-special character is encountered
		/// </summary>
		/// <returns>A live text token</returns>
		private LiveTextToken otherState ()
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
