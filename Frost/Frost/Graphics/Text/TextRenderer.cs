using System;
using System.Collections.Generic;
using Frost.Utility;
using SFML.Graphics;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Renders text onto a texture
	/// </summary>
	public abstract class TextRenderer : IFullDisposable
	{
		/// <summary>
		/// Indicates whether the text is allowed to span multiple lines and can contain newlines
		/// </summary>
		/// <remarks>Newlines (\n) and carriage-returns (\r) will be ignored if this property is false.</remarks>
		public bool MultiLine { get; set; }

		/// <summary>
		/// Indicates whether words automatically wrap to the next line if the text becomes longer than a specified length
		/// </summary>
		public bool WordWrap { get; set; }

		/// <summary>
		/// Width of text (in pixels) to wrap each line by
		/// </summary>
		/// <remarks>There must be at least one word per line.
		/// It is possible to go over this length if the first word on a line has a pixel length greater than this value.</remarks>
		public int WrapWidth { get; set; }

		private readonly TextAppearance _appearance;

		/// <summary>
		/// Appearance of the rendered text
		/// </summary>
		public TextAppearance Appearance
		{
			get { return _appearance; }
		}

		/// <summary>
		/// Creates a text renderer
		/// </summary>
		/// <param name="appearance">Visual appearance of the text</param>
		/// <exception cref="ArgumentNullException">The <paramref name="appearance"/> of the text can't be null.</exception>
		protected TextRenderer (TextAppearance appearance)
		{
			if(appearance == null)
				throw new ArgumentNullException("appearance");
			_appearance = appearance;
		}

		#region Rendering

		/// <summary>
		/// Calculates the bounds of the space that the text will occupy
		/// </summary>
		/// <returns>Width and height of the bounds</returns>
		protected abstract SFML.Window.Vector2u CalculateBounds ();

		/// <summary>
		/// Draws the text onto a texture
		/// </summary>
		/// <param name="target">Texture to render to</param>
		protected abstract void Draw (RenderTexture target);

		/// <summary>
		/// Draws and retrieves the texture that contains the text
		/// </summary>
		/// <returns>A texture containing the rendered text</returns>
		public Texture GetTexture ()
		{
			var bounds = CalculateBounds();
			var target = new RenderTexture(bounds.X, bounds.Y);
			return draw(target);
		}

		/// <summary>
		/// Draws and retrieves a texture with a predetermined size that contains the text
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		/// <returns>A texture containing the rendered text</returns>
		public Texture GetTexture (uint width, uint height)
		{
			var target = new RenderTexture(width, height);
			return draw(target);
		}

		/// <summary>
		/// Draws the text onto a texture
		/// </summary>
		/// <param name="target">Texture to draw on</param>
		/// <returns>Texture</returns>
		private Texture draw (RenderTexture target)
		{
			// Clear the back color to a transparent font color.
			// This fixes font smoothing issues blending to black.
			var backColor = new Color(_appearance.Color, 0);
			target.Clear(backColor);

			// Draw and finalize the texture
			Draw(target);
			target.Display();

			return new Texture(target.Texture).Clone();
		}
		#endregion

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the text renderer has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Triggered when the text renderer is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Frees the resources held by the text renderer
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Deconstructs the text renderer
		/// </summary>
		~TextRenderer ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the text renderer
		/// </summary>
		/// <param name="disposing">Flag indicating whether internal resources should be freed</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);
			}
		}
		#endregion
	}
}
