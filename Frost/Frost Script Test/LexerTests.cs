using System;
using Frost.Scripting.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Frost_Script_Test
{
	/// <summary>
	/// Tests the basic functionality of <see cref="Lexer"/>
	/// </summary>
	[TestClass]
	public class LexerTests
	{
		/// <summary>
		/// Checks if the constructor throws an exception when given a null stream
		/// </summary>
		[TestMethod, TestCategory("Lexer")]
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
		[TestMethod, TestCategory("Lexer")]
		public void EmptyTest ()
		{
			var lexer = LexerTestUtility.SetupLexer(String.Empty);
			var token = lexer.GetNext();
			Assert.IsNull(token);
		}

		/// <summary>
		/// Checks if the lexer returns null for a string containing only whitespace
		/// </summary>
		[TestMethod, TestCategory("Lexer")]
		public void WhitespaceTest ()
		{
			var lexer = LexerTestUtility.SetupLexer(" \t\n\r\n ");
			var token = lexer.GetNext();
			Assert.IsNull(token);
		}
	}
}
