using System;
using System.Collections.Generic;
using Frost.Geometry;
using Frost.Graphics.Text;
using Frost.Utility;
using SFML.Graphics;
using Color = Frost.Graphics.Color;
using Font = Frost.Graphics.Text.Font;

namespace Frost.UI
{
	/// <summary>
	/// Informational overlay that displays useful debugging information.
	/// Lines can be added dynamically to the overlay while the game is running.
	/// </summary>
	public class DebugOverlay : IFrameRender, IFullDisposable
	{
		private const int TextColor        = 0xffffff;
		private const int BackgroundColor  = 0x404040;
		private const byte BackgroundAlpha = 0x80;

		private static readonly SFML.Graphics.Color _backgroundColor = new Color(BackgroundColor, BackgroundAlpha);
		private static readonly Color _textColor = new Color(TextColor);

		private readonly SFML.Graphics.Sprite _background;
		private readonly Font _font;
		private readonly uint _fontSize;
		private readonly object _locker = new object();

		// These are parallel arrays
		private readonly List<IDebugOverlayLine> _content = new List<IDebugOverlayLine>();
		private readonly List<SimpleText> _textLines = new List<SimpleText>();

		/// <summary>
		/// Creates a new debug overlay
		/// </summary>
		/// <param name="runner">Game runner to pull information from</param>
		/// <param name="font">Font used to display text</param>
		/// <param name="fontSize">Font size</param>
		/// <exception cref="ArgumentNullException">The game <paramref name="runner"/> and the <paramref name="font"/> used to display the text can't be null.</exception>
		public DebugOverlay (GameRunner runner, Font font, uint fontSize)
		{
			if(runner == null)
				throw new ArgumentNullException("runner");
			if(font == null)
				throw new ArgumentNullException("font");

			_font       = font;
			_fontSize   = fontSize;
			_background = new SFML.Graphics.Sprite();
		}

		/// <summary>
		/// Adds a line to the debug overlay.
		/// The object is queried every update to get the text that should be displayed.
		/// </summary>
		/// <param name="item">Line to add</param>
		/// <exception cref="ArgumentNullException">The new debug line (<paramref name="item"/>) can't be null.</exception>
		public void AddLine (IDebugOverlayLine item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			item.Disposing += content_Disposing;

			var line = new SimpleText(_font, _fontSize, _textColor);
			lock(_locker)
			{
				_content.Add(item);
				_textLines.Add(line);
			}
		}

		/// <summary>
		/// Removes a line from the debug overlay
		/// </summary>
		/// <param name="item">Line to remove</param>
		/// <returns>True if <paramref name="item"/> was found and removed</returns>
		/// <exception cref="ArgumentNullException">The debug line (<paramref name="item"/>) to remove can't be null.</exception>
		public bool RemoveLine (IDebugOverlayLine item)
		{
			if(item == null)
				throw new ArgumentNullException("item");

			lock(_locker)
				return _content.Remove(item);
			// BUG: Items not removed from _textLines
		}

		/// <summary>
		/// Called when content displayed in the overlay is being disposed
		/// </summary>
		/// <param name="sender">Object being disposed</param>
		/// <param name="e">Event arguments (ignored)</param>
		/// <exception cref="InvalidCastException">Thrown if <paramref name="sender"/> is not a <see cref="IDebugOverlayLine"/></exception>
		private void content_Disposing (object sender, EventArgs e)
		{
			var item = sender as IDebugOverlayLine;
			if(item == null)
				throw new InvalidCastException("The sender of the object being disposed must be a debug overlay line.");
			while(RemoveLine(item)) {} // Remove all instances of the item
		}

		/// <summary>
		/// Boundaries of the overlay window
		/// </summary>
		public Rect2f Bounds { get; private set; }

		// TODO: Add ability to specify X and Y position

		private volatile bool _resize;

		/// <summary>
		/// Updates the contents of the overlay
		/// </summary>
		public void Update ()
		{
			updateText();
			
			// Calculate the bounds
			var point = calculateBounds(); // Point at the bottom-right corner
			if(point.X > Bounds.Width || point.Y > Bounds.Height)
			{// Bounds need to be updated and the background resized
				Bounds  = new Rect2f(0f, 0f, point.X, point.Y);
				_resize = true;
			}
		}

		/// <summary>
		/// Updates the text displayed in the overlay
		/// </summary>
		private void updateText ()
		{
			lock(_locker)
				for(var i = 0; i < _content.Count; ++i)
				{// Update the text contents of each line
					var content = _content[i];
					var line    = _textLines[i];

					var str = line.Contents;
					str.Clear();
					content.GetDebugInfo(str);
					line.Update();
				}
		}

		/// <summary>
		/// Calculates the bounds of every line
		/// </summary>
		/// <returns>Position of the bottom-right corner of the overlay</returns>
		private Point2f calculateBounds ()
		{
			float width = 0f, height = 0f;
			lock(_locker)
				for(var i = 0; i < _content.Count; ++i)
				{// Expand the bounds for each line
					var line = _textLines[i];
					var rect = line.Bounds;
					if(rect.Width > width)
						width = rect.Width;
					height += rect.Height;
				}

			return new Point2f(width, height);
		}

		/// <summary>
		/// Draws the debug overlay onto the display
		/// </summary>
		/// <param name="args">Render information</param>
		public void Render ( FrameDrawEventArgs args)
		{
			var bounds = Bounds;
			if(_background.Texture == null || _resize)
			{// The background needs to be resized
				// Calculate the bounds
				var width  = (uint)(bounds.Width + 1);
				var height = (uint)(bounds.Height + 1);

				// Create the new texture
				var background          = new Image(width, height, _backgroundColor);
				_background.Texture     = new SFML.Graphics.Texture(background);
				_background.TextureRect = new IntRect(0, 0, (int)width, (int)height);
				_resize = false;
			}

			// Draw the background
			var rs = RenderStates.Default;
			rs.Transform.Translate(bounds.Left, bounds.Top);
			args.Display.Draw(_background, rs);

			// Draw each line
			var yPos = bounds.Top;
			lock(_locker)
				for(var i = 0; i < _textLines.Count; ++i)
				{
					var line = _textLines[i];
					line.Draw(args.Display, bounds.Left, yPos);
					yPos += line.Bounds.Height;
				}
		}

		#region Disposable

		/// <summary>
		/// Flag that indicates whether the overlay has been disposed
		/// </summary>
		public bool Disposed { get; private set; }

		/// <summary>
		/// Triggered when the overlay is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Frees the resources held by the overlay
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Tears down the debug overlay
		/// </summary>
		~DebugOverlay ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Underlying method that releases the resources held by the overlay
		/// </summary>
		/// <param name="disposing">True if <see cref="Dispose"/> was called and inner resources should be disposed as well</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!Disposed)
			{// Don't do anything if the overlay is already disposed
				Disposed = true;
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);
				if(disposing)
				{// Dispose of the resources this object holds
					_content.Clear();
					_textLines.Clear();
					_background.Dispose();
				}
			}
		}
		#endregion
	}
}
