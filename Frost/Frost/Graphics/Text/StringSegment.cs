using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Plain text segment
	/// </summary>
	public class StringSegment : LiveTextSegment
	{
		private readonly string _text;

		/// <summary>
		/// String value
		/// </summary>
		public string Value
		{
			get { return _text; }
		}

		/// <summary>
		/// Creates a new string segment in a live text string
		/// </summary>
		/// <param name="text">String value</param>
		/// <exception cref="ArgumentNullException">The <paramref name="text"/> can't be null.</exception>
		public StringSegment (string text)
		{
			if(text == null)
				throw new ArgumentNullException("text");

			_text = text;
		}

		/// <summary>
		/// Applies the segment to the renderer state
		/// </summary>
		/// <param name="appearance">Unused - no modifications are made to the appearance</param>
		/// <returns>Text in the segment</returns>
		public override string Apply (ref TextAppearance appearance)
		{
			return _text;
		}

		/// <summary>
		/// Retrieves the string value of the segment
		/// </summary>
		/// <returns>A string (same as <see cref="Value"/>)</returns>
		public override string ToString ()
		{
			return _text;
		}
	}
}
