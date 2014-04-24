using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// A unit of live text
	/// </summary>
	public abstract class LiveTextSegment
	{
		/// <summary>
		/// Indicates whether the segment can be broken into smaller segments
		/// </summary>
		/// <remarks>A false value for this property implies that <see cref="BreakApart"/>
		/// will throw a <see cref="NotSupportedException"/>, but it may just return a single segment equivalent to itself.</remarks>
		public abstract bool Breakable { get; }

		/// <summary>
		/// Breaks the segment into smaller segments.
		/// This is used when a segment extends outside the bounds and wrapping is attempted.
		/// </summary>
		/// <returns>Collection of smaller segments</returns>
		/// <exception cref="NotSupportedException">The segment does not support being broken apart.
		/// <see cref="Breakable"/> should be false in this instance.</exception>
		public abstract IEnumerable<LiveTextSegment> BreakApart ();

		/// <summary>
		/// Calculates the needed size of the live text segment
		/// </summary>
		/// <returns>Width (<see cref="Vector2f.X"/>) and height (<see cref="Vector2f.Y"/>) of the bounds needed</returns>
		public abstract Vector2f CalculateBounds ();

		/// <summary>
		/// Draws the live text segment onto a texture
		/// </summary>
		/// <param name="target">Texture to draw the segment to</param>
		/// <param name="position">Position of the top-left corner of the segment</param>
		public abstract void Draw (RenderTexture target, Vector2f position);
	}
}
