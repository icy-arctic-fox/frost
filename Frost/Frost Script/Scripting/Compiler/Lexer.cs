using System;
using System.IO;
using System.Text;
using Frost.IO;
using Frost.Utility;

namespace Frost.Scripting.Compiler
{
	/// <summary>
	/// Pulls tokens from a stream of characters
	/// </summary>
	public class Lexer : IFullDisposable
	{
		private const char NullChar = '\0';

		private readonly PushbackStream _pushback;
		private readonly BinaryReader _br;
		private uint _line, _char;
		private readonly StringBuilder _lexeme = new StringBuilder();

		/// <summary>
		/// Creates a new token lexer
		/// </summary>
		/// <param name="s">Stream to read characters from</param>
		/// <exception cref="ArgumentNullException">The stream <paramref name="s"/> used to read characters from can't be null.</exception>
		public Lexer (Stream s)
		{
			if(s == null)
				throw new ArgumentNullException("s");

			_pushback = new PushbackStream(s);
			_br       = new BinaryReader(_pushback);
			_line     = 1;
			_char     = 0; // Initially 0 because each read increments by 1
		}

		/// <summary>
		/// Gets the next token from the stream
		/// </summary>
		/// <returns>Next token or null if there aren't any token remaining</returns>
		/// <exception cref="ParserException">Thrown if an unexpected character was encountered</exception>
		public Token GetNext ()
		{
			while(!EndOfStream)
			{// A while-loop is used here to continue reading when a comment is encountered
				resetLexeme();
				char c;
				if(skipWhitespace(out c))
					return initialState(c);
			}

			return null; // No tokens left
		}

		#region Lexer states

		/// <summary>
		/// Starting state for all tokens
		/// </summary>
		/// <param name="c">First character</param>
		/// <returns>A token</returns>
		private Token initialState (char c)
		{
			if(Char.IsDigit(c))
				return digitState(c);
			if(c == '-')
				return dashState();

			// else - Character doesn't match anything known of
			error(String.Format("Unrecognized character '{0}'", c));
			return null; // Should never get here
		}

		/// <summary>
		/// State when the character parsed is a digit
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		private Token digitState (char c)
		{
			return (c == '0') ? digit0State() : numberState();
		}

		/// <summary>
		/// State when parsing a number that is likely in a format that isn't base 10
		/// </summary>
		/// <returns>A numerical token</returns>
		private Token digit0State ()
		{
			char t; // Get the next char, which indicates the type
			if(getNextChar(out t))
			{// Check what the character is
				var b = IntegerToken.Base.Decimal;
				var value = 0;
				switch(t)
				{
				case 'x':
					if(Lexeme[0] == '-')
						return goBackNegativeNonBase10(t);
					b = IntegerToken.Base.Hexadecimal;
					value = hexadecimalIntegerState();
					break;
				case 'b':
					if(Lexeme[0] == '-')
						return goBackNegativeNonBase10(t);
					b = IntegerToken.Base.Binary;
					value = binaryIntegerState();
					break;
				case '.':
					pushback(t);
					return numberState();
				default:
					if(Char.IsDigit(t))
					{// Octal
						if(Lexeme[0] == '-')
							return goBackNegativeNonBase10(t);
						pushback(t);
						b = IntegerToken.Base.Octal;
						value = octalIntegerState();
					}
					else // Unknown
						error(String.Format("Unexpected character '{0}' found. Expected numerical literal.", t));
					break;
				}

				return new IntegerToken(value, b, _line, TokenStartPosition);
			}
			
			// End of stream, just a 0 by itself
			return new IntegerToken(0, IntegerToken.Base.Decimal, _line, TokenStartPosition);
		}

		/// <summary>
		/// Goes back in the stream before a non-decimal (base-10) number was encountered
		/// </summary>
		/// <param name="t">Non-decimal indicator character (b, x)</param>
		/// <returns>A dash token</returns>
		/// <remarks>Any number not in base-10 can't be negative, so return a dash token and then a positive number</remarks>
		private Token goBackNegativeNonBase10 (char t)
		{
			pushback(t);
			pushback('0');
			return new Token(TokenTag.Subtract, _line, _char);
		}

