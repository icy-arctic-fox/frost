using System;
using System.IO;
using System.Text;
using Frost.Scripting.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Frost_Script_Test
{
	/// <summary>
	/// Tests the <see cref="Lexer"/> functionality
	/// </summary>
	[TestClass]
	public class LexerTests
	{
		/// <summary>
		/// Checks if the constructor throws an exception when given a null stream
		/// </summary>
		[TestMethod]
		public void NullStreamTest ()
		{
			try
			{
				new Lexer(null);
			}
			catch(Exception e)
			{
				Assert.IsInstanceOfType(e, typeof(ArgumentNullException));
				var ane = (ArgumentNullException)e;
				Assert.AreEqual("s", ane.ParamName);
				return;
			}
			Assert.Fail("The constructor did not throw an exception.");
		}

		/// <summary>
		/// Checks if the lexer returns null for an empty string/stream
		/// </summary>
		[TestMethod]
		public void EmptyTest ()
		{
			var lexer = setupLexer(String.Empty);
			var token = lexer.GetNext();
			Assert.IsNull(token);
		}

		/// <summary>
		/// Checks if the lexer returns null for a string containing only whitespace
		/// </summary>
		[TestMethod]
		public void WhitespaceTest ()
		{
			var lexer = setupLexer(" \t\n\r\n ");
			var token = lexer.GetNext();
			Assert.IsNull(token);
		}

		#region Numeric literal tests

		/// <summary>
		/// Checks if the lexer gives a correct token for just the character 0
		/// </summary>
		[TestMethod]
		public void ZeroTest ()
		{
			const int expected = 0;
			var lexer = setupLexer("0");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, expected);
		}

		/// <summary>
		/// Checks if the lexer gives a correct token for a single digit number
		/// </summary>
		[TestMethod]
		public void SingleDigitIntegerTest ()
		{
			const int expected = 5;
			var lexer = setupLexer("5");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, expected);
		}

		#region Binary integer literal tests

		/// <summary>
		/// Checks if the lexer handles binary integers starting with 0
		/// </summary>
		[TestMethod]
		public void BinaryIntegerTest ()
		{
			const int expected = 21845;
			var lexer = setupLexer("0b0101010101010101");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, expected);
		}

		/// <summary>
		/// Checks if the lexer handles binary integers starting with 1
		/// </summary>
		[TestMethod]
		public void BinaryIntegerTest2 ()
		{
			const int expected = 21845;
			var lexer = setupLexer("0b101010101010101");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, expected);
		}

		/// <summary>
		/// Checks if the lexer handles a binary 0 value
		/// </summary>
		[TestMethod]
		public void Binary0IntegerTest ()
		{
			const int expected = 0;
			var lexer = setupLexer("0b0");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, expected);
		}

		/// <summary>
		/// Checks if the lexer properly handles maximum binary values
		/// </summary>
		[TestMethod]
		public void BigBinaryIntegerTest ()
		{
			const int expected = Int32.MaxValue;
			var lexer = setupLexer("0b01111111111111111111111111111111");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, expected);
		}

		/// <summary>
		/// Checks if the lexer properly handles minimum binary values
		/// </summary>
		[TestMethod]
		public void SmallBinaryIntegerTest ()
		{
			const int expected = Int32.MinValue;
			var lexer = setupLexer("0b10000000000000000000000000000000");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, expected);
		}

		/// <summary>
		/// Checks if the lexer properly complains about binary integers that are too large
		/// </summary>
		[TestMethod]
		public void OverflowBinaryIntegerTest ()
		{
			var lexer = setupLexer("0b1010101010101010101010101010101010101010101010101010101010101010");
			try
			{
				lexer.GetNext();
			}
			catch(Exception e)
			{
				Assert.IsInstanceOfType(e, typeof(ParserException));
				Assert.IsInstanceOfType(e.InnerException, typeof(OverflowException));
				var pe = (ParserException)e;
				Assert.AreEqual(1U, pe.Line);
				Assert.AreEqual(1U, pe.Character);
				return;
			}
			Assert.Fail("The lexer did not throw an exception.");
		}

		/// <summary>
		/// Checks if the lexer properly complains about invalid characters after the binary prefix (0b)
		/// </summary>
		[TestMethod]
		public void InvalidBinaryIntegerTest ()
		{
			var lexer = setupLexer("0babc");
			try
			{
				lexer.GetNext();
			}
			catch(Exception e)
			{
				Assert.IsInstanceOfType(e, typeof(ParserException));
				var pe = (ParserException)e;
				Assert.AreEqual(1U, pe.Line);
				Assert.AreEqual(3U, pe.Character);
				return;
			}
			Assert.Fail("The lexer did not throw an exception.");
		}

		/// <summary>
		/// Checks if the lexer properly complains about missing characters after the binary prefix (0b)
		/// </summary>
		[TestMethod]
		public void InvalidBinaryIntegerTest2 ()
		{
			var lexer = setupLexer("0b");
			try
			{
				lexer.GetNext();
			}
			catch(Exception e)
			{
				Assert.IsInstanceOfType(e, typeof(ParserException));
				var pe = (ParserException)e;
				Assert.AreEqual(1U, pe.Line);
				Assert.AreEqual(2U, pe.Character);
				return;
			}
			Assert.Fail("The lexer did not throw an exception.");
		}

		/// <summary>
		/// Checks that the lexer properly handles a binary integer terminated by a symbol
		/// </summary>
		[TestMethod]
		public void BinaryIntegerStopSymbol ()
		{
			const int expected = 10;
			var lexer = setupLexer("0b1010+");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, expected);
		}
		#endregion
		#endregion

		#region Utility methods

		/// <summary>
		/// Creates a lexer and gives it a stream containing some text
		/// </summary>
		/// <param name="text">Text to give the lexer</param>
		/// <returns>A constructed lexer</returns>
		private static Lexer setupLexer (string text)
		{
			var bytes = Encoding.UTF8.GetBytes(text);
			var ms = new MemoryStream(bytes);
			return new Lexer(ms);
		}

		/// <summary>
		/// Asserts that a token is an integer token and has a given value
		/// </summary>
		/// <param name="token">Token to validate</param>
		/// <param name="line">Line number the token should be on</param>
		/// <param name="pos">Character position the token should start at</param>
		/// <param name="value">Expected value of the token</param>
		private static void assertIntegerToken (Token token, uint line, uint pos, int value)
		{
			Assert.IsInstanceOfType(token, typeof(IntegerToken));
			Assert.AreEqual(line, token.Line);
			Assert.AreEqual(pos, token.Character);
			Assert.AreEqual(value, ((IntegerToken)token).Value);
		}
		#endregion
	}
}
