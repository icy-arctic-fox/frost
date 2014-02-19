using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is null</exception>
		public Lexer (Stream s)
		{
			if(s == null)
				throw new ArgumentNullException("s", "The stream to read characters from can't be null.");

			_pushback = new PushbackStream(s);
			_br       = new BinaryReader(_pushback);
			_line     = 1;
			_char     = 0; // Initially 0 because each read increments by 1
		}

		/// <summary>
		/// Gets the next token from the stream
		/// </summary>
		/// <returns>Next token or null if there aren't any token remaining</returns>
		public Token GetNext ()
		{
			throw new NotImplementedException();
		}

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
			var bytes = BitConverter.GetBytes(c);
			_pushback.Write(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Clears the contents of the lexeme
		/// </summary>
		private void restartLexeme ()
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
					restartLexeme();
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
		/// <exception cref="Exception">Thrown if the character read from the stream doesn't match <paramref name="expected"/>.
		/// This is also thrown if the end of the stream was encountered.</exception>
		private void expect (char expected)
		{
			char actual;
			if(getNextChar(out actual))
			{
				if(actual != expected)
					throw new ParserException(String.Format("Expected '{0}', but got '{1}'", expected, actual), _line, _char);
			}
			else
				throw new ParserException(String.Format("Unexpected end of file reached. Expected '{0}'", expected), _line, _char);
		}

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
