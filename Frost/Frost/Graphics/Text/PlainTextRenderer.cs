using System;
using Frost.Utility;
using SFML.Graphics;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Draws text onto the screen.
	/// Rendering time for text is reduced by drawing to an off-screen texture
	/// and then copying it to an on-screen texture.
	/// </summary>
	public abstract class PlainTextRenderer : TextRenderer
	{
		public string Text
		{
			get { return TextObject.DisplayedString; }
			set { TextObject.DisplayedString = value; }
		}
	}
}
