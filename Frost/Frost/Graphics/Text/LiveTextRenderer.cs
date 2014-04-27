using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Draws live text onto the screen.
	/// Rendering time for text is reduced by drawing to an off-screen texture
	/// and then copying it to an on-screen texture.
	/// </summary>
	public class LiveTextRenderer : TextRenderer
	{
		/// <summary>
		/// Text to render
		/// </summary>
		public LiveTextString Text { get; set; }

		/// <summary>
		/// Creates a new live text renderer
		/// </summary>
		/// <param name="text">Text to render</param>
		public LiveTextRenderer (LiveTextString text)
		{
			Text = text;
		}

		/// <summary>
		/// Gets a collection of segments which have text segments with newlines segments broken apart
		/// </summary>
		/// <param name="originalSegments">Original collection of segments</param>
		/// <returns>Collection of lines of segments</returns>
		private static IEnumerable<IEnumerable<ILiveTextSegment>> getMultiLineSegments (IEnumerable<ILiveTextSegment> originalSegments)
		{
			var lines   = new List<IEnumerable<ILiveTextSegment>>();
			var curLine = new List<ILiveTextSegment>();

			foreach(var segment in originalSegments)
			{
				var stringSegment = segment as ILiveTextStringSegment;
				if(stringSegment != null)
				{// This is a segment that can have newlines
					var count    = 1;
					var segments = stringSegment.SplitOnLineBreaks();
					foreach(var lineSegment in segments)
					{// Add the first one to the current line and advance to the next line
						var empty = lineSegment.Text.Length <= 0;
						if(count > 1 || empty)
						{// Advance to next line
							lines.Add(curLine);
							curLine = new List<ILiveTextSegment>();
							if(!empty)
								++count;
						}
						if(!empty)
							curLine.Add(lineSegment);
					}
				}
				else // Segment that probably can't have newlines
					curLine.Add(segment);
			}

			if(curLine.Count > 0)
				lines.Add(curLine);
			return lines;
		}

		/// <summary>
		/// Gets a collection of segments which have text segments stripped of newline characters
		/// </summary>
		/// <param name="originalSegments">Original collection of segments</param>
		/// <returns>Collection of lines of segments</returns>
		private static IEnumerable<IEnumerable<ILiveTextSegment>> getStrippedSegments (IEnumerable<ILiveTextSegment> originalSegments)
		{
			var line = from segment in originalSegments
						let stringSegment = segment as ILiveTextStringSegment
						select stringSegment != null ? stringSegment.StripLineBreaks() : segment;
			var lines = new List<IEnumerable<ILiveTextSegment>>(1) {line};
			return lines;
		}

		/// <summary>
		/// Retrieves a list of segments that have been corrected to account for null and multi-line
		/// </summary>
		/// <returns>Collection of lines of segments</returns>
		private IEnumerable<IEnumerable<ILiveTextSegment>> getSegments ()
		{
			var segments = Text ?? (IEnumerable<ILiveTextSegment>)new List<ILiveTextSegment>(0);
			return MultiLine ? getMultiLineSegments(segments) : getStrippedSegments(segments);
		}

		/// <summary>
		/// Calculates the bounds of the space that the text will occupy
		/// </summary>
		/// <returns>Width and height of the bounds</returns>
		protected override Vector2u CalculateBounds ()
		{
			var segments = getSegments();
			return WordWrap
						? calculateWrappedBounds(segments, WrapWidth)
						: calculateBounds(segments);
		}

		/// <summary>
		/// Calculates the bounds of some live text that does not wrap onto new lines
		/// </summary>
		/// <param name="lines">Lines of live text segments to calculate the bounds of</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateBounds (IEnumerable<IEnumerable<ILiveTextSegment>> lines)
		{
			float width = 0f, height = 0f;
			foreach(var line in lines)
			{// Iterate through each line
				float lineWidth = 0f, lineHeight = 0f;
				foreach(var segment in line)
				{// Iterate through each segment on the line
					var bounds = segment.CalculateSegmentBounds();
					lineWidth += bounds.X; // Extend the line width
					if(bounds.Y > lineHeight) // If the segment is taller than the current line height,
						lineHeight = bounds.Y; // then extend the line height
				}
				height += lineHeight; // Extend the height of the bounds
				if(lineWidth > width) // If the line is wider than the current bounds,
					width = lineWidth; // then extend the width of the bounds
			}

			var finalWidth  = (uint)Math.Ceiling(width);
			var finalHeight = (uint)Math.Ceiling(height);
			return new Vector2u(finalWidth, finalHeight);
		}

		/// <summary>
		/// Calculates the bounds of some text that has word wrapping applied to it
		/// </summary>
		/// <param name="lines">Lines of live text segments to calculate the bounds of</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateWrappedBounds (IEnumerable<IEnumerable<ILiveTextSegment>> lines, int width)
		{
			var wordWrap = performWordWrap(lines, width);

			// Compute the bounds
			var bounds = wordWrap.Bounds;
			var textWidth   = bounds.Width  + bounds.Left;
			var textHeight  = bounds.Height + bounds.Top;
			var finalWidth  = textWidth  < 0 ? 0U : (uint)textWidth;
			var finalHeight = textHeight < 0 ? 0U : (uint)textHeight;

			return new Vector2u(finalWidth, finalHeight);
		}

		/// <summary>
		/// Draws the text onto a texture
		/// </summary>
		/// <param name="target">Texture to render to</param>
		protected override void Draw (RenderTexture target)
		{
			var segments = getSegments();
			if(WordWrap)
				drawWrappedText(target, segments, WrapWidth);
			else
				drawText(target, segments);
		}

		/// <summary>
		/// Draws the text without applying any word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="lines">Lines of live text segments to render</param>
		private static void drawText (RenderTexture target, IEnumerable<IEnumerable<ILiveTextSegment>> lines)
		{
			var y = 0f;
			foreach(var line in lines)
			{// Iterate through each line
				var x = 0f;
				foreach(var segment in line)
				{// Iterate through each segment on the line
					// Draw the segment
					var position = new Vector2f(x, y);
					segment.DrawSegment(target, position);

					// Calculate the bounds and advance the position
					var bounds = segment.CalculateSegmentBounds();
					x += bounds.X;
					y += bounds.Y;
				}
			}
		}

		/// <summary>
		/// Draws the text and applies word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="lines">Lines of live text segments to render</param>
		/// <param name="width">Target width to wrap lines by</param>
		private static void drawWrappedText (RenderTexture target, IEnumerable<IEnumerable<ILiveTextSegment>> lines, int width)
		{
			var wordWrap = performWordWrap(lines, width);

			// Draw each segment
			foreach(var line in wordWrap.Lines)
			{// Iterate through each line
				foreach(var ws in line)
				{// Iterate through each segment on the line
					// Get the information we need
					var segment  = ws.Value.Segment;
					var bounds   = ws.Bounds;
					var position = new Vector2f(bounds.Left, bounds.Top);

					// Draw the segment
					segment.DrawSegment(target, position);
				}
			}
		}

		/// <summary>
		/// Performs word wrapping on live text segments
		/// </summary>
		/// <param name="lines">Lines of live text segments to wrap</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <returns>Word wrapping information</returns>
		private static WordWrap<WrappedSegment> performWordWrap (IEnumerable<IEnumerable<ILiveTextSegment>> lines, int width)
		{
			// Perform word wrapping
			var wordWrap = new WordWrap<WrappedSegment>(width);
			foreach(var line in lines)
			{// Iterate through each line
				foreach(var segment in line)
				{// Iterate through each segment on the line
					if(segment.IsSegmentBreakable)
					{// Break the segment apart
						var smallerSegments = segment.BreakSegmentApart();
						foreach(var smallerSegment in smallerSegments)
							wordWrap.Append(new WrappedSegment(smallerSegment));
					}
					else // Unbreakable
						wordWrap.Append(new WrappedSegment(segment));
				}
				wordWrap.NextLine(); // Force new line
			}

			return wordWrap;
		}

		/// <summary>
		/// This class is used to calculate the size of each segment.
		/// Instances of this class are passed to <see cref="WordWrap{T}"/>.
		/// </summary>
		private class WrappedSegment : ITextSize
		{
			private readonly ILiveTextSegment _segment;

			/// <summary>
			/// Creates a new segment in a word wrapped text block
			/// </summary>
			/// <param name="segment">Segment to be wrapped</param>
			public WrappedSegment (ILiveTextSegment segment)
			{
				_segment = segment;
			}

			/// <summary>
			/// Wrapped segment
			/// </summary>
			public ILiveTextSegment Segment
			{
				get { return _segment; }
			}

			/// <summary>
			/// Computes the width and height of the segment
			/// </summary>
			/// <param name="width">Width</param>
			/// <param name="height">Height</param>
			public void GetSize (out int width, out int height)
			{
				computeBounds(out width, out height);
			}

			/// <summary>
			/// Computes the width and height of the word after trimming trailing whitespace
			/// </summary>
			/// <param name="width">Width</param>
			/// <param name="height">Height</param>
			public void GetTrimmedSize (out int width, out int height)
			{
				GetSize(out width, out height); // TODO: Trim whitespace from the segment
			}

			/// <summary>
			/// Actually performs the computation of the text bounds
			/// </summary>
			/// <param name="width">Width</param>
			/// <param name="height">Height</param>
			private void computeBounds (out int width, out int height)
			{
				var bounds = _segment.CalculateSegmentBounds();
				width  = (int)Math.Ceiling(bounds.X);
				height = (int)Math.Ceiling(bounds.Y);
			}
		}
	}
}
