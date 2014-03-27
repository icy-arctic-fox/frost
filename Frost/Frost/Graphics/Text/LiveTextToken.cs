using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// A piece from a live text string
	/// </summary>
	internal class LiveTextToken
	{
		private readonly string _token;

		/// <summary>
		/// Creates a live text token
		/// </summary>
		/// <param name="token">String representation of the entire token</param>
		/// <exception cref="ArgumentNullException">The <paramref name="token"/> of the token can't be null.</exception>
		public LiveTextToken (string token)
		{
			if(token == null)
				throw new ArgumentNullException("token");
			_token = token;
		}

		/// <summary>
		/// Textual representation of the token
		/// </summary>
		/// <returns>Token's string value</returns>
		public override string ToString ()
		{
			return _token;
		}
	}
}
