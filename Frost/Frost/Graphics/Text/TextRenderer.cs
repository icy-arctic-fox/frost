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
		#region Underlying text objects

		/// <summary>
		/// Tracks the current state of the text
		/// </summary>
		protected readonly T TextObject = new T();

		private Font _font;
		#endregion

		private bool _multiLine = true,
			_wordWrap = true;

		/// <summary>
		/// Indicates whether the text is allowed to span multiple lines and can contain newlines
		/// </summary>
		/// <remarks>Newlines (\n) and carriage-returns (\r) will be ignored if this property is false.</remarks>
		public bool MultiLine
		{
			get { return _multiLine; }
			set
			{
				if(value != _multiLine)
				{
					Prepared   = false;
					_multiLine = value;
				}
			}
		}

		/// <summary>
		/// Indicates whether words automatically wrap to the next line if the text becomes longer than a specified length
		/// </summary>
		public bool WordWrap
		{
			get { return _wordWrap; }
			set
			{
				if(value != _wordWrap)
				{
					Prepared  = false;
					_wordWrap = value;
				}
			}
		}

		private uint _wrapWidth;

		/// <summary>
		/// Width of text (in pixels) to wrap each line by
		/// </summary>
		/// <remarks>There must be at least one word per line.
		/// It is possible to go over this length if the first word on a line has a pixel length greater than this value.</remarks>
		public uint WrapWidth
		{
			get { return _wrapWidth; }
			set
			{
				if(value != _wrapWidth)
				{
					Prepared   = false;
					_wrapWidth = value;
				}
			}
		}

		/// <summary>
		/// Current font that determines the appearance of the text
		/// </summary>
		public Font Font
		{
			get { return _font; }
			set
			{
				Prepared = false;
				_font = value;
				TextObject.Font = value.UnderlyingFont;
			}
		}

		private TextAlignment _align;

		/// <summary>
		/// Horizontal alignment of the text within the bounds
		/// </summary>
		public TextAlignment Alignment
		{
			get { return _align; }
			set
			{
				Prepared = false;
				_align   = value;
			}
		}

		/// <summary>
		/// Underlying texture used to draw text on
		/// </summary>
		private RenderTexture _buffer;

		/// <summary>
		/// Texture that is returned after drawing
		/// </summary>
		private Texture _texture;

		/// <summary>
		/// Underlying texture to draw the text onto when preparing
		/// </summary>
		protected RenderTarget Buffer
		{
			get { return _buffer; }
		}

		#region Rendering

		/// <summary>
		/// Indicates whether the text has been prepared (rendered) internally so that is is ready to be quickly drawn
		/// </summary>
		public bool Prepared { get; protected set; }

		/// <summary>
		/// Disposes of the old underlying texture that text was draw on.
		/// This should be called whenever a textual property has been changed.
		/// </summary>
		protected void ResetTexture ()
		{
			if(_buffer != null)
			{
				_buffer.Dispose();
				_buffer = null;

				if(_texture != null)
					_texture.InternalTexture.Dispose();
			}
		}

		/// <summary>
		/// Prepares the underlying texture to be drawn on.
		/// This must be called as part of the <see cref="Prepare"/> process.
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		protected void PrepareTexture (uint width, uint height)
		{
			if(_buffer != null)
				_buffer.Dispose();
			_buffer = new RenderTexture(width, height);

			if(_texture == null)
				_texture = new Texture(_buffer.Texture);
			else
				_texture.InternalTexture = _buffer.Texture;
		}

		/// <summary>
		/// Prepares the text for drawing.
		/// This method renders the text internally so that it is ready to be quickly drawn.
		/// </summary>
		public abstract void Prepare ();

		/// <summary>
		/// Draws and retrieves the texture that contains the text
		/// </summary>
		/// <returns>A texture containing the rendered text</returns>
		/// <remarks>If the text hasn't been prepared by <see cref="Prepare"/> prior to calling this method,
		/// <see cref="Prepare"/> will be called before drawing the text.
		/// However, if it has been, then no drawing needs to be done.
		/// The texture only needs to be redrawn when a property is changed.</remarks>
		public Texture GetTexture ()
		{
			if(!Prepared)
			{
				Prepare();
#if DEBUG
				if(_texture == null) // Would be nice to check if the internal texture was disposed
					throw new ApplicationException("The text renderer implementation did not prepare the underlying texture.");
#endif
				_buffer.Display();
			}
			return _texture.Clone();
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

				if(disposing)
				{// Dispose of the internal resources
					if(Prepared)
						_buffer.Texture.Dispose();
				}
			}
		}
		#endregion
	}
}
