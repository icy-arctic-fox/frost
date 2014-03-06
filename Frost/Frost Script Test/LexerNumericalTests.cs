using System;
using Frost.Scripting.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Frost_Script_Test
{
	/// <summary>
	/// Tests the numerical constant functionality of <see cref="Lexer"/>
	/// </summary>
	[TestClass]
	public class LexerNumericalTests
	{
		/// <summary>
		/// Checks if the lexer gives a correct token for just the character 0
		/// </summary>
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
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

		#region Floating-point literal tests

		/// <summary>
		/// Checks that the lexer handles a typical floating-point number
		/// </summary>
		[TestMethod, TestCategory("Lexer")]
		public void FloatTest ()
		{
			const float expected = 123.456f;
			var lexer = LexerTestUtility.SetupLexer("123.456");
			var token = lexer.GetNext();
			LexerTestUtility.AssertFloatToken(token, 1, 1, expected);
		}

		/// <summary>
		/// Checks that the lexer handles a floating-point number with a value of 0
		/// </summary>
		[TestMethod, TestCategory("Lexer")]
		public void FloatZeroTest ()
		{
			const float expected = 0f;
			var lexer = LexerTestUtility.SetupLexer("0.0");
			var token = lexer.GetNext();
			LexerTestUtility.AssertFloatToken(token, 1, 1, expected);
		}

		/// <summary>
		/// Checks that the lexer handles a floating-point number that ends with a dot
		/// </summary>
		[TestMethod, TestCategory("Lexer")]
		public void FloatEndingDotTest ()
		{
			const float expected = 4f;
			var lexer = LexerTestUtility.SetupLexer("4.");
			var token = lexer.GetNext();
			LexerTestUtility.AssertFloatToken(token, 1, 1, expected);
		}

		/// <summary>
		/// Checks that the lexer handles a negative floating-point number
		/// </summary>
		[TestMethod, TestCategory("Lexer")]
		public void NegativeFloatTest ()
		{
			const float expected = -1.23f;
			var lexer = LexerTestUtility.SetupLexer("-1.23");
			var token = lexer.GetNext();
			LexerTestUtility.AssertFloatToken(token, 1, 1, expected);
		}

		/// <summary>
		/// Checks that the lexer properly handles a floating-point number terminated by a symbol
		/// </summary>
		[TestMethod, TestCategory("Lexer")]
		public void FloatStopSymbolTest ()
		{
			const float expected = 567.89f;
			var lexer = LexerTestUtility.SetupLexer("567.89+");
			var token = lexer.GetNext();
			LexerTestUtility.AssertFloatToken(token, 1, 1, expected);
		}
		#endregion
	}
}
