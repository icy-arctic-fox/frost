using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Live text token that symbolizes a formatting code (start of a formatted segment)
	/// with associated text (a string that follows that will be formatted)
	/// </summary>
	internal class LiveTextStartFormatToken : LiveTextToken
	{
		private readonly string _formatName, _extra;

		/// <summary>
		/// Name of the type of formatting that will be applied
		/// </summary>
		public string FormatName
		{
			get { return _formatName; }
		}

		/// <summary>
		/// Additional information used in formatting
		/// </summary>
		/// <remarks>This property can be null</remarks>
		public string ExtraInfo
		{
			get { return _extra; }
		}

		/// <summary>
		/// Creates a start formatting token
		/// </summary>
		/// <param name="token">Full string of the token</param>
		/// <param name="formatName">Name of the formatting</param>
		/// <param name="extra">Optional extra information used in formatting</param>
		/// <exception cref="ArgumentNullException">The full string (<paramref name="token"/>) and <paramref name="formatName"/> can't be null.</exception>
		public LiveTextStartFormatToken (string token, string formatName, string extra)
			: base(token)
		{
			if(formatName == null)
				throw new ArgumentNullException("formatName");

			_formatName = formatName;
			_extra      = extra;
		}
	}
}
