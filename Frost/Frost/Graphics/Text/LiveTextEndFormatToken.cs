using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Represents the end of a formatting code sequence
	/// </summary>
	internal class LiveTextEndFormatToken : LiveTextToken
	{
		/// <summary>
		/// Creates a live text token
		/// </summary>
		/// <param name="token">String representation of the entire token</param>
		/// <exception cref="ArgumentNullException">The full string (<paramref name="token"/>) can't be null.</exception>
		public LiveTextEndFormatToken (string token)
			: base(token)
		{
			// ...
		}
	}
}
