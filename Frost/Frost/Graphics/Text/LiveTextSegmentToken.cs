using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// A standalone part of live text that does not apply formatting to text
	/// </summary>
	internal class LiveTextSegmentToken : LiveTextToken
	{
		private readonly string _info;

		/// <summary>
		/// Textual segment information
		/// </summary>
		public string Info
		{
			get { return _info; }
		}

		/// <summary>
		/// Creates a live text segment token
		/// </summary>
		/// <param name="token">String representation of the entire token</param>
		/// <param name="info">Information about the segment (string contained between [ and ])</param>
		/// <exception cref="ArgumentNullException">The full string (<paramref name="token"/>) and <paramref name="info"/> can't be null.</exception>
		public LiveTextSegmentToken (string token, string info)
			: base(token)
		{
			if(info == null)
				throw new ArgumentNullException("info");

			_info = info;
		}
	}
}
