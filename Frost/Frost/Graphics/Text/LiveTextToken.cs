using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// A piece from a live text string
	/// </summary>
	internal class LiveTextToken
	{
		private readonly string _value;

		/// <summary>
		/// Creates a live text token
		/// </summary>
		/// <param name="value">String value of the token</param>
		/// <exception cref="ArgumentNullException">The <paramref name="value"/> of the token can't be null.</exception>
		public LiveTextToken (string value)
		{
			if(value == null)
				throw new ArgumentNullException("value");
			_value = value;
		}

		/// <summary>
		/// Textual representation of the token
		/// </summary>
		/// <returns>Token's string value</returns>
		public override string ToString ()
		{
			return _value;
		}
	}
}
