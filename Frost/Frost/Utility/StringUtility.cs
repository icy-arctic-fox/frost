using System;
using System.Text;

namespace Frost.Utility
{
	/// <summary>
	/// Useful functionality for when working with strings
	/// </summary>
	public static class StringUtility
	{
		/// <summary>
		/// Counts the number of lines in a string
		/// </summary>
		/// <param name="value">String to count the lines in</param>
		/// <returns>Number of lines in the string</returns>
		/// <remarks>-1 is returned if <paramref name="value"/> is null.
		/// 0 is returned if <paramref name="value"/> is an empty string.</remarks>
		public static int CountLines (this string value)
		{
			if(value == null)
				return -1;
			if(value == String.Empty)
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
	}
}
