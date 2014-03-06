using System.IO;
using System.Text;
using Frost.Scripting.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Frost_Script_Test
{
	/// <summary>
	/// Utility methods for testing <see cref="Lexer"/>
	/// </summary>
	static class LexerTestUtility
	{
		/// <summary>
		/// Creates a lexer and gives it a stream containing some text
		/// </summary>
		/// <param name="text">Text to give the lexer</param>
		/// <returns>A constructed lexer</returns>
		public static Lexer SetupLexer (string text)
		{
			var bytes = Encoding.UTF8.GetBytes(text);
			var ms = new MemoryStream(bytes);
			return new Lexer(ms);
		}

		/// <summary>
		/// Asserts that a token has a specific tag, line, and position
		/// </summary>
		/// <param name="token">Token to validate</param>
		/// <param name="tag">The tag that the token should have</param>
		/// <param name="line">Line number that the token should be on</param>
		/// <param name="pos">Character position that the token should start at</param>
		public static void AssertToken (Token token, TokenTag tag, uint line, uint pos)
		{
			Assert.IsNotNull(token);
			Assert.AreEqual(tag, token.Tag);
			Assert.AreEqual(line, token.Line);
			Assert.AreEqual(pos, token.Character);
		}

		/// <summary>
		/// Asserts that a token is an integer token and has a given value
		/// </summary>
		/// <param name="token">Token to validate</param>
		/// <param name="line">Line number that the token should be on</param>
		/// <param name="pos">Character position that the token should start at</param>
		/// <param name="value">Expected value of the token</param>
		/// <param name="b">Base that the integer was in</param>
		public static void AssertIntegerToken (Token token, uint line, uint pos, int value, IntegerToken.Base b)
		{
			Assert.IsInstanceOfType(token, typeof(IntegerToken));
			Assert.AreEqual(TokenTag.Integer, token.Tag);
			Assert.AreEqual(line, token.Line);
			Assert.AreEqual(pos, token.Character);
			Assert.AreEqual(value, ((IntegerToken)token).Value);
			Assert.AreEqual(b, ((IntegerToken)token).OriginalBase);
		}
	}
}
