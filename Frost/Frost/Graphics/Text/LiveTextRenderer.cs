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
		/// <param name="appearance">Initial (default) visual appearance of the text</param>
		/// <exception cref="ArgumentNullException">The <paramref name="appearance"/> of the text can't be null.</exception>
		public LiveTextRenderer (TextAppearance appearance)
			: base(appearance)
		{
			// ...
		}

		/// <summary>
		/// Creates a new live text renderer
		/// </summary>
		/// <param name="text">Text to render</param>
		/// <param name="appearance">Initial (default) visual appearance of the text</param>
		/// <exception cref="ArgumentNullException">The <paramref name="appearance"/> of the text can't be null.</exception>
		public LiveTextRenderer (LiveTextString text, TextAppearance appearance)
			: base(appearance)
		{
			Text = text;
		}

		/// <summary>
		/// Gets a collection of segments which have text segments with newlines segments broken apart
		/// </summary>
		/// <param name="originalSegments">Original collection of segments</param>
		/// <returns>New collection of segments</returns>
		private static IEnumerable<ILiveTextSegment> getMultiLineSegments (IEnumerable<ILiveTextSegment> originalSegments)
		{
			var segments = new List<ILiveTextSegment>();
			foreach(var segment in originalSegments)
			{
				var stringSegment = segment as LiveTextStringSegment;
				if(stringSegment != null)
				{// This is a segment that can have newlines
					var text = SplitTextOnLinebreaks(stringSegment.Text);
					segments.AddRange(text.Select(line => new LiveTextStringSegment(line, stringSegment.Appearance)));
				}
				else // Segment that probably can't have newlines
					segments.Add(segment);
			}
			return segments;
		}

		/// <summary>
		/// Gets a collection of segments which have text segments stripped of newline characters
		/// </summary>
		/// <param name="originalSegments">Original collection of segments</param>
		/// <returns>New collection of segments</returns>
		private static IEnumerable<ILiveTextSegment> getStrippedSegments (IEnumerable<ILiveTextSegment> originalSegments)
		{
			var segments = new List<ILiveTextSegment>();
			foreach(var segment in originalSegments)
			{
				var stringSegment = segment as LiveTextStringSegment;
				if(stringSegment != null)
				{// This is a segment that can have newlines
					var text = stringSegment.Text.Replace('\r', ' ').Replace('\n', ' ');
					segments.Add(new LiveTextStringSegment(text, stringSegment.Appearance));
				}
				else // Segment that probably can't have newlines
					segments.Add(segment);
			}
			return segments;
		}

		/// <summary>
		/// Retrieves a list of segments that have been corrected to account for null and multi-line
		/// </summary>
		/// <returns>List of segments</returns>
		private IEnumerable<ILiveTextSegment> getSegments ()
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
		/// <param name="liveText">Text to calculate the bounds of</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateBounds (IEnumerable<ILiveTextSegment> liveText)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calculates the bounds of some text that has word wrapping applied to it
		/// </summary>
		/// <param name="liveText">Text to calculate the bounds of</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateWrappedBounds (IEnumerable<ILiveTextSegment> liveText, int width)
		{
			throw new NotImplementedException();
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
		/// <param name="liveText">Text to render</param>
		private static void drawText (RenderTarget target, IEnumerable<ILiveTextSegment> liveText)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the text and applies word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="liveText">Text to render</param>
		/// <param name="width">Target width to wrap lines by</param>
		private static void drawWrappedText (RenderTarget target, IEnumerable<ILiveTextSegment> liveText, int width)
		{
			throw new NotImplementedException();
		}
	}
}
