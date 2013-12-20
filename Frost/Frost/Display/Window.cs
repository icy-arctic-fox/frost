using System;
using SFML.Graphics;
using SFML.Window;

namespace Frost.Display
{
	/// <summary>
	/// Interface for displaying graphics to the user
	/// </summary>
	public class Window : IDisplay, IDisposable
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

		/// <summary>
		/// Underlying implementation of the window.
		/// This references the SFML window object that displays and controls the window functionality.
		/// </summary>
		protected readonly RenderWindow Implementation;

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
			_title = title ?? String.Empty;
			Implementation = new RenderWindow(new VideoMode(width, height), _title);
		}

		#region Size and location

		/// <summary>
		/// Updates the size of the window
		/// </summary>
		/// <param name="width">New width in pixels</param>
		/// <param name="height">New height in pixels</param>
		public void SetSize (uint width, uint height)
		{
			Implementation.Size = new Vector2u(width, height);
		}

		/// <summary>
		/// Width of the window in pixels
		/// </summary>
		public uint Width
		{
			get { return Implementation.Size.X; }
			set { SetSize(value, Height); }
		}

		/// <summary>
		/// Height of the window in pixels
		/// </summary>
		public uint Height
		{
			get { return Implementation.Size.Y; }
			set { SetSize(Width, value); }
		}

		/// <summary>
		/// Updates the location of the window
		/// </summary>
		/// <param name="x">New x-coordinate</param>
		/// <param name="y">New y-coordinate</param>
		public void SetPosition (int x, int y)
		{
			Implementation.Position = new Vector2i(x, y);
		}

		/// <summary>
		/// X-coordinate of the window's position on the screen
		/// </summary>
		public int X
		{
			get { return Implementation.Position.X; }
			set { SetPosition(value, Y); }
		}

		/// <summary>
		/// Y-coordinate of the window's position on the screen
		/// </summary>
		public int Y
		{
			get { return Implementation.Position.Y; }
			set { SetPosition(X, value); }
		}
		#endregion

		/// <summary>
		/// Cached string for the window title
		/// </summary>
		/// <remarks>This is needed since SFML doesn't supply a way to get the window title.</remarks>
		private string _title;

		/// <summary>
		/// Text displayed in the title bar
		/// </summary>
		public string Title
		{
			get { return _title; }
			set
			{
				_title = value ?? String.Empty;
				Implementation.SetTitle(_title);
			}
		}

		/// <summary>
		/// Cached flag for window visibility
		/// </summary>
		/// <remarks>This is needed since SFML doesn't supply a way to get the window visibility.</remarks>
		private bool _visible = true;

		/// <summary>
		/// Indicates if the window is visible on the screen
		/// </summary>
		public bool Visible
		{
			get { return _visible; }
			set
			{
				_visible = value;
				Implementation.SetVisible(_visible);
			}
		}

		/// <summary>
		/// SFML color for the background.
		/// This is used instead of the <see cref="Frost.Graphics.Color"/> structure because converting it every frame would be a waste.
		/// </summary>
		private Color _backColor = Color.Black;

		/// <summary>
		/// Background color displayed beneath all of the drawn components
		/// </summary>
		public Graphics.Color BackgroundColor
		{
			get { return _backColor; }
			set { _backColor = value; }
		}

		#region Display

		public void Draw (Sprite sprite)
		{
			Implementation.Draw(sprite);
		}

		/// <summary>
		/// Sets a window as being actively rendered in on a thread.
		/// The window can be rendered from only one thread at a time.
		/// The initial thread that created the window will be "active."
		/// Call this method with false to disable it so that other threads can render to it.
		/// A thread must call this method with a true flag to enable rendering for that thread, but this will not work if rendering is active on another thread.
		/// </summary>
		/// <param name="flag">True if the window is active for rendering on the current thread, or false to disable rendering on the current thread</param>
		/// <returns>True if the current thread is now active for rendering, false if another thread is already rendering.</returns>
		public bool SetActive (bool flag = true)
		{
			return Implementation.SetActive(flag);
		}

		/// <summary>
		/// Processes window messages
		/// </summary>
		public void Update ()
		{
			Implementation.DispatchEvents();
		}

		/// <summary>
		/// Starts a new frame
		/// </summary>
		public void EnterFrame ()
		{
			Implementation.Clear(_backColor);
		}

		/// <summary>
		/// Ends drawing commands for a frame and displays it
		/// </summary>
		public void ExitFrame ()
		{
			Implementation.Display();
		}
		#endregion

		#region Disposable

		/// <summary>
		/// Indicates whether or not the window and its resources have been freed
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
				Implementation.Close();
				Implementation.Dispose();
			}
		}
		#endregion
	}
}
