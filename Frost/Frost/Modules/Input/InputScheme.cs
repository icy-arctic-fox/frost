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

				var i = (int)input.Type;
				var assignments = _inputAssignments[i];
				if(assignments == null)
				{// Haven't assigned to this type of input yet
					_inputAssignments[i] = assignments = new Dictionary<int, int>(16);

					switch(input.Type)
					{// Subscribe to events
					case InputType.Keyboard:
						Keyboard.KeyPress   += Keyboard_KeyPress;
						Keyboard.KeyRelease += Keyboard_KeyRelease;
						break;

					case InputType.Mouse:
						if(input.Value == (int)MouseButton.None) // TODO: Treat +/- mouse axis separate
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

				assignments[input.Value] = id;
				_assignedIds[id]         = i;
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
				int i;
				if(_assignedIds.TryGetValue(id, out i))
				{
					var assignments = _inputAssignments[i];
					var found = (from entry in assignments where entry.Value == id select entry.Key).FirstOrDefault();
					assignments.Remove(found);

					if(assignments.Count <= 0) // Removed the last of this type, unsubscribe
						unsubscribe(i);
					_assignedIds.Remove(id);
				}
			}
		}

		/// <summary>
		/// Clears all assigned inputs
		/// </summary>
		public virtual void Clear ()
		{
			lock(_inputAssignments)
				for(var i = 0; i < _inputAssignments.Length; ++i)
					if(_inputAssignments[i] != null) // Remove listeners
						unsubscribe(i);
		}

		/// <summary>
		/// Unsubscribes from events corresponding to the input type and sets the dictionary for it to null
		/// </summary>
		/// <param name="i">Input type (and index)</param>
		private void unsubscribe (int i)
		{
			switch((InputType)i)
			{
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
			_inputAssignments[i] = null;
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

		/// <summary>
		/// Attempts to get the assigned ID for a corresponding input
		/// </summary>
		/// <param name="input">Values for the input</param>
		/// <param name="id">ID assigned to the input</param>
		/// <returns>True if an ID is assigned</returns>
		private bool tryGetInputId (InputDescriptor input, out int id)
		{
			lock(_inputAssignments)
			{
				var i = (int)input.Type;
				var assignments = _inputAssignments[i];
				if(assignments != null) // There are indices for this input type
					return assignments.TryGetValue(input.Value, out id);
			}

			id = -1;
			return false;
		}

		private void Keyboard_KeyPress (object sender, KeyboardEventArgs e)
		{
			var input = new InputDescriptor(InputType.Keyboard, (int)e.Key);
			int id;
			if(tryGetInputId(input, out id))
			{// This is an assigned input
				_inputEventArgs.Input = input;
				_inputEventArgs.Id    = id;
				OnInputStarted(_inputEventArgs);
			}
		}

		private void Keyboard_KeyRelease (object sender, KeyboardEventArgs e)
		{
			var input = new InputDescriptor(InputType.Keyboard, (int)e.Key);
			int id;
			if(tryGetInputId(input, out id))
			{// This is an assigned input
				_inputEventArgs.Input = input;
				_inputEventArgs.Id    = id;
				OnInputEnded(_inputEventArgs);
			}
		}

		private void Mouse_Move (object sender, MouseEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Mouse_Press (object sender, MouseEventArgs e)
		{
			var input = new InputDescriptor(InputType.Mouse, (int)e.Buttons);
			int id;
			if(tryGetInputId(input, out id))
			{// This is an assigned input
				_inputEventArgs.Input = input;
				_inputEventArgs.Id    = id;
				OnInputStarted(_inputEventArgs);
			}
		}

		private void Mouse_Release (object sender, MouseEventArgs e)
		{
			var input = new InputDescriptor(InputType.Mouse, (int)e.Buttons);
			int id;
			if(tryGetInputId(input, out id))
			{// This is an assigned input
				_inputEventArgs.Input = input;
				_inputEventArgs.Id    = id;
				OnInputEnded(_inputEventArgs);
			}
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
			Json.Converters.Add(new JsonInputDescriptorConverter());
		}
		#endregion

		#region Save and load

		/// <summary>
		/// Saves the input scheme (control mapping/key bindings) to a file
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
		public static TScheme Load<TScheme> (string path) where TScheme : InputScheme
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
				Clear();
			}
		}
		#endregion
	}
}
