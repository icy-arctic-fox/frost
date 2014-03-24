using System;
using System.Collections.Generic;
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
		/// Retrieves the text to render and corrects it based on rendering properties
		/// </summary>
		/// <returns>Fixed live text</returns>
		private LiveTextString getFixedText ()
		{
			if(Text != null)
			{
				var liveText = Text;
				if(!MultiLine)
				{// Strip newline characters
					var segments = new List<LiveTextSegment>();
					foreach(var segment in liveText)
					{
						var stringSegment = segment as StringSegment;
						if(stringSegment != null)
						{// Current segment is a string, strip newlines from it
							var text = stringSegment.Value;
							text = text.Replace('\r', ' ');
							text = text.Replace('\n', ' ');
							segments.Add(new StringSegment(text));
						}
						else // Not a string, just append it
							segments.Add(segment);
					}
					liveText = new LiveTextString(segments);
				}
				return liveText;
			}

			// Empty string
			return new LiveTextString();
		}

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
		/// Calculates the bounds of the space that the text will occupy
		/// </summary>
		/// <returns>Width and height of the bounds</returns>
		protected override Vector2u CalculateBounds ()
		{
			var liveText = Text ?? new LiveTextString();
			var appearance = Appearance.CloneTextAppearance();
			return WordWrap
						? calculateWrappedBounds(liveText, MultiLine, WrapWidth, appearance)
						: calculateBounds(liveText, MultiLine, appearance);
		}

		/// <summary>
		/// Calculates the bounds of some live text that does not wrap multiple lines
		/// </summary>
		/// <param name="liveText">Text to calculate the bounds of</param>
		/// <param name="multiLine">Flag indicating whether newlines are allowed</param>
		/// <param name="appearance">Information about the initial (default) appearance of the text</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateBounds (LiveTextString liveText, bool multiLine, TextAppearance appearance)
		{
			using(var t = new SFML.Graphics.Text())
			{
				// Prepare the text
				appearance.ApplyTo(t);

				float x = 0f, y = 0f, maxWidth = 0f, lineHeight = 0f;
				foreach(var segment in liveText)
				{
					// Get text and appearance changes from the segment
					var text = segment.Apply(ref appearance);
					appearance.ApplyTo(t);

					// Get the lines of text
					var lines = new List<string>();
					if(text != null)
					{
						if(multiLine) // Add each line
							lines.AddRange(SplitTextOnLinebreaks(text));
						else
						{// Strip newline characters and add one line
							text = text.Replace('\r', ' ');
							text = text.Replace('\n', ' ');
							lines.Add(text);
						}
					}

					// Calculate the bounds of each line
					foreach(var line in lines)
					{
						// Advance to the next line
						y += lineHeight; // lineHeight is 0f on first iteration,
						lineHeight = 0f; // these two lines do nothing on the first line

						// Set the text to calculate bounds of
						t.DisplayedString = line;

						// Compute the bounds
						var bounds = t.GetLocalBounds();
						var width  = bounds.Width  + bounds.Left;
						var height = bounds.Height + bounds.Top;

						// Update the position and max bounds
						x += width;
						if(x > maxWidth)
							maxWidth = x;
						if(height > lineHeight)
							lineHeight = height;
					}
				}

				var finalWidth  = (uint)Math.Ceiling(maxWidth);
				var finalHeight = (uint)Math.Ceiling(y + lineHeight);
				return new Vector2u(finalWidth, finalHeight);
			}
		}

		/// <summary>
		/// Calculates the bounds of some text that has word wrapping applied to it
		/// </summary>
		/// <param name="liveText">Text to calculate the bounds of</param>
		/// <param name="multiLine">Flag indicating whether the original newlines are allowed</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <param name="appearance">Information about the initial (default) appearance of the text</param>
		/// <returns>Width and height of the bounds</returns>
		private static Vector2u calculateWrappedBounds (LiveTextString liveText, bool multiLine, int width,
														TextAppearance appearance)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the text onto a texture
		/// </summary>
		/// <param name="target">Texture to render to</param>
		protected override void Draw (RenderTexture target)
		{
			var liveText = Text ?? new LiveTextString();
			var appearance = Appearance.CloneTextAppearance();

			if(WordWrap)
				drawWrappedText(target, liveText, MultiLine, WrapWidth, appearance);
			else
				drawText(target, liveText, MultiLine, appearance);
		}

		/// <summary>
		/// Draws the text without applying any word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="text">Text to render</param>
		/// <param name="multiLine">Flag indicating whether newlines are allowed</param>
		/// <param name="appearance">Information about the initial (default) appearance of the text</param>
		private static void drawText (RenderTarget target, LiveTextString text, bool multiLine, TextAppearance appearance)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the text and applies word wrapping to it
		/// </summary>
		/// <param name="target">Texture to draw the text onto</param>
		/// <param name="liveText">Text to render</param>
		/// <param name="multiLine">Flag indicating whether the original newlines are allowed</param>
		/// <param name="width">Target width to wrap lines by</param>
		/// <param name="appearance">Information about the initial (default) appearance of the text</param>
		private static void drawWrappedText (RenderTarget target, LiveTextString liveText, bool multiLine, int width,
											TextAppearance appearance)
		{
			throw new NotImplementedException();
		}
	}
}
