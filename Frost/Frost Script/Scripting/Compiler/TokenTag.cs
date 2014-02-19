namespace Frost.Scripting.Compiler
{
	/// <summary>
	/// All valid token tags
	/// </summary>
	public enum TokenTag
	{
		#region Numerical

		/// <summary>
		/// Integer literal
		/// </summary>
		Integer,

		/// <summary>
		/// Floating-point number literal
		/// </summary>
		Float,

		/// <summary>
		/// Integer represented in hexadecimal
		/// </summary>
		Hex,

		/// <summary>
		/// Integer represented in binary
		/// </summary>
		Binary,
		#endregion

		#region Built-in Types

		/// <summary>
		/// Integer type
		/// </summary>
		IntType,

		/// <summary>
		/// Floating-point number type
		/// </summary>
		FloatType,

		/// <summary>
		/// String type
		/// </summary>
		StringType,
		#endregion

		#region Strings

		/// <summary>
		/// Variable, function, class, or other identifiable object
		/// </summary>
		Identifier,

		/// <summary>
		/// Single quoted string with literal meaning
		/// </summary>
		QuotedString,

		/// <summary>
		/// Double quoted string with evaluated meaning
		/// </summary>
		DoubleQuotedString,
		#endregion

		#region Keywords

		/// <summary>
		/// Definition keyword
		/// </summary>
		Def,

		/// <summary>
		/// Function keyword
		/// </summary>
		Function,

		/// <summary>
		/// Class keyword
		/// </summary>
		Class,

		/// <summary>
		/// Variable keyword
		/// </summary>
		Variable,

		/// <summary>
		/// New instance keyword
		/// </summary>
		New,

		/// <summary>
		/// If statement keyword
		/// </summary>
		If,

		/// <summary>
		/// Else statement keyword
		/// </summary>
		Else,

		/// <summary>
		/// While keyword
		/// </summary>
		While,
		#endregion

		#region Punctuation

		/// <summary>
		/// Opening parenthesis
		/// </summary>
		LeftParen,

		/// <summary>
		/// Closing parenthesis
		/// </summary>
		RightParen,

		/// <summary>
		/// Opening square brace
		/// </summary>
		LeftBrace,

		/// <summary>
		/// Closing square brace
		/// </summary>
		RightBrace,

		/// <summary>
		/// Opening curly brace
		/// </summary>
		LeftCurlyBrace,

		/// <summary>
		/// Closing curly brace
		/// </summary>
		RightCurlyBrace,

		/// <summary>
		/// Period punctuation mark
		/// </summary>
		Dot,

		/// <summary>
		/// Semicolon punctuation mark
		/// </summary>
		Semicolon,

		/// <summary>
		/// Colon punctuation mark
		/// </summary>
		Colon,

		/// <summary>
		/// Comma punctuation mark
		/// </summary>
		Comma,
		#endregion

		#region Operators

		/// <summary>
		/// Addition operator
		/// </summary>
		Add,

		/// <summary>
		/// Subtraction operator
		/// </summary>
		Subtract,

		/// <summary>
		/// Multiplication operator
		/// </summary>
		Multiply,

		/// <summary>
		/// Division operator
		/// </summary>
		Divide,

		/// <summary>
		/// Assignment (equals) operator
		/// </summary>
		Assignment,

		/// <summary>
		/// Less than operator
		/// </summary>
		LessThan,

		/// <summary>
		/// Greater than operator
		/// </summary>
		GreaterThan,

		/// <summary>
		/// Less than or equal to operator
		/// </summary>
		LessThanEqual,

		/// <summary>
		/// Greater than or equal to operator
		/// </summary>
		GreaterThanEqual,

		/// <summary>
		/// Equality (double equals) operator
		/// </summary>
		Equal
		#endregion
	}
}
