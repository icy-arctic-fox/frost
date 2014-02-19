using System;

namespace Frost.Scripting.Compiler
{
	/// <summary>
	/// A token is one or more characters taken from a stream that symbolize something
	/// </summary>
	/// <remarks>Tokens that require additional information,
	/// such as a numerical value or lexeme string have a sub-class.</remarks>
	public class Token
	{
		private readonly TokenTag _tag;

		/// <summary>
		/// Type of token the lexer produced
		/// </summary>
		private TokenTag Tag
		{
			get { return _tag; }
		}

		/// <summary>
		/// Class of tokens this token belongs to
		/// </summary>
		public TokenCategory Category
		{
			get
			{
				switch(_tag)
				{
				case TokenTag.Integer:
				case TokenTag.Float:
				case TokenTag.Hex:
				case TokenTag.Binary:
					return TokenCategory.Numerical;
				case TokenTag.IntType:
				case TokenTag.FloatType:
				case TokenTag.StringType:
					return TokenCategory.Type;
				case TokenTag.QuotedString:
				case TokenTag.DoubleQuotedString:
					return TokenCategory.String;
				case TokenTag.Def:
				case TokenTag.Function:
				case TokenTag.Class:
				case TokenTag.Variable:
				case TokenTag.New:
				case TokenTag.If:
				case TokenTag.Else:
				case TokenTag.While:
					return TokenCategory.Keyword;
				case TokenTag.LeftParen:
				case TokenTag.RightParen:
				case TokenTag.LeftBrace:
				case TokenTag.RightBrace:
				case TokenTag.LeftCurlyBrace:
				case TokenTag.RightCurlyBrace:
				case TokenTag.Dot:
				case TokenTag.Semicolon:
				case TokenTag.Colon:
				case TokenTag.Comma:
					return TokenCategory.Punctuation;
				case TokenTag.Add:
				case TokenTag.Subtract:
				case TokenTag.Multiply:
				case TokenTag.Divide:
				case TokenTag.Assignment:
				case TokenTag.LessThan:
				case TokenTag.GreaterThan:
				case TokenTag.LessThanEqual:
				case TokenTag.GreaterThanEqual:
				case TokenTag.Equal:
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
		/// <param name="tag">Type of token the lexer produced</param>
		/// <param name="line">Line number that the token appeared on</param>
		/// <param name="pos">Position of the character on the line where the token starts</param>
		public Token (TokenTag tag, uint line, uint pos)
		{
			_tag  = tag;
			_line = line;
			_char = pos;
		}

		/// <summary>
		/// Creates a string representation of the token
		/// </summary>
		/// <returns>The token as a string in the form:
		/// Line #:# Value</returns>
		public override string ToString ()
		{
			return String.Format("Line {0}({1}) {2}", _line, _char, _tag);
		}
	}
}
