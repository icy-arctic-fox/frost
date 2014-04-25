﻿using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Simple live text segment that contains just text
	/// </summary>
	public class LiveTextStringSegment : ILiveTextStringSegment
	{
		private const string NullTextReprsentation = "nil";

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
			get { throw new NotImplementedException(); }
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calculates the needed size of the live text segment
		/// </summary>
		/// <returns>Width (<see cref="Vector2f.X"/>) and height (<see cref="Vector2f.Y"/>) of the bounds needed</returns>
		public Vector2f CalculateSegmentBounds ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws the live text segment onto a texture
		/// </summary>
		/// <param name="target">Texture to draw the segment to</param>
		/// <param name="position">Position of the top-left corner of the segment</param>
		public void DrawSegment (RenderTexture target, Vector2f position)
		{
			throw new NotImplementedException();
		}
	}
}