		/// <summary>
		/// State when parsing an integer in hexadecimal
		/// </summary>
		/// <returns>Parsed integer value</returns>
		private int hexadecimalIntegerState ()
		{
			var count = 0;
			char d;
			while(getNextChar(out d))
			{// Getting an end of stream is fine, that signifies that the number completed
				if(!Char.IsDigit(d) && (d < 'a' || d > 'f') && (d < 'A' || d > 'F'))
				{// End of numerical value
					if(count <= 0) // Unexpectedly reached the end
						error(String.Format("Expected 0-9, a-f, or A-F digit in hexadecimal numerical literal, but got '{0}'", d));
					else // Reached the end, push the character back
						pushback(d);
					break;
				}
				++count;
			}

			if(count <= 0) // Unexpected end of stream
				error("Expected 0-9, a-f, or A-F digit in hexadecimal numerical literal, but end of file reached");

			// Valid hexadecimal value
			const IntegerToken.Base b = IntegerToken.Base.Hexadecimal;
			var lexeme = Lexeme.Substring(2); // Remove 0x prefix
			var value = 0;
			try
			{
				value = Convert.ToInt32(lexeme, (int)b);
			}
			catch(OverflowException e)
			{
				_char = TokenStartPosition;
				error("Integer value too large (overflow)", e);
			}
			return value;
		}

		/// <summary>
		/// State when parsing an integer in binary
		/// </summary>
		/// <returns>Parsed integer value</returns>
		private int binaryIntegerState ()
		{
			var count = 0;
			char d;
			while(getNextChar(out d))
			{// Getting an end of stream is fine, that signifies that the number completed
				if(Char.IsDigit(d))
				{
					if(d != '0' && d != '1')
						error(String.Format("Expected 0 or 1 digit in binary numerical literal, but got '{0}'", d));
				}
				else
				{// End of numerical value
					if(count <= 0) // Unexpected reached the end
						error(String.Format("Expected 0 or 1 digit in binary numerical literal, but got '{0}'", d));
					else // Reached the end, push the character back
						pushback(d);
					break;
				}
				++count;
			}

			if(count <= 0) // Unexpected end of stream
				error("Expected 0 or 1 digit in binary numerical literal, but end of file reached");

			// Valid binary value
			const IntegerToken.Base b = IntegerToken.Base.Binary;
			var lexeme = Lexeme.Substring(2); // Remove 0b prefix
			var value = 0;
			try
			{
				value = Convert.ToInt32(lexeme, (int)b);
			}
			catch(OverflowException e)
			{
				_char = TokenStartPosition;
				error("Integer value too large (overflow)", e);
			}
			return value;
		}

		/// <summary>
		/// State when parsing an integer in octal
		/// </summary>
		/// <returns>Parsed integer value</returns>
		private int octalIntegerState ()
		{
			char d;
			while(getNextChar(out d))
			{// Getting an end of stream is fine, that signifies that the number completed
				if(Char.IsDigit(d))
				{
					if(d < '0' || d > '7')
						error(String.Format("Expected 0-7 digit in octal numerical literal, but got '{0}'", d));
				}
				else
				{// End of numerical value
					pushback(d);
					break;
				}
				// Octal digits don't need to be checked here because there's always at least one digit.
				// The digit was pushed back in the digit0State() method.
			}
			const IntegerToken.Base b = IntegerToken.Base.Octal;
			var lexeme = Lexeme.Substring(1); // Remove 0 prefix
			var value = 0;
			try
			{
				value = Convert.ToInt32(lexeme, (int)b);
			}
			catch(OverflowException e)
			{
				_char = TokenStartPosition;
				error("Integer value too large (overflow)", e);
			}
			return value;
		}

		/// <summary>
		/// State when parsing a base 10 numerical value (integer or decimal)
		/// </summary>
		/// <returns>A numerical token</returns>
		private Token numberState ()
		{
			var decimalFound = false;
			char d;
			while(getNextChar(out d))
			{// Getting an end of stream is fine, that signifies that the number completed
				if(d == '.')
				{// Found a decimal point
					if(decimalFound)
						error("Unexpected duplicate decimal point found in floating-point number");
					else
						decimalFound = true;
				}
				else if(!Char.IsDigit(d))
				{// End of numerical value
					pushback(d);
					break;
				}
			}

			if(decimalFound)
			{// Floating-point token found
				var value = Convert.ToSingle(Lexeme);
				return new FloatToken(value, _line, TokenStartPosition);
			}

			else
			{// Integer token found
				const IntegerToken.Base b = IntegerToken.Base.Decimal;
				var value = 0;
				try
				{
					value = Convert.ToInt32(Lexeme, (int)b);
				}
				catch(OverflowException e)
				{
					_char = TokenStartPosition;
					error("Integer value too large (overflow)", e);
				}
				return new IntegerToken(value, b, _line, TokenStartPosition);
			}
		}

		/// <summary>
		/// State when a hyphen (-) is read
		/// </summary>
		/// <returns>Some type of token</returns>
		private Token dashState ()
		{
			char c;
			if(getNextChar(out c))
			{
				if(Char.IsDigit(c)) // Start of a negative number
					return digitState(c);
				pushback(c);
			}

			// else - Just a hyphen
			return new Token(TokenTag.Subtract, _line, TokenStartPosition);
		}
		#endregion

