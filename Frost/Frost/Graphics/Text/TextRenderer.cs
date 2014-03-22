using System;
using Frost.Utility;
using SFML.Graphics;
using T = SFML.Graphics.Text;

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
		public uint WrapWidth { get; set; }

		/// <summary>
		/// Current font that determines the appearance of the text
		/// </summary>
		public Font Font { get; set; }

		/// <summary>
		/// Horizontal alignment of the text within the bounds
		/// </summary>
		public TextAlignment Alignment { get; set; }

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
			Draw(target);
			return new Texture(target.Texture);
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
			Draw(target);
			return new Texture(target.Texture);
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
