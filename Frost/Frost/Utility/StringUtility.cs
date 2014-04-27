using System;

namespace Frost.Utility
{
	/// <summary>
	/// Useful functionality for when working with strings
	/// </summary>
	public static class StringUtility
	{
		/// <summary>
		/// Counts the number of words in a string
		/// </summary>
		/// <param name="value">String to count words in</param>
		/// <returns>Number of words in the string</returns>
		/// <remarks>A word is considered to be some characters surrounded by whitespace (or the start/end of the string).
		/// -1 will be returned if <paramref name="value"/> is null.</remarks>
		public static int CountWords (this string value)
		{
			if(value == null)
				return -1;
			var trimmed = value.Trim();

			var start = -1;
			var words = 0;
			var whitespace = false;
			for(var i = 0; i < trimmed.Length; ++i)
			{// Iterate over each character
				var c = trimmed[i];
				if(Char.IsWhiteSpace(c))
					whitespace = true;

				else if(whitespace)
				{// Transition from whitespace to non-whitespace, this is a word boundary
					++words;
					start = i;
					whitespace = false;
				}
			}

			if(start >= 0) // Add the last word, if there was one
				++words;

			return words;
		}

		/// <summary>
		/// Breaks a string apart by words
		/// </summary>
		/// <param name="value">String to break apart</param>
		/// <returns>Array of words</returns>
		/// <remarks>null will be returned if <paramref name="value"/> is null.</remarks>
		public static string[] SplitIntoWords (this string value)
		{
			if(value == null)
				return null;

			var count = CountWords(value);
			var words = new string[count];

			var start = 0;
			var whitespace = true;
			for(int i = 0, j = 0; i < value.Length && j < count; ++i)
			{
				var c = value[i];
				if(Char.IsWhiteSpace(c))
				{
					if(!whitespace)
					{// Start of whitespace
						var word   = value.Substring(start, i - start);
						words[j++] = word;
						whitespace = true;
					}
				}

				else if(whitespace)
				{// Transition from whitespace to non-whitespace, this is the start of a word
					start = i;
					whitespace = false;
				}
			}

			if(!whitespace)
			{// There's characters left at the end of the string
				var word = value.Substring(start);
				var j    = count - 1;
				words[j] = word;
			}

			return words;
		}

		/// <summary>
		/// Splits text apart into words.
		/// The text is split where it changes from whitespace to non-whitespace.
		/// This keeps whitespace at the end of the word.
		/// </summary>
		/// <param name="value">String to split</param>
		/// <returns>Array of words</returns>
		/// <remarks>null will be returned if <paramref name="value"/> is null.</remarks>
		public static string[] SplitIntoWordsKeepWhitespace (this string value)
		{
			if(value == null)
				return null;

			var count = CountWords(value);
			if(value.Length > 0 && Char.IsWhiteSpace(value[0]))
				++count; // value starts with whitespace, causing there to be an extra "word"
			var words = new string[count];

			var start      = 0;
			var whitespace = false;
			for(int i = 0, j = 0; i < value.Length && j < count; ++i)
			{// Iterate over each character
				var c = value[i];
				if(Char.IsWhiteSpace(c))
					whitespace = true;

				else if(whitespace)
				{// Transition from whitespace to non-whitespace, break here
					var word   = value.Substring(start, i - start);
					words[j++] = word;

					start      = i;
					whitespace = false;
				}
			}

			if(start > 0)
			{// Add final word
				var word = value.Substring(start);
				var j    = count - 1;
				words[j] = word;
			}

			return words;
		}

		/// <summary>
		/// Counts the number of lines in a string
		/// </summary>
		/// <param name="value">String to count lines in</param>
		/// <returns>Number of lines in the string</returns>
		/// <remarks>-1 is returned if <paramref name="value"/> is null.
		/// 0 is returned if <paramref name="value"/> is an empty string.</remarks>
		public static int CountLines (this string value)
		{
			if(value == null)
				return -1;
			if(value.Length <= 0)
				return 0;

			var lines = 1;
			var cr = false;
			for(var i = 0; i < value.Length; ++i)
			{
				var c = value[i];
				switch(c)
				{
				case '\r':
					cr = true;
					++lines;
					break;

				case '\n':
					if(!cr) // Increment if previous character wasn't \r
						++lines;
					cr = false;
					break;

				default:
					cr = false;
					break;
				}
			}

			return lines;
		}

		/// <summary>
		/// Splits text on newlines
		/// </summary>
		/// <param name="value">Text string to split</param>
		/// <returns>Array of lines of text</returns>
		/// <remarks>null will be returned if <paramref name="value"/> is null.</remarks>
		public static string[] SplitOnLinebreaks (this string value)
		{
			return value == null ? null : value.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
		}
	}
}
