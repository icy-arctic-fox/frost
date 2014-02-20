﻿using System;
using System.IO;
using System.Text;
using Frost.Scripting.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Frost_Script_Test
{
	[TestClass]
	public class LexerTests
	{
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
		/// Checks if the lexer gives a correct token for just the character 0
		/// </summary>
		[TestMethod]
		public void ZeroTest ()
		{
			var lexer = setupLexer("0");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, 0);
		}

		/// <summary>
		/// Checks if the lexer gives a correct token for a single digit number
		/// </summary>
		[TestMethod]
		public void SingleDigitIntegerTest ()
		{
			var lexer = setupLexer("5");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, 5);
		}

		/// <summary>
		/// Checks if the lexer handles binary integers starting with 0
		/// </summary>
		[TestMethod]
		public void BinaryIntegerTest ()
		{
			const int value = 21845;
			var lexer = setupLexer("0b0101010101010101");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, value);
		}

		/// <summary>
		/// Checks if the lexer handles binary integers starting with 1
		/// </summary>
		[TestMethod]
		public void BinaryIntegerTest2 ()
		{
			const int value = 21845;
			var lexer = setupLexer("0b101010101010101");
			var token = lexer.GetNext();
			assertIntegerToken(token, 1, 1, value);
		}
	}
}