using System;
using System.Threading;
using Frost.Modules.Input;
using Frost.Utility;
using M = SFML.Window.Mouse;
using K = SFML.Window.Keyboard;

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
			updateKeyboard();  // TODO: Skip this if none of the keyboard events are subscribed to
			// TODO: Update joysticks
		}

		private InputDescriptor _capturedInput;
		private readonly ManualResetEventSlim _capturing = new ManualResetEventSlim();

		/// <summary>
		/// Captures the next input that the user makes and returns it
		/// </summary>
		/// <param name="timeout">Maximum amount of time to wait in milliseconds</param>
		/// <returns>The input that the user provided or <see cref="InputDescriptor.Unassigned"/> if it took longer than <paramref name="timeout"/> to receive input</returns>
		/// <remarks>This method should be called from a separate thread, as it would hang the update thread and not return any captured input.
		/// All 'input started' events will not trigger while this method is blocking.</remarks>
		public InputDescriptor CaptureNextInput (int timeout)
		{
			lock(_capturing)
			{
				_capturedInput = InputDescriptor.Unassigned;
				_capturing.Reset();
			}

			_capturing.Wait(timeout);
			return _capturedInput;
		}

		/// <summary>
		/// Marks input as being captured
		/// </summary>
		/// <param name="type">Type of captured input</param>
		/// <param name="value">Value of the captured input</param>
		private void capturedInput (InputType type, int value)
		{
			var input = new InputDescriptor(type, value);
			lock(_capturing)
			{
				_capturedInput = input;
				_capturing.Set();
			}
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
				buttons = _prevMouseButtons & ~curButtons;
				for(var b = (MouseButton)0; b < MouseButton.Count; ++b)
					if(buttons.HasFlag(b))
					{
						_mouseEventArgs.Buttons = b;
						Mouse.OnRelease(_mouseEventArgs);
					}
			}
			buttons = curButtons & ~_prevMouseButtons;
			if(buttons != MouseButton.None)
			{// Button was pressed
				for(var b = (MouseButton)0; b < MouseButton.Count; ++b)
					if(buttons.HasFlag(b))
					{
						if(_capturing.IsSet)
						{// Not capturing
							_mouseEventArgs.Buttons = buttons;
							Mouse.OnPress(_mouseEventArgs);
						}
						else // Capture input
							capturedInput(InputType.Mouse, (int)buttons);
					}
			}
			_prevMouseButtons = curButtons;

			// Check for mouse movement
			// Movement is processed after button presses to prevent accidental drag.
			var curMousePos = M.GetPosition();
			if(curMousePos.X != _prevMousePos.X || curMousePos.Y != _prevMousePos.Y)
			{// Mouse moved
				_mouseEventArgs.Position = _prevMousePos = curMousePos;
				Mouse.OnMove(_mouseEventArgs);
				// TODO: Add capturing
			}
		}
		#endregion

		#region Keyboard

		/// <summary>
		/// Status of each keyboard key (true for pressed)
		/// </summary>
		private readonly FlagArray _keyStates = new FlagArray((int)K.Key.KeyCount);

		/// <summary>
		/// Reused arguments given to keyboard subscribers
		/// </summary>
		private readonly KeyboardEventArgs _keyboardEventArgs = new KeyboardEventArgs(Key.A);

		/// <summary>
		/// Updates the state of the keyboard and dispatches events for it
		/// </summary>
		private void updateKeyboard ()
		{
			// TODO: Do all of the keys need to iterated?
			var i = 0;
			for(var k = (K.Key)0; k < K.Key.KeyCount; ++k, ++i)
			{
				var pressed    = K.IsKeyPressed(k);
				var wasPressed = _keyStates[i];
				if(wasPressed && !pressed)
				{// Key was released
					_keyStates[i] = false;
					_keyboardEventArgs.Key = (Key)k;
					Keyboard.OnKeyRelease(_keyboardEventArgs);
				}
				else if(!wasPressed && pressed)
				{// Key was pressed
					if(_capturing.IsSet)
					{// Not capturing
						_keyStates[i] = true;
						_keyboardEventArgs.Key = (Key)k;
						Keyboard.OnKeyPress(_keyboardEventArgs);
					}
					else // Capture input
						capturedInput(InputType.Keyboard, i);
				}
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
		/// Triggered when the module begins disposing
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Disposes of the module by releasing resources held by it
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
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
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);
				if(disposing)
				{
					// ...
				}
			}
		}
		#endregion
	}
}
