using System;
using SFML.Graphics;
using SFML.Window;
using Frost.Modules.Input;
using Frost.Utility;
using Mouse = Frost.Modules.Input.Mouse;
using M     = SFML.Window.Mouse;

namespace Frost.Display
{
	/// <summary>
	/// Interface for displaying graphics to the user
	/// </summary>
	public class Window : IDisplay, IFullDisposable
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
		private readonly RenderWindow _window;

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
			_title  = title ?? String.Empty;
			_window = new RenderWindow(new VideoMode(width, height), _title);

			// Listen for window events
			subscribe();
		}

		private void subscribe ()
		{
			_window.Closed      += _window_Closed;
			_window.LostFocus   += _window_LostFocus;
			_window.GainedFocus += _window_GainedFocus;

			_window.MouseButtonPressed  += _window_MouseButtonPressed;
			_window.MouseButtonReleased += _window_MouseButtonReleased;
			_window.MouseEntered        += _window_MouseEntered;
			_window.MouseLeft           += _window_MouseLeft;
			_window.MouseMoved          += _window_MouseMoved;
			Mouse.Release += Mouse_Release;
			// TODO: MouseWheelMoved	
		}

		private void unsubscribe ()
		{
			_window.Closed      -= _window_Closed;
			_window.LostFocus   -= _window_LostFocus;
			_window.GainedFocus -= _window_GainedFocus;

			_window.MouseButtonPressed  -= _window_MouseButtonPressed;
			_window.MouseButtonReleased -= _window_MouseButtonReleased;
			_window.MouseEntered        -= _window_MouseEntered;
			_window.MouseLeft           -= _window_MouseLeft;
			_window.MouseMoved          -= _window_MouseMoved;
			Mouse.Release -= Mouse_Release;
			// TODO: MouseWheelMoved
		}

		#region Events
		#region Closing

		/// <summary>
		/// Triggered when the user clicks the close button on the window
		/// </summary>
		public event EventHandler<WindowClosingEventArgs> Closing;

		/// <summary>
		/// Called when the user clicks the close button on the window
		/// </summary>
		/// <param name="args">Event arguments</param>
		/// <remarks>Canceling this event through <paramref name="args"/> will prevent the window from closing.
		/// Not canceling the event will close the window.
		/// This method invokes the <see cref="Closing"/> event.</remarks>
		protected void OnWindowClosing (WindowClosingEventArgs args)
		{
			Closing.NotifySubscribers(this, args);
			if(!args.IsCanceled)
				OnWindowClose(args);
		}

		/// <summary>
		/// Triggered when the user clicks the close button on the window
		/// </summary>
		/// <param name="sender">Window implementation</param>
		/// <param name="e">Event arguments</param>
		void _window_Closed (object sender, EventArgs e)
		{
			var args = new WindowClosingEventArgs();
			OnWindowClosing(args);
		}
		#endregion

		#region Closed

		/// <summary>
		/// Checks if the window is open
		/// </summary>
		public bool IsOpen
		{
			get { return _window.IsOpen(); }
		}

		/// <summary>
		/// Triggered when the window closes
		/// </summary>
		public event EventHandler<EventArgs> Closed;

		/// <summary>
		/// Called when the window closes
		/// </summary>
		/// <param name="args">Event arguments</param>
		/// <remarks>This method invokes the <see cref="Closed"/> event.</remarks>
		protected void OnWindowClose (EventArgs args)
		{
			_window.Close();
			Closed.NotifySubscribers(this, args);
		}
		#endregion

		#region Focus events
		/// <summary>
		/// Indicates whether the window has focus
		/// </summary>
		public bool HasFocus { get; private set; }

		#region GainedFocus

		/// <summary>
		/// Triggered when the window gains focus
		/// </summary>
		public event EventHandler<EventArgs> GainedFocus;

		/// <summary>
		/// Called when the window gains focus
		/// </summary>
		/// <param name="e">Event arguments</param>
		/// <remarks>This method triggers the <see cref="GainedFocus"/> event.</remarks>
		protected virtual void OnGainedFocus (EventArgs e)
		{
			HasFocus = true;
			GainedFocus.NotifySubscribers(this, e);
		}

		/// <summary>
		/// Called when the underlying window gains focus
		/// </summary>
		/// <param name="sender">Underlying window</param>
		/// <param name="e">Event arguments</param>
		private void _window_GainedFocus (object sender, EventArgs e)
		{
			OnGainedFocus(e);
		}
		#endregion

		#region LostFocus

		/// <summary>
		/// Triggered when the window has lost focus
		/// </summary>
		public event EventHandler<EventArgs> LostFocus;

		/// <summary>
		/// Called when the window has lost focus
		/// </summary>
		/// <param name="e">Event arguments</param>
		/// <remarks>This method triggers the <see cref="LostFocus"/> event.</remarks>
		protected virtual void OnLostFocus (EventArgs e)
		{
			HasFocus = false;
			LostFocus.NotifySubscribers(this, e);
		}

		/// <summary>
		/// Called when the underlying window has lost focus
		/// </summary>
		/// <param name="sender">Underlying window</param>
		/// <param name="e">Event arguments</param>
		private void _window_LostFocus (object sender, EventArgs e)
		{
			OnLostFocus(e);
		}
		#endregion
		#endregion

		#region Mouse events

		/// <summary>
		/// Reusable mouse event arguments passed to listeners
		/// </summary>
		private readonly MouseEventArgs _mouseEventArgs = new MouseEventArgs();
		#region MouseDown

		/// <summary>
		/// Triggered when the mouse is pressed inside the window
		/// </summary>
		public event EventHandler<MouseEventArgs> MouseDown;

		/// <summary>
		/// Called when a mouse button is pressed inside the window
		/// </summary>
		/// <param name="args">Mouse event arguments</param>
		/// <remarks>This method triggers the <see cref="MouseDown"/> event.</remarks>
		protected virtual void OnMouseDown (MouseEventArgs args)
		{
			MouseDown.NotifySubscribers(this, args);
		}

		/// <summary>
		/// Called when the underlying window has a mouse button pressed in it
		/// </summary>
		/// <param name="sender">Underlying window</param>
		/// <param name="e">Mouse event arguments</param>
		private void _window_MouseButtonPressed (object sender, MouseButtonEventArgs e)
		{
			_mouseEventArgs.Buttons  = e.Button.FromSfml();
			_mouseEventArgs.Position = new Point2D(e.X, e.Y);
			OnMouseDown(_mouseEventArgs);
		}
		#endregion

		#region MouseUp

		/// <summary>
		/// Triggered when a mouse button is release inside or outside of the window
		/// </summary>
		public event EventHandler<MouseEventArgs> MouseUp;

		/// <summary>
		/// Called when a mouse button is release inside or outside of the window
		/// </summary>
		/// <param name="args">Mouse event arguments</param>
		/// <remarks>This method triggers the <see cref="MouseUp"/> event.</remarks>
		protected virtual void OnMouseUp (MouseEventArgs args)
		{
			MouseUp.NotifySubscribers(this, args);
		}

		/// <summary>
		/// Called when the mouse is released inside or outside of the window
		/// </summary>
		/// <param name="sender">Invoking object</param>
		/// <param name="e">Mouse event arguments</param>
		void Mouse_Release (object sender, MouseEventArgs e)
		{
			OnMouseUp(e);
		}
		#endregion

		#region MouseEnter

		/// <summary>
		/// Triggered when the mouse moves into the window
		/// </summary>
		public event EventHandler<MouseEventArgs> MouseEnter;

		/// <summary>
		/// Called when the mouse moves into the window
		/// </summary>
		/// <param name="args">Mouse event arguments</param>
		/// <remarks>This method triggers the <see cref="MouseEnter"/> event.</remarks>
		protected virtual void OnMouseEnter (MouseEventArgs args)
		{
			MouseEnter.NotifySubscribers(this, args);
		}

		/// <summary>
		/// Called when the mouse enters the underlying window
		/// </summary>
		/// <param name="sender">Underlying window</param>
		/// <param name="e">Mouse event arguments</param>
		void _window_MouseEntered (object sender, EventArgs e)
		{
			_mouseEventArgs.Buttons  = MouseButton.None;
			_mouseEventArgs.Position = MousePosition;
			OnMouseEnter(_mouseEventArgs);
		}
		#endregion

		#region MouseLeave

		/// <summary>
		/// Triggered when the mouse leaves the window
		/// </summary>
		public event EventHandler<MouseEventArgs> MouseLeave;

		/// <summary>
		/// Called when the mouse leaves the window
		/// </summary>
		/// <param name="args">Mouse event arguments</param>
		/// <remarks>This method triggers the <see cref="MouseLeave"/> event.</remarks>
		protected virtual void OnMouseLeave (MouseEventArgs args)
		{
			MouseLeave.NotifySubscribers(this, args);
		}

		/// <summary>
		/// Called when the mouse leaves the underlying window
		/// </summary>
		/// <param name="sender">Underlying window</param>
		/// <param name="e">Mouse event arguments</param>
		void _window_MouseLeft (object sender, EventArgs e)
		{
			_mouseEventArgs.Buttons  = MouseButton.None;
			_mouseEventArgs.Position = MousePosition;
			OnMouseLeave(_mouseEventArgs);
		}
		#endregion

		#region MouseMove

		/// <summary>
		/// Triggered when the mouse moves inside of the window
		/// </summary>
		public event EventHandler<MouseEventArgs> MouseMove;

		/// <summary>
		/// Called when the mouse moves inside of the window
		/// </summary>
		/// <param name="args">Mouse event arguments</param>
		/// <remarks>This method triggers the <see cref="MouseMove"/> event.</remarks>
		protected virtual void OnMouseMove (MouseEventArgs args)
		{
			MouseMove.NotifySubscribers(this, args);
		}

		/// <summary>
		/// Called when the mouse moves inside the underlying window
		/// </summary>
		/// <param name="sender">Underlying window</param>
		/// <param name="e">Mouse event arguments</param>
		void _window_MouseMoved (object sender, MouseMoveEventArgs e)
		{
			_mouseEventArgs.Buttons  = Mouse.Buttons;
			_mouseEventArgs.Position = new Point2D(e.X, e.Y);
			OnMouseMove(_mouseEventArgs);
		}
		#endregion

		#region Click

		/// <summary>
		/// Triggered when a mouse button is clicked (pressed and released)
		/// </summary>
		public event EventHandler<MouseEventArgs> Click;

		/// <summary>
		/// Called when a mouse button is pressed and released inside the window
		/// </summary>
		/// <param name="args">Event arguments</param>
		/// <remarks>This method triggers the <see cref="Click"/> event.</remarks>
		protected virtual void OnClick (MouseEventArgs args)
		{
			Click.NotifySubscribers(null, args);
		}

		/// <summary>
		/// Called when the underlying window has a mouse button released in it
		/// </summary>
		/// <param name="sender">Underlying window</param>
		/// <param name="e">Mouse event arguments</param>
		void _window_MouseButtonReleased (object sender, MouseButtonEventArgs e)
		{
			_mouseEventArgs.Buttons  = e.Button.FromSfml();
			_mouseEventArgs.Position = new Point2D(e.X, e.Y);
			OnClick(_mouseEventArgs);
		}
		#endregion
		#endregion
		#endregion

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
				_window.SetTitle(_title);
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
				_window.SetVisible(_visible);
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

		#region Mouse
		#region Position

		/// <summary>
		/// Location of the mouse along the x-axis relative to the window's top-left corner
		/// </summary>
		public int MouseX
		{
			get { return M.GetPosition(_window).X; }
			set { M.SetPosition(new Vector2i(value, MouseY), _window); }
		}

		/// <summary>
		/// Location of the mouse along the y-axis relative to the window's top-left corner
		/// </summary>
		public int MouseY
		{
			get { return M.GetPosition(_window).Y; }
			set { M.SetPosition(new Vector2i(MouseX, value), _window); }
		}

		/// <summary>
		/// Location of the mouse on the screen
		/// </summary>
		public Point2D MousePosition
		{
			get { return M.GetPosition(_window); }
			set { M.SetPosition(value, _window); }
		}
		#endregion
		#endregion

		#region Display

		private volatile bool _vsync; // TODO: Is it possible to find out whether VSync is enabled on start? Force on or off on start-up otherwise.

		/// <summary>
		/// Indicates whether vertical synchronization is enabled
		/// </summary>
		/// <remarks>Vertical synchronization forces frames to be drawn to the screen in time with screen refreshes.
		/// Enabling VSync can reduce the rendering rate (fps).</remarks>
		public bool VSync
		{
			get { return _vsync; } // TODO: This may be incorrect at startup
			set
			{
				_vsync = value;
				_window.SetVerticalSyncEnabled(value);
			}
		}

		/// <summary>
		/// Draws a basic object to the window
		/// </summary>
		/// <param name="drawable">Object to draw</param>
		public void Draw (Drawable drawable)
		{
			_window.Draw(drawable);
		}

		/// <summary>
		/// Draws a basic object with a transformation
		/// </summary>
		/// <param name="drawable">Object to draw</param>
		/// <param name="transform">Transformation to apply to <paramref name="drawable"/></param>
		public void Draw (Drawable drawable, RenderStates transform)
		{
			_window.Draw(drawable, transform);
		}

		/// <summary>
		/// Draws vertices with a transformation
		/// </summary>
		/// <param name="verts">Vertices to draw</param>
		/// <param name="transform">Transformation to apply to <paramref name="verts"/></param>
		public void Draw (Vertex[] verts, RenderStates transform)
		{
			_window.Draw(verts, PrimitiveType.Quads, transform); // TODO: Support primitive type selection
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
			return _window.SetActive(flag);
		}

		/// <summary>
		/// Processes window messages
		/// </summary>
		public void Update ()
		{
			_window.DispatchEvents();
		}

		/// <summary>
		/// Starts a new frame
		/// </summary>
		public void EnterFrame ()
		{
			_window.Clear(_backColor);
		}

		/// <summary>
		/// Ends drawing commands for a frame and displays it
		/// </summary>
		public void ExitFrame ()
		{
			_window.Display();
		}
		#endregion

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether or not the window and its resources have been freed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
			set { _disposed = value; }
		}

		/// <summary>
		/// Triggered when the window is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Closes the window and releases it and its resources
		/// </summary>
		/// <remarks>If the window has already been disposed, then this method will do nothing.</remarks>
		/// <seealso cref="Disposed"/>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Deconstructs the window
		/// </summary>
		~Window ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Closes the window and frees the resources held by it
		/// </summary>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{// Window hasn't been disposed of yet
				_disposed = true;
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);

				unsubscribe();
				if(disposing)
				{
					_window.Close();
					_window.Dispose();
				}
			}
		}
		#endregion
	}
}
