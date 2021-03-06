﻿using System.Collections.Generic;
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

		private readonly LinkedList<LinkedList<Word<T>>> _lines = new LinkedList<LinkedList<Word<T>>>();
		private LinkedList<Word<T>> _curLine = new LinkedList<Word<T>>();

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
		public Word<T>[][] Lines
		{
			get
			{
				// Convert _lines from LL<LL<W<T>>> to W<T>[][]
				var lines   = new Word<T>[LineCount][];
				var curLine = _lines.First;
				for(var i = 0; i < lines.Length && curLine != null; ++i)
				{
					var lineList = curLine.Value;
					var line     = new Word<T>[lineList.Count];
					var curWord  = lineList.First;
					for(var j = 0; j < line.Length && curWord != null; ++j)
					{
						line[j] = curWord.Value;
						curWord = curWord.Next;
					}
					lines[i] = line;
					curLine  = curLine.Next;
				}
				return lines;
			}
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
		/// <param name="segment">Segment to append to the text block</param>
		public void Append (T segment)
		{
			// Use the trimmed size for determining whether an overflow needs to occur
			int trimmedWidth, trimmedHeight;
			segment.GetTrimmedSize(out trimmedWidth, out trimmedHeight);

			// But use the real size when positioning the segment
			int width, height;
			segment.GetSize(out width, out height);

			if(_curWidth > 0 && _curWidth + trimmedWidth > _targetWidth)
			{// Move to the next line, out of space on the current one
				NextLine();
				_curHeight      = height;
				_bounds.Height += height;
			}

			else
			{// Stay on the same line, there's enough space for the word
				if(height > _curHeight)
				{// Extend the height of the bounds
					var diff = height - _curHeight;
					_bounds.Height += diff;
					_curHeight = height;

					// Shift the previous segments downward on the line
					var curNode = _curLine.First;
					while(curNode != null)
					{
						var w  = curNode.Value;
						var b  = w.Bounds;
						b.Top += diff;
						curNode.Value = new Word<T>(w.Value, b);
						curNode = curNode.Next;
					}
				}
			}

			// Calculate the position of the word
			var x = _curWidth;
			var y = _bounds.Height - height;

			// Add the word to the current line
			var bounds = new IntRect(x, y, width, height);
			var word   = new Word<T>(segment, bounds);
			_curLine.AddLast(word);

			// Extend the width of the line
			_curWidth += width;
			if(_curWidth > _bounds.Width)
				_bounds.Width = _curWidth;
		}

		/// <summary>
		/// Advances the word wrap to the next line
		/// </summary>
		public void NextLine ()
		{
			_curWidth  = 0;
			_curHeight = 0;
			_curLine   = new LinkedList<Word<T>>();
			_lines.AddLast(_curLine);
		}
	}
}
