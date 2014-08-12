using System;
using System.Collections.Generic;
using System.Linq;
using Frost.Utility;
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
			var words = _text.SplitIntoWordsKeepWhitespace();
			return words.Select(word => new LiveTextStringSegment(word, _appearance));
		}

		/// <summary>
		/// Retrieves the bounds that the text object calculates
		/// </summary>
		/// <param name="t">Preconfigured text object</param>
		/// <param name="width">Computed width</param>
		/// <param name="height">Computed height</param>
		private void calculateTextBounds (SFML.Graphics.Text t, out float width, out float height)
		{
			var bounds = t.GetLocalBounds();
			width  = bounds.Width + bounds.Left;
			height = bounds.Height + bounds.Top;
		}

		/// <summary>
		/// Calculates the fully needed width and height
		/// </summary>
		/// <param name="t">Preconfigured text object</param>
		/// <param name="width">Computed width</param>
		/// <param name="height">Computed height</param>
		private void calculateFullBounds (SFML.Graphics.Text t, out float width, out float height)
		{
			float h;
			calculateTextBounds(t, out width, out h);

			// Calculate the line height
			var lineCount  = _text.CountLines();
			var lineHeight = _appearance.Font.SfmlFont.GetLineSpacing(_appearance.Size);
			var textHeight = lineHeight * lineCount;

			// Set the height to the larger of the two
			height = Math.Max(h, textHeight);
		}

		/// <summary>
		/// Calculates the needed size of the live text segment
		/// </summary>
		/// <returns>Width (<see cref="Vector2f.X"/>) and height (<see cref="Vector2f.Y"/>) of the bounds needed</returns>
		public Vector2f CalculateSegmentBounds ()
		{
			using(var t = new SFML.Graphics.Text())
			{
				// Set up the text object
				t.DisplayedString = _text;
				_appearance.ApplyTo(t);

				float width, height;
				calculateFullBounds(t, out width, out height);
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
				// Set up the text object
				t.DisplayedString = _text;
				_appearance.ApplyTo(t);

				// Get the bounds (needed for the background/edge fix)
				float width, height;
				calculateFullBounds(t, out width, out height);

				// Calculate the transform
				var rs = RenderStates.Default;
				rs.BlendMode = BlendMode.None;
				rs.Transform.Translate(position);

				// Draw the text
				t.Draw(target, rs);
			}
		}

		/// <summary>
		/// Splits the segment into new segments on line breaks
		/// </summary>
		/// <returns>New segments</returns>
		public IEnumerable<ILiveTextStringSegment> SplitOnLineBreaks ()
		{
			var lines = _text.SplitOnLinebreaks();
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
