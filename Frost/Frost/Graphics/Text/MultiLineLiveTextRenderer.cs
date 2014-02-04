﻿using T = SFML.Graphics.Text;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Renders live text on multiple lines
	/// </summary>
	public class MultiLineLiveTextRenderer : LiveTextRenderer
	{
		private readonly T _text = new T();

		/// <summary>
		/// Displayed text
		/// </summary>
		public override string Text
		{
			get { return _text.DisplayedString; }
			set { _text.DisplayedString = value; }
		}

		/// <summary>
		/// Prepares the text for drawing.
		/// This method renders the text internally so that it is ready to be quickly drawn.
		/// </summary>
		public override void Prepare ()
		{
			throw new System.NotImplementedException();
		}
	}
}
