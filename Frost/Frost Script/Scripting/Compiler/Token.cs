using System;

namespace Frost.Scripting.Compiler
{
	/// <summary>
	/// A token is one or more characters taken from a stream that symbolize something
	/// </summary>
	public class Token
	{
		private readonly string _value;

		/// <summary>
		/// String value of the token
		/// </summary>
		public string Value
		{
			get { return _value; }
		}

		private readonly TokenType _type;

		/// <summary>
		/// Type of token the lexer produced
		/// </summary>
		private TokenType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Class of tokens this token belongs to
		/// </summary>
		public TokenCategory Category
		{
			get
			{
				switch(_type)
				{
				case TokenType.Integer:
				case TokenType.Float:
				case TokenType.Hex:
				case TokenType.Binary:
					return TokenCategory.Numerical;
				case TokenType.IntType:
				case TokenType.FloatType:
				case TokenType.StringType:
					return TokenCategory.Type;
				case TokenType.QuotedString:
				case TokenType.DoubleQuotedString:
					return TokenCategory.String;
				case TokenType.Def:
				case TokenType.Function:
				case TokenType.Class:
				case TokenType.Variable:
				case TokenType.New:
				case TokenType.If:
				case TokenType.Else:
				case TokenType.While:
					return TokenCategory.Keyword;
				case TokenType.LeftParen:
				case TokenType.RightParen:
				case TokenType.LeftBrace:
				case TokenType.RightBrace:
				case TokenType.LeftCurlyBrace:
				case TokenType.RightCurlyBrace:
				case TokenType.Dot:
				case TokenType.Semicolon:
				case TokenType.Colon:
				case TokenType.Comma:
					return TokenCategory.Punctuation;
				case TokenType.Add:
				case TokenType.Subtract:
				case TokenType.Multiply:
				case TokenType.Divide:
				case TokenType.Assignment:
				case TokenType.LessThan:
				case TokenType.GreaterThan:
				case TokenType.LessThanEqual:
				case TokenType.GreaterThanEqual:
				case TokenType.Equal:
					return TokenCategory.Operator;
				default:
					return TokenCategory.Identifier;
				}
			}
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
		/// <param name="type">Type of token the lexer produced</param>
		/// <param name="line">Line number that the token appeared on</param>
		/// <param name="pos">Position of the character on the line where the token starts</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
		protected Token (string value, TokenType type, uint line, uint pos)
		{
			if(value == null)
				throw new ArgumentNullException("value", "The token's string value can't be null.");

			_value = value;
			_type  = type;
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
