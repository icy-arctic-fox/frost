using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Frost.Utility;
using Newtonsoft.Json;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Maps input to easier to recognize values
	/// </summary>
	public abstract class InputScheme : IDisposable, IControllerBase
	{
		/// <summary>
		/// Maps input type to input ID to assignment ID
		/// </summary>
		private readonly Dictionary<int, int>[] _inputAssignments = new Dictionary<int, int>[(int)InputType.Count];

		/// <summary>
		/// Maps assigned IDs to the index of the source type (in <see cref="_inputAssignments"/>)
		/// </summary>
		private readonly Dictionary<int, int> _assignedIds = new Dictionary<int, int>();

		/// <summary>
		/// Assigns an ID to an input
		/// </summary>
		/// <param name="id">ID to assign</param>
		/// <param name="input">Description of the input</param>
		protected void AssignInput (int id, InputDescriptor input)
		{
			lock(_inputAssignments)
			{
				if(_assignedIds.ContainsKey(id))
					UnassignInput(id); // Un-assign existing ID first

				var index = (int)input.Type;
				var assignments = _inputAssignments[index];
				if(assignments == null)
				{ // Haven't assigned to this type of input yet
					_inputAssignments[index] = assignments = new Dictionary<int, int>(16);

					switch(input.Type)
					{// Subscribe to events
					case InputType.Keyboard:
						Keyboard.KeyPress   += Keyboard_KeyPress;
						Keyboard.KeyRelease += Keyboard_KeyRelease;
						break;

					case InputType.Mouse:
						if(input.Id == (int)MouseButton.None)
							Mouse.Move += Mouse_Move;
						else
						{
							Mouse.Press   += Mouse_Press;
							Mouse.Release += Mouse_Release;
						}
						break;

					default:
						throw new ArgumentException("Unsupported input source type");
					}
				}

				assignments[input.Id] = id;
				_assignedIds[id]      = index;
			}
		}

		/// <summary>
		/// Removes an assignment of an ID to an input
		/// </summary>
		/// <param name="id">ID of the input to un-assign</param>
		protected void UnassignInput (int id)
		{
			lock(_inputAssignments)
			{
				int index;
				if(_assignedIds.TryGetValue(id, out index))
				{
					var assignments = _inputAssignments[index];
					var found = (from entry in assignments where entry.Value == id select entry.Key).FirstOrDefault();
					assignments.Remove(found);

					if(assignments.Count <= 0)
					{// Removed the last of this type, unsubscribe
						switch((InputType)index)
						{// Subscribe to events
						case InputType.Keyboard:
							Keyboard.KeyPress   -= Keyboard_KeyPress;
							Keyboard.KeyRelease -= Keyboard_KeyRelease;
							break;

						case InputType.Mouse:
							Mouse.Move    -= Mouse_Move;
							Mouse.Press   -= Mouse_Press;
							Mouse.Release -= Mouse_Release;
							break;

						default:
							throw new ArgumentException("Unsupported input source type");
						}
						_inputAssignments[index] = null;
					}

					_assignedIds.Remove(id);
				}
			}
		}

		/// <summary>
		/// Reusable event arguments since input is triggered frequently
		/// </summary>
		private readonly InputEventArgs _inputEventArgs = new InputEventArgs();

		/// <summary>
		/// Triggered when any input is initially detected (such as a key being pressed)
		/// </summary>
		public event EventHandler<InputEventArgs> InputStarted;

		/// <summary>
		/// Called when an assigned input is initially detected (keyboard key press)
		/// </summary>
		/// <param name="args">Arguments for the input</param>
		/// <remarks>This method triggers the <see cref="InputStarted"/> event.</remarks>
		protected virtual void OnInputStarted (InputEventArgs args)
		{
			InputStarted.NotifySubscribers(this, args);
		}

		/// <summary>
		/// Triggered when any input stops (such as a key being released)
		/// </summary>
		public event EventHandler<InputEventArgs> InputEnded;

		/// <summary>
		/// Called when an assigned input has stopped (keyboard key release)
		/// </summary>
		/// <param name="args">Arguments for the input</param>
		/// <remarks>This method triggers the <see cref="InputEnded"/> event.</remarks>
		protected virtual void OnInputEnded (InputEventArgs args)
		{
			InputEnded.NotifySubscribers(this, args);
		}

		#region Subscribers

		private void Keyboard_KeyPress (object sender, KeyboardEventArgs e)
		{
			var input = new InputDescriptor(InputType.Keyboard, (int)e.Key);
			_inputEventArgs.Input = input;
			OnInputStarted(_inputEventArgs);
		}

		private void Keyboard_KeyRelease (object sender, KeyboardEventArgs e)
		{
			var input = new InputDescriptor(InputType.Keyboard, (int)e.Key);
			_inputEventArgs.Input = input;
			OnInputEnded(_inputEventArgs);
		}

		private void Mouse_Move (object sender, MouseEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Mouse_Press (object sender, MouseEventArgs e)
		{
			var input = new InputDescriptor(InputType.Mouse, (int)e.Buttons);
			_inputEventArgs.Input = input;
			OnInputStarted(_inputEventArgs);
		}

		private void Mouse_Release (object sender, MouseEventArgs e)
		{
			var input = new InputDescriptor(InputType.Mouse, (int)e.Buttons);
			_inputEventArgs.Input = input;
			OnInputEnded(_inputEventArgs);
		}
		#endregion

		#region Json
		private static readonly JsonSerializer Json;

		/// <summary>
		/// Initializes the Json serializer
		/// </summary>
		static InputScheme ()
		{
			Json = new JsonSerializer {Formatting = Formatting.Indented};
		}
		#endregion

		#region Save and load

		/// <summary>
		/// Saves the input scheme (control mapping/keybindings) to a file
		/// </summary>
		/// <param name="path">Path to the file</param>
		public void Save (string path)
		{
			using(var writer = File.CreateText(path))
				Json.Serialize(writer, this);
		}

		/// <summary>
		/// Loads the configuration from a file
		/// </summary>
		/// <param name="path">Path of the file to load from</param>
		/// <returns>The input scheme or null if there was an error loading the configuration</returns>
		public static InputScheme Load<TScheme> (string path) where TScheme : InputScheme
		{
			using(var reader = File.OpenText(path))
				return Json.Deserialize(reader, typeof(TScheme)) as TScheme;
		}
		#endregion

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the input scheme has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Disposes of the scheme and the resources it holds
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Destroys the input scheme
		/// </summary>
		~InputScheme ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the scheme and the resources it holds
		/// </summary>
		/// <param name="disposing">Indicates whether inner-resources should be disposed (<see cref="Dispose"/> was called)</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;

				// Unsubscribe from all events
				Keyboard.KeyPress   -= Keyboard_KeyPress;
				Keyboard.KeyRelease -= Keyboard_KeyRelease;

				Mouse.Move    -= Mouse_Move;
				Mouse.Press   -= Mouse_Press;
				Mouse.Release -= Mouse_Release;
			}
		}
		#endregion
	}
}
