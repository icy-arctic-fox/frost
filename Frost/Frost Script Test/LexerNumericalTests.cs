﻿using System;
using Frost.Scripting.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Frost_Script_Test
{
	class LexerNumericalTests
	{
		/// <summary>
		/// Checks if the lexer gives a correct token for just the character 0
		/// </summary>
		[TestMethod]
		public void ZeroTest ()
		{
			const int expected = 0;
			var lexer = LexerTestUtility.SetupLexer("0");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Decimal);
		}

		/// <summary>
		/// Checks if the lexer gives a correct token for a single digit number
		/// </summary>
		[TestMethod]
		public void SingleDigitIntegerTest ()
		{
			const int expected = 5;
			var lexer = LexerTestUtility.SetupLexer("5");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Decimal);
		}

		#region Binary integer literal tests

		/// <summary>
		/// Checks if the lexer handles binary integers starting with 0
		/// </summary>
		[TestMethod]
		public void BinaryIntegerLeading0Test ()
		{
			const int expected = 21845;
			var lexer = LexerTestUtility.SetupLexer("0b0101010101010101");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Binary);
		}

		/// <summary>
		/// Checks if the lexer handles binary integers starting with 1
		/// </summary>
		[TestMethod]
		public void BinaryIntegerTest ()
		{
			const int expected = 21845;
			var lexer = LexerTestUtility.SetupLexer("0b101010101010101");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Binary);
		}

		/// <summary>
		/// Checks if the lexer handles a binary 0 value
		/// </summary>
		[TestMethod]
		public void Binary0IntegerTest ()
		{
			const int expected = 0;
			var lexer = LexerTestUtility.SetupLexer("0b0");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Binary);
		}

		/// <summary>
		/// Checks if the lexer properly handles maximum binary values
		/// </summary>
		[TestMethod]
		public void MaxBinaryIntegerTest ()
		{
			const int expected = Int32.MaxValue;
			var lexer = LexerTestUtility.SetupLexer("0b01111111111111111111111111111111");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Binary);
		}

		/// <summary>
		/// Checks if the lexer properly handles minimum binary values
		/// </summary>
		[TestMethod]
		public void MinBinaryIntegerTest ()
		{
			const int expected = Int32.MinValue;
			var lexer = LexerTestUtility.SetupLexer("0b10000000000000000000000000000000");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Binary);
		}

		/// <summary>
		/// Checks if the lexer properly complains about binary integers that are too large
		/// </summary>
		[TestMethod]
		public void OverflowBinaryIntegerTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("0b1010101010101010101010101010101010101010101010101010101010101010");
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
		/// Checks if the lexer properly handles binary numbers with a negative prefix
		/// </summary>
		[TestMethod]
		public void NegativeBinaryIntegerTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("-0b111");
			var token = lexer.GetNext();
			LexerTestUtility.AssertToken(token, TokenTag.Subtract, 1, 1);

			token = lexer.GetNext();
			const int value = 7;
			LexerTestUtility.AssertIntegerToken(token, 1, 2, value, IntegerToken.Base.Binary);
		}

		/// <summary>
		/// Checks if the lexer properly complains about invalid characters after the binary prefix (0b)
		/// </summary>
		[TestMethod]
		public void InvalidBinaryIntegerDigitTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("0b2");
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
		/// Checks if the lexer properly complains about invalid characters after the binary prefix (0b)
		/// </summary>
		[TestMethod]
		public void InvalidBinaryIntegerCharTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("0babc");
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
		public void InvalidBinaryIntegerTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("0b");
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
		public void BinaryIntegerStopSymbolTest ()
		{
			const int expected = 10;
			var lexer = LexerTestUtility.SetupLexer("0b1010+");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Binary);
		}
		#endregion

		#region Octal integer literal tests

		/// <summary>
		/// Checks if the lexer handles octal integers starting with 0
		/// </summary>
		[TestMethod]
		public void OctalIntegerTest ()
		{
			const int expected = 5;
			var lexer = LexerTestUtility.SetupLexer("005");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Octal);
		}

		/// <summary>
		/// Checks if the lexer handles an octal 0 value
		/// </summary>
		[TestMethod]
		public void Octal0IntegerTest ()
		{
			const int expected = 0;
			var lexer = LexerTestUtility.SetupLexer("00");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Octal);
		}

		/// <summary>
		/// Checks if the lexer properly handles maximum octal values
		/// </summary>
		[TestMethod]
		public void MaxOctalIntegerTest ()
		{
			const int expected = Int32.MaxValue;
			var lexer = LexerTestUtility.SetupLexer("017777777777");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Octal);
		}

		/// <summary>
		/// Checks if the lexer properly handles minimum octal values
		/// </summary>
		[TestMethod]
		public void MinOctalIntegerTest ()
		{
			const int expected = Int32.MinValue;
			var lexer = LexerTestUtility.SetupLexer("020000000000");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Octal);
		}

		/// <summary>
		/// Checks if the lexer properly complains about octal integers that are too large
		/// </summary>
		[TestMethod]
		public void OverflowOctalIntegerTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("0123456701234567");
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
		/// Checks if the lexer properly handles octal numbers with a negative prefix
		/// </summary>
		[TestMethod]
		public void NegativeOctalIntegerTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("-0123");
			var token = lexer.GetNext();
			LexerTestUtility.AssertToken(token, TokenTag.Subtract, 1, 1);

			token = lexer.GetNext();
			const int value = 83;
			LexerTestUtility.AssertIntegerToken(token, 1, 2, value, IntegerToken.Base.Octal);
		}

		/// <summary>
		/// Checks if the lexer properly complains about invalid characters after the octal prefix (0)
		/// </summary>
		[TestMethod]
		public void InvalidOctalIntegerTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("0abc");
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
		/// Checks if the lexer properly complains about invalid numbers after the octal prefix (0)
		/// </summary>
		[TestMethod]
		public void InvalidOctalIntegerDigitTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("089");
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
		/// Checks that the lexer properly handles an octal integer terminated by a symbol
		/// </summary>
		[TestMethod]
		public void OctalIntegerStopSymbolTest ()
		{
			const int expected = 342391;
			var lexer = LexerTestUtility.SetupLexer("01234567+");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Octal);
		}
		#endregion

		#region Hexadecimal integer literal tests

		/// <summary>
		/// Checks if the lexer handles hexadecimal integers starting with 0
		/// </summary>
		[TestMethod]
		public void HexadecimalIntegerTest ()
		{
			const int expected = 5;
			var lexer = LexerTestUtility.SetupLexer("0x05");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Hexadecimal);
		}

		/// <summary>
		/// Checks if the lexer handles a hexadecimal 0 value
		/// </summary>
		[TestMethod]
		public void Hexadecimal0IntegerTest ()
		{
			const int expected = 0;
			var lexer = LexerTestUtility.SetupLexer("0x0");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Hexadecimal);
		}

		/// <summary>
		/// Checks if the lexer properly handles maximum hexadecimal values
		/// </summary>
		[TestMethod]
		public void MaxHexadecimalIntegerTest ()
		{
			const int expected = Int32.MaxValue;
			var lexer = LexerTestUtility.SetupLexer("0x7FFFFFFF");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Hexadecimal);
		}

		/// <summary>
		/// Checks if the lexer properly handles minimum hexadecimal values
		/// </summary>
		[TestMethod]
		public void MinHexadecimalIntegerTest ()
		{
			const int expected = Int32.MinValue;
			var lexer = LexerTestUtility.SetupLexer("0x80000000");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Hexadecimal);
		}

		/// <summary>
		/// Checks if the lexer properly complains about hexadecimal integers that are too large
		/// </summary>
		[TestMethod]
		public void OverflowHexadecimalIntegerTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("0xffffffffff");
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
		/// Checks if the lexer properly handles hexadecimal numbers with a negative prefix
		/// </summary>
		[TestMethod]
		public void NegativeHexadecimalIntegerTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("-0xff");
			var token = lexer.GetNext();
			LexerTestUtility.AssertToken(token, TokenTag.Subtract, 1, 1);

			token = lexer.GetNext();
			const int value = 0xff;
			LexerTestUtility.AssertIntegerToken(token, 1, 2, value, IntegerToken.Base.Hexadecimal);
		}

		/// <summary>
		/// Checks if the lexer properly complains about missing characters after the hexadecimal prefix (0x)
		/// </summary>
		[TestMethod]
		public void InvalidHexadecimalIntegerTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("0x");
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
		/// Checks if the lexer properly complains about invalid characters after the hexadecimal prefix (0x)
		/// </summary>
		[TestMethod]
		public void InvalidHexadecimalIntegerDigitTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("0xzzz");
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
		/// Checks that the lexer properly handles a hexadecimal integer terminated by a symbol
		/// </summary>
		[TestMethod]
		public void HexadecimalIntegerStopSymbolTest ()
		{
			const int expected = 0xaabbcc;
			var lexer = LexerTestUtility.SetupLexer("0xaabbcc+");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Hexadecimal);
		}

		/// <summary>
		/// Checks that the lexer properly handles a hexadecimal integer terminated by a symbol
		/// </summary>
		[TestMethod]
		public void HexadecimalMixedCaseTest ()
		{
			const int expected = 0x7abcde;
			var lexer = LexerTestUtility.SetupLexer("0x7AbCdE");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Hexadecimal);
		}
		#endregion

		#region Decimal integer literal tests

		/// <summary>
		/// Checks that the lexer properly handles a decimal integer with multiple digits
		/// </summary>
		[TestMethod]
		public void DecimalIntegerTest ()
		{
			const int expected = 1234567890;
			var lexer = LexerTestUtility.SetupLexer("1234567890");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Decimal);
		}

		/// <summary>
		/// Checks that the lexer properly handles a decimal integer terminated by a symbol
		/// </summary>
		[TestMethod]
		public void DecimalIntegerStopSymbolTest ()
		{
			const int expected = 555;
			var lexer = LexerTestUtility.SetupLexer("555+");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Decimal);
		}

		/// <summary>
		/// Checks if the lexer properly handles maximum decimal values
		/// </summary>
		[TestMethod]
		public void MaxDecimalIntegerTest ()
		{
			const int expected = Int32.MaxValue;
			var lexer = LexerTestUtility.SetupLexer("2147483647");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Decimal);
		}

		/// <summary>
		/// Checks if the lexer properly handles minimum decimal values
		/// </summary>
		[TestMethod]
		public void MinDecimalIntegerTest ()
		{
			const int expected = Int32.MinValue;
			var lexer = LexerTestUtility.SetupLexer("-2147483648");
			var token = lexer.GetNext();
			LexerTestUtility.AssertIntegerToken(token, 1, 1, expected, IntegerToken.Base.Decimal);
		}

		/// <summary>
		/// Checks if the lexer properly complains about decimal integers that are too large
		/// </summary>
		[TestMethod]
		public void OverflowDecimalIntegerTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("9999999999");
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
		/// Checks if the lexer properly complains about decimal integers that are too small
		/// </summary>
		[TestMethod]
		public void UnderflowDecimalIntegerTest ()
		{
			var lexer = LexerTestUtility.SetupLexer("-9999999999");
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
		#endregion
	}
}