		#region Lexer utilities

		/// <summary>
		/// Indicates if the end of the stream has been reached
		/// </summary>
		private bool EndOfStream
		{
			get { return _pushback.Position >= _pushback.Length; }
		}

		/// <summary>
		/// Pushes a character back onto the stream
		/// </summary>
		/// <param name="c">Character to push</param>
		private void pushback (char c)
		{
			var bytes = Encoding.UTF8.GetBytes(new[] {c});
			_pushback.Write(bytes, 0, bytes.Length);
			_lexeme.Remove(_lexeme.Length - 1, 1); // Remove last character from lexeme
			--_char;
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

		/// <summary>
		/// Position of where the current token (lexeme) started on the line
		/// </summary>
		private uint TokenStartPosition
		{
			get { return _char - (uint)_lexeme.Length + 1; }
		}

		/// <summary>
		/// Reads the next character in from the stream and advances the internal state
		/// </summary>
		/// <param name="c">Character read from the stream</param>
		/// <returns>True if a character was read or false if the end of the stream was reached</returns>
		private bool getNextChar (out char c)
		{
			if(!EndOfStream)
			{// There are still characters to read
				c = _br.ReadChar();
				++_char;
				_lexeme.Append(c);
				return true;
			}

			// End of stream reached
			c = NullChar;
			return false;
		}

		/// <summary>
		/// Skips over any number of whitespace characters at the current stream position
		/// </summary>
		/// <param name="c">First character following the whitespace</param>
		/// <returns>True if a non-whitespace character was read or false if the end of the stream was reached before finding one</returns>
		private bool skipWhitespace (out char c)
		{
			while(getNextChar(out c))
			{// Continue reading until the end of the stream is reached
				if(Char.IsWhiteSpace(c))
				{
					switch(c)
					{// Should the line number be advanced?
					case '\r':
						++_line;
						_char = 0;
						if(getNextChar(out c) && c != '\n')
						{// Skip \n to avoid \r\n being detected as two newlines
							--_char;
							pushback(c);
						}
						break;
					case '\n':
						++_line;
						_char = 0;
						break;
					}
				}
				else
				{// Non-whitespace character encountered
					resetLexeme();
					_lexeme.Append(c);
					return true;
				}
			}

			// End of stream reached
			return false;
		}

		/// <summary>
		/// Reads a character from the stream and throws an exception if it didn't match the expected character
		/// </summary>
		/// <param name="expected">Character that is expected to be next in the stream</param>
		/// <exception cref="ParserException">Thrown if the character read from the stream doesn't match <paramref name="expected"/>.
		/// This is also thrown if the end of the stream was encountered.</exception>
		private void expect (char expected)
		{
			char actual;
			if(getNextChar(out actual))
			{
				if(actual != expected)
					error(String.Format("Expected '{0}', but got '{1}'", expected, actual));
			}
			else
				error(String.Format("Unexpected end of file reached. Expected '{0}'", expected));
		}

		/// <summary>
		/// Reads a character from the stream and throws an exception if it isn't whitespace
		/// </summary>
		/// <remarks>The end of the stream is considered whitespace.</remarks>
		/// <exception cref="ParserException">Thrown if the character read from the stream isn't whitespace</exception>
		private void expectWhitespace ()
		{
			char c;
			if(getNextChar(out c) && !Char.IsWhiteSpace(c))
				error(String.Format("Unexpected character '{0}' encountered when whitespace was expected", c));
		}

		/// <summary>
		/// Throws a parser exception for the current position in the stream
		/// </summary>
		/// <param name="message">Informational message</param>
		/// <exception cref="ParserException">It's what this method does</exception>
		private void error (string message)
		{
			throw new ParserException(message, _line, _char);
		}

		/// <summary>
		/// Throws a parser exception for the current position in the stream
		/// </summary>
		/// <param name="message">Informational message</param>
		/// <param name="inner">Inner exception</param>
		/// <exception cref="ParserException">It's what this method does</exception>
		private void error (string message, Exception inner)
		{
			throw new ParserException(message, _line, _char, inner);
		}
		#endregion

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the lexer has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Triggered when the lexer is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Frees the resources held by the lexer
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Deconstructs the lexer
		/// </summary>
		~Lexer ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the lexer
		/// </summary>
		/// <param name="disposing">Flag indicating whether internal resources should be freed</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);

				if(disposing) // Dispose of the stream
					_br.Dispose();
			}
		}
		#endregion
	}
}
