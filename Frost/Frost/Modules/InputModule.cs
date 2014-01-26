using Frost.Modules.Input;
using M = SFML.Window.Mouse;

namespace Frost.Modules
{
	/// <summary>
	/// Retrieves input from the user and presents it in a friendly format for games
	/// </summary>
	public class InputModule : IModule
	{
		/// <summary>
		/// Starts up the module and prepares it for usage
		/// </summary>
		public void Initialize ()
		{
			_mouseEventArgs.Buttons  = _prevMouseButtons = Mouse.Buttons;
			_mouseEventArgs.Position = _prevMousePos     = M.GetPosition();
		}

		/// <summary>
		/// Polls the input devices for changes.
		/// This must be called for every logic update.
		/// </summary>
		public void Update ()
		{
			updateMouse(); // TODO: Skip this if none of the mouse events are subscribed to
			// TODO: Update keyboard
			// TODO: Update joysticks
		}

		#region Mouse

		/// <summary>
		/// Previous mouse buttons pressed - used to detect button presses
		/// </summary>
		private MouseButton _prevMouseButtons;

		/// <summary>
		/// Previous mouse position - used to detect mouse movement
		/// </summary>
		private SFML.Window.Vector2i _prevMousePos;
		
		/// <summary>
		/// Reused arguments given to mouse subscribers
		/// </summary>
		private readonly MouseEventArgs _mouseEventArgs = new MouseEventArgs();

		/// <summary>
		/// Updates the state of the mouse and dispatches events for it
		/// </summary>
		private void updateMouse ()
		{
			// Detect mouse button presses
			// This should be done in the following order: Release, Press
			var curButtons = Mouse.Buttons;
			var buttons    = _prevMouseButtons & curButtons;
			if(_prevMouseButtons != buttons)
			{// Button was released
				_mouseEventArgs.Buttons = _prevMouseButtons & ~curButtons;
				Mouse.OnRelease(_mouseEventArgs);
			}
			buttons = curButtons & ~_prevMouseButtons;
			if(buttons != MouseButton.None)
			{// Button was pressed
				_mouseEventArgs.Buttons = buttons;
				Mouse.OnPress(_mouseEventArgs);
			}
			_prevMouseButtons = curButtons;

			// Check for mouse movement
			// Movement is processed after button presses to prevent accidental drag.
			var curMousePos = M.GetPosition();
			if(curMousePos.X != _prevMousePos.X || curMousePos.Y != _prevMousePos.Y)
			{// Mouse moved
				_mouseEventArgs.Position = _prevMousePos = curMousePos;
				Mouse.OnMove(_mouseEventArgs);
			}
		}
		#endregion

		#region Disposable
		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the module has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Disposes of the module by releasing resources held by it
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Destructor - disposes of the module
		/// </summary>
		~InputModule ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the module
		/// </summary>
		/// <param name="disposing">True if inner-resources should be disposed of (<see cref="Dispose"/> was called)</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				if(disposing)
				{
					// ...
				}
			}
		}
		#endregion
	}
}
