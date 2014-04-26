using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Simple live text segment that contains just text
	/// </summary>
	public class LiveTextStringSegment : ILiveTextStringSegment
	{
		internal const string NullTextReprsentation = "nil";

		private readonly string _text;

		/// <summary>
		/// Displayed text
		/// </summary>
		public string Text
		{
			get { return _text; }
		}

		private readonly TextAppearance _appearance;

		/// <summary>
		/// Visual appearance of the text
		/// </summary>
		public TextAppearance Appearance
		{
			get { return _appearance; }
		}

		/// <summary>
		/// Creates the base of the live text segment
		/// </summary>
		/// <param name="text">Displayed text (null will be substituted with "nil")</param>
		/// <param name="appearance">Appearance of the text for the segment</param>
		/// <exception cref="ArgumentNullException">The <paramref name="appearance"/> of the text can't be null.</exception>
		public LiveTextStringSegment (string text, TextAppearance appearance)
		{
			if(appearance == null)
				throw new ArgumentNullException("appearance");

			_text = text ?? NullTextReprsentation;
			_appearance = appearance;
		}

		/// <summary>
		/// Indicates whether the segment can be broken into smaller segments
		/// </summary>
		/// <remarks>A false value for this property implies that <see cref="ILiveTextSegment.BreakSegmentApart"/>
		/// will throw a <see cref="NotSupportedException"/>, but it may just return a single segment equivalent to itself.</remarks>
		public bool IsSegmentBreakable
		{
			get { return _text.Any(Char.IsWhiteSpace); }
		}

		/// <summary>
		/// Breaks the segment into smaller segments.
		/// This is used when a segment extends outside the bounds and wrapping is attempted.
		/// </summary>
		/// <returns>Collection of smaller segments</returns>
		/// <exception cref="NotSupportedException">The segment does not support being broken apart.
		/// <see cref="ILiveTextSegment.IsSegmentBreakable"/> should be false in this instance.</exception>
		public IEnumerable<ILiveTextSegment> BreakSegmentApart ()
		{
			var words = breakTextIntoWords(_text);
			return words.Select(word => new LiveTextStringSegment(word, _appearance));
		}

		/// <summary>
		/// Breaks text apart into words
		/// </summary>
		/// <param name="text">Text to break apart</param>
		/// <returns>Collection of strings, each item is a word with trailing whitespace</returns>
		private static IEnumerable<string> breakTextIntoWords (string text)
		{
			var words = new List<string>();

			var whitespace = false;
			var start = 0;
			for(var i = 0; i < text.Length; ++i)
			{// Iterate over each character
				var c = text[i];
				if(Char.IsWhiteSpace(c))
					whitespace = true;

				else if(whitespace)
				{// Transition from whitespace to non-whitespace, break here
					var word = text.Substring(start, i - start);
					words.Add(word);

					start = i;
					whitespace = false;
				}
			}

			// Add final word
			var last = text.Substring(start);
			words.Add(last);

			return words;
		}

		/// <summary>
		/// Calculates the needed size of the live text segment
		/// </summary>
		/// <returns>Width (<see cref="Vector2f.X"/>) and height (<see cref="Vector2f.Y"/>) of the bounds needed</returns>
		public Vector2f CalculateSegmentBounds ()
		{
			using(var t = new SFML.Graphics.Text())
			{
				t.DisplayedString = _text;
				_appearance.ApplyTo(t);
				var bounds = t.GetLocalBounds();
				var width  = bounds.Width  + bounds.Left;
				var height = bounds.Height + bounds.Top;
				return new Vector2f(width, height);
			}
		}

		/// <summary>
		/// Draws the live text segment onto a texture
		/// </summary>
		/// <param name="target">Texture to draw the segment to</param>
		/// <param name="position">Position of the top-left corner of the segment</param>
		public void DrawSegment (RenderTexture target, Vector2f position)
		{
			using(var t = new SFML.Graphics.Text())
			{
				t.DisplayedString = _text;
				_appearance.ApplyTo(t);
				var rs = RenderStates.Default;
				rs.Transform.Translate(position);
				t.Draw(target, rs);
			}
		}

		/// <summary>
		/// Splits the segment into new segments on line breaks
		/// </summary>
		/// <returns>New segments</returns>
		public IEnumerable<ILiveTextStringSegment> SplitOnLineBreaks ()
		{
			var lines = _text.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
			return (from line in lines select new LiveTextStringSegment(line, _appearance));
		}

		/// <summary>
		/// Removes newline characters from the segment's text
		/// </summary>
		/// <returns>New segment without newline characters</returns>
		public ILiveTextStringSegment StripLineBreaks ()
		{
			var text = _text.Replace('\n', ' ').Replace('\r', ' ');
			return new LiveTextStringSegment(text, _appearance);
		}
	}
}
