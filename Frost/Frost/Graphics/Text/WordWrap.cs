using System;
using System.Collections.Generic;
using SFML.Graphics;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Computes where line breaks should appear in a block of text.
	/// The bounds of each segment/word are updated after each call to <see cref="Append"/>.
	/// </summary>
	internal class WordWrap<T> where T : ITextSize
	{
		private readonly int _targetWidth;
		private int _curWidth, _curHeight; // Sizes of the current line
		private IntRect _bounds;

		private readonly LinkedList<LinkedList<T>> _lines = new LinkedList<LinkedList<T>>();
		private LinkedList<T> _curLine = new LinkedList<T>();

		// TODO: Add computation for horizontal alignment (left, center, right)

		/// <summary>
		/// Creates a new word wrap processor
		/// </summary>
		/// <param name="width">Width to wrap each line to</param>
		public WordWrap (int width)
		{
			_targetWidth = width;
			_lines.AddLast(_curLine);
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
		public int LineCount
		{
			get
			{
				var lines = _lines.Count;
				if(_lines.Last.Value.Count <= 0)
					--lines; // Current line has nothing on it, don't count it
				return lines;
			}
		}

		/// <summary>
		/// Adds a unbreakable segment to the end of the text
		/// </summary>
		/// <param name="word">Segment to append to the text block</param>
		public void Append (T word)
		{
			var width  = word.GetWidth();
			var height = word.GetHeight();

			if(_curWidth > 0 && _curWidth + width > _targetWidth)
			{// Move to the next line, out of space on the current one
				_curWidth = 0;
				_curHeight = height;
				_curLine = new LinkedList<T>();
				_lines.AddLast(_curLine);
			}
			else
			{// Stay on the same line
				if(height > _curHeight)
				{// Extend the height of the bounds
					var diff = _curHeight - height;
					_bounds.Height += diff;
					_curHeight = height;
				}
			}

			// Add the word to the current line
			_curLine.AddLast(word);

			// Extend the width of the line
			_curWidth += width;
			if(_curWidth > _bounds.Width)
				_bounds.Width = _curWidth;
		}
	}
}
