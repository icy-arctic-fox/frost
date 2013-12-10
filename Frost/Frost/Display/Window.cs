using System;
using SFML.Graphics;
using SFML.Window;

namespace Frost.Display
{
	/// <summary>
	/// Interface for displaying graphics to the user
	/// </summary>
	public class Window : IDisposable
	{
		#region Constants

		/// <summary>
		/// Default width of newly created windows in pixels
		/// </summary>
		public const uint DefaultWidth = 960;

		/// <summary>
		/// Default height of newly created windows in pixels
		/// </summary>
		public const uint DefaultHeight = 540;

		/// <summary>
		/// Default title for newly created windows
		/// </summary>
		public const string DefaultTitle = "Frost Game Engine";
		#endregion

		protected readonly RenderWindow _window;

		/// <summary>
		/// Creates a new window to display graphics
		/// </summary>
		public Window ()
			: this(DefaultWidth, DefaultHeight, DefaultTitle)
		{
			// ...
		}

		/// <summary>
		/// Creates a new window of a given size to display graphics
		/// </summary>
		/// <param name="width">Initial width of the window in pixels</param>
		/// <param name="height">Initial height of the window in pixels</param>
		/// <param name="title">Text displayed in the title bar</param>
		public Window (uint width, uint height, string title)
		{
			_window = new RenderWindow(new VideoMode(width, height), title);
		}

		#region Size and location

		/// <summary>
		/// Updates the size of the window
		/// </summary>
		/// <param name="width">New width in pixels</param>
		/// <param name="height">New height in pixels</param>
		public void SetSize (uint width, uint height)
		{
			_window.Size = new Vector2u(width, height);
		}

		/// <summary>
		/// Width of the window in pixels
		/// </summary>
		public uint Width
		{
			get { return _window.Size.X; }
			set { SetSize(value, Height); }
		}

		/// <summary>
		/// Height of the window in pixels
		/// </summary>
		public uint Height
		{
			get { return _window.Size.Y; }
			set { SetSize(Width, value); }
		}

		/// <summary>
		/// Updates the location of the window
		/// </summary>
		/// <param name="x">New x-coordinate</param>
		/// <param name="y">New y-coordinate</param>
		public void SetPosition (int x, int y)
		{
			_window.Position = new Vector2i(x, y);
		}

		/// <summary>
		/// X-coordinate of the window's position on the screen
		/// </summary>
		public int X
		{
			get { return _window.Position.X; }
			set { SetPosition(value, Y); }
		}

		/// <summary>
		/// Y-coordinate of the window's position on the screen
		/// </summary>
		public int Y
		{
			get { return _window.Position.Y; }
			set { SetPosition(X, value); }
		}
		#endregion

		#region Disposable

		/// <summary>
		/// Indicates whether or not the window has been disposed
		/// </summary>
		public bool Disposed { get; private set; }

		/// <summary>
		/// Closes the window and frees the resources held by it
		/// </summary>
		/// <remarks>If the window has already been disposed, then this method will do nothing.</remarks>
		/// <seealso cref="Disposed"/>
		public void Dispose ()
		{
			if(!Disposed)
			{// Window hasn't been disposed of yet
				Disposed = true;
				throw new NotImplementedException();
			}
		}
		#endregion
	}
}
