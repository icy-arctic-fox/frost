using System;
using Frost.Utility;
using Newtonsoft.Json;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Simplistic four-button controller with arrows
	/// </summary>
	public class BasicController : Controller
	{
		private const int AcceptButtonId    = 0;
		private const int CancelButtonId    = 1;
		private const int PrimaryButtonId   = 2;
		private const int SecondaryButtonId = 3;

		private const int UpArrowId    = 4;
		private const int DownArrowId  = 5;
		private const int LeftArrowId  = 6;
		private const int RightArrowId = 7;

		#region Buttons
		#region Accept button

		/// <summary>
		/// Triggered when the accept button is pressed
		/// </summary>
		public event EventHandler<InputEventArgs> AcceptButtonPressed;

		/// <summary>
		/// Triggered when the accept button is released
		/// </summary>
		public event EventHandler<InputEventArgs> AcceptButtonReleased;

		private InputDescriptor _accept = InputDescriptor.Unassigned;

		/// <summary>
		/// Accept, confirm, or advance button
		/// </summary>
		[JsonProperty("accept")]
		public InputDescriptor AcceptButton
		{
			get { return _accept; }
			set
			{
				_accept = value;
				AssignInput(AcceptButtonId, value);
			}
		}
		#endregion

		#region Cancel button

		/// <summary>
		/// Triggered when the cancel button is pressed
		/// </summary>
		public event EventHandler<InputEventArgs> CancelButtonPressed;

		/// <summary>
		/// Triggered when the cancel button is released
		/// </summary>
		public event EventHandler<InputEventArgs> CancelButtonReleased;

		private InputDescriptor _cancel = InputDescriptor.Unassigned;

		/// <summary>
		/// Cancel or confirm button
		/// </summary>
		[JsonProperty("cancel")]
		public InputDescriptor CancelButton
		{
			get { return _cancel; }
			set
			{
				_cancel = value;
				AssignInput(CancelButtonId, value);
			}
		}
		#endregion

		#region Primary button

		/// <summary>
		/// Triggered when the primary button is pressed
		/// </summary>
		public event EventHandler<InputEventArgs> PrimaryButtonPressed;

		/// <summary>
		/// Triggered when the primary button is released
		/// </summary>
		public event EventHandler<InputEventArgs> PrimaryButtonReleased;

		private InputDescriptor _primary = InputDescriptor.Unassigned;

		/// <summary>
		/// Primary action button
		/// </summary>
		[JsonProperty("primary")]
		public InputDescriptor PrimaryButton
		{
			get { return _primary; }
			set
			{
				_primary = value;
				AssignInput(PrimaryButtonId, value);
			}
		}
		#endregion

		#region Secondary button

		/// <summary>
		/// Triggered when the secondary button is pressed
		/// </summary>
		public event EventHandler<InputEventArgs> SecondaryButtonPressed;

		/// <summary>
		/// Triggered when the secondary button is released
		/// </summary>
		public event EventHandler<InputEventArgs> SecondaryButtonReleased;

		private InputDescriptor _secondary = InputDescriptor.Unassigned;

		/// <summary>
		/// Secondary action button
		/// </summary>
		[JsonProperty("secondary")]
		public InputDescriptor SecondaryButton
		{
			get { return _secondary; }
			set
			{
				_secondary = value;
				AssignInput(SecondaryButtonId, value);
			}
		}
		#endregion
		#endregion

		#region Arrows
		#region Up arrow

		/// <summary>
		/// Triggered when the up arrow is pressed
		/// </summary>
		public event EventHandler<InputEventArgs> UpArrowPressed;

		/// <summary>
		/// Triggered when the up arrow is released
		/// </summary>
		public event EventHandler<InputEventArgs> UpArrowReleased;

		private InputDescriptor _up = InputDescriptor.Unassigned;

		/// <summary>
		/// Up arrow button
		/// </summary>
		[JsonProperty("up")]
		public InputDescriptor UpArrow
		{
			get { return _up; }
			set
			{
				_up = value;
				AssignInput(UpArrowId, value);
			}
		}
		#endregion

		#region Down arrow

		/// <summary>
		/// Triggered when the down arrow is pressed
		/// </summary>
		public event EventHandler<InputEventArgs> DownArrowPressed;

		/// <summary>
		/// Triggered when the down arrow is released
		/// </summary>
		public event EventHandler<InputEventArgs> DownArrowReleased;

		private InputDescriptor _down = InputDescriptor.Unassigned;

		/// <summary>
		/// Down arrow button
		/// </summary>
		[JsonProperty("down")]
		public InputDescriptor DownArrow
		{
			get { return _down; }
			set
			{
				_down = value;
				AssignInput(DownArrowId, value);
			}
		}
		#endregion

		#region Left arrow

		/// <summary>
		/// Triggered when the left arrow is pressed
		/// </summary>
		public event EventHandler<InputEventArgs> LeftArrowPressed;

		/// <summary>
		/// Triggered when the left arrow is released
		/// </summary>
		public event EventHandler<InputEventArgs> LeftArrowReleased;

		private InputDescriptor _left = InputDescriptor.Unassigned;

		/// <summary>
		/// Left arrow button
		/// </summary>
		[JsonProperty("left")]
		public InputDescriptor LeftArrow
		{
			get { return _left; }
			set
			{
				_left = value;
				AssignInput(LeftArrowId, value);
			}
		}
		#endregion

		#region Right arrow

		/// <summary>
		/// Triggered when the right arrow is pressed
		/// </summary>
		public event EventHandler<InputEventArgs> RightArrowPressed;

		/// <summary>
		/// Triggered when the right arrow is released
		/// </summary>
		public event EventHandler<InputEventArgs> RightArrowReleased;

		private InputDescriptor _right = InputDescriptor.Unassigned;

		/// <summary>
		/// Right arrow button
		/// </summary>
		[JsonProperty("right")]
		public InputDescriptor RightArrow
		{
			get { return _right; }
			set
			{
				_right = value;
				AssignInput(RightArrowId, value);
			}
		}
		#endregion
		#endregion

		/// <summary>
		/// Unassigns inputs from all buttons and arrows
		/// </summary>
		public override void Clear ()
		{
			base.Clear();

			_accept    = InputDescriptor.Unassigned;
			_cancel    = InputDescriptor.Unassigned;
			_primary   = InputDescriptor.Unassigned;
			_secondary = InputDescriptor.Unassigned;

			_up    = InputDescriptor.Unassigned;
			_down  = InputDescriptor.Unassigned;
			_left  = InputDescriptor.Unassigned;
			_right = InputDescriptor.Unassigned;
		}

		/// <summary>
		/// Called when any input is detected
		/// </summary>
		/// <param name="args">Information about the input</param>
		protected override void OnInputStarted (InputEventArgs args)
		{
			base.OnInputStarted(args);

			EventHandler<InputEventArgs> ev;
			switch(args.Id)
			{
			case AcceptButtonId:
				ev = AcceptButtonPressed;
				break;
			case CancelButtonId:
				ev = CancelButtonPressed;
				break;
			case PrimaryButtonId:
				ev = PrimaryButtonPressed;
				break;
			case SecondaryButtonId:
				ev = SecondaryButtonPressed;
				break;
			case UpArrowId:
				ev = UpArrowPressed;
				break;
			case DownArrowId:
				ev = DownArrowPressed;
				break;
			case LeftArrowId:
				ev = LeftArrowPressed;
				break;
			case RightArrowId:
				ev = RightArrowPressed;
				break;
			default:
				return;
			}
			ev.NotifySubscribers(this, args);
		}

		/// <summary>
		/// Called when some input is no longer detected
		/// </summary>
		/// <param name="args">Information about the input</param>
		protected override void OnInputEnded (InputEventArgs args)
		{
			base.OnInputEnded(args);

			EventHandler<InputEventArgs> ev;
			switch(args.Id)
			{
			case AcceptButtonId:
				ev = AcceptButtonReleased;
				break;
			case CancelButtonId:
				ev = CancelButtonReleased;
				break;
			case PrimaryButtonId:
				ev = PrimaryButtonReleased;
				break;
			case SecondaryButtonId:
				ev = SecondaryButtonReleased;
				break;
			case UpArrowId:
				ev = UpArrowReleased;
				break;
			case DownArrowId:
				ev = DownArrowReleased;
				break;
			case LeftArrowId:
				ev = LeftArrowReleased;
				break;
			case RightArrowId:
				ev = RightArrowReleased;
				break;
			default:
				return;
			}
			ev.NotifySubscribers(this, args);
		}
	}
}
