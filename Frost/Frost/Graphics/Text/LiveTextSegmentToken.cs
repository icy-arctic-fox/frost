using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// A standalone part of live text that does not apply formatting to text
	/// </summary>
	internal class LiveTextSegmentToken : LiveTextToken
	{
		private readonly string _type, _info;

		/// <summary>
		/// Name of the type of segment that will be inserted
		/// </summary>
		public string Type
		{
			get { return _type; }
		}

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
		/// <param name="type">Name of the type of segment</param>
		/// <param name="info">Information about the segment (string contained between [ and ])</param>
		/// <exception cref="ArgumentNullException">The full string (<paramref name="token"/>), <paramref name="type"/>, and <paramref name="info"/> can't be null.</exception>
		public LiveTextSegmentToken (string token, string type, string info)
			: base(token)
		{
			if(type == null)
				throw new ArgumentNullException("type");
			if(info == null)
				throw new ArgumentNullException("info");

			_type = type;
			_info = info;
		}
	}
}
