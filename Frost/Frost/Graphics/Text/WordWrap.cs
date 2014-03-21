using System;
using System.Collections.Generic;
using SFML.Graphics;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Computes where line breaks should appear in a block of text
	/// </summary>
	internal class WordWrap<T>
	{
		private readonly int _width;
		private IntRect _bounds;

		/// <summary>
		/// Creates a new word wrap processor
		/// </summary>
		/// <param name="width">Width to wrap each line to</param>
		public WordWrap (int width)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Bounds that the wrapped text will occupy
		/// </summary>
		public IntRect Bounds
		{
			get { return _bounds; }
		}

		/// <summary>
		/// Lines of text after wrapping has been applied
		/// </summary>
		public IEnumerable<Word<T>[]> Lines
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Total number of lines of text after wrapping has been applied
		/// </summary>
		public int LineCount { get; private set; }

		/// <summary>
		/// Adds a unbreakable segment to the end of the text
		/// </summary>
		/// <param name="word">Segment to </param>
		public void Append (T word)
		{
			throw new NotImplementedException();
		}
	}
}
