using System;
using Frost.Utility;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Simplistic four-button controller with arrows
	/// </summary>
	public class BasicController : InputScheme
	{
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

		/// <summary>
		/// Accept, confirm, or advance button
		/// </summary>
		public InputDescriptor AcceptButton { get; private set; }
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

		/// <summary>
		/// Cancel or confirm button
		/// </summary>
		public InputDescriptor CancelButton { get; private set; }
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

		/// <summary>
		/// Primary action button
		/// </summary>
		public InputDescriptor PrimaryButton { get; private set; }
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

		/// <summary>
		/// Secondary action button
		/// </summary>
		public InputDescriptor SecondaryButton { get; private set; }
		#endregion
		#endregion

		/// <summary>
		/// Called when any input is detected
		/// </summary>
		/// <param name="args">Information about the input</param>
		protected override void OnInputStarted (InputEventArgs args)
		{
			base.OnInputStarted(args);

			// TODO: Is there possibly a better way to do this?
			if(args.Input == AcceptButton)
				AcceptButtonPressed.NotifySubscribers(this, args);
			else if(args.Input == CancelButton)
				CancelButtonPressed.NotifySubscribers(this, args);
			else if(args.Input == PrimaryButton)
				PrimaryButtonPressed.NotifySubscribers(this, args);
			else if(args.Input == SecondaryButton)
				SecondaryButtonPressed.NotifySubscribers(this, args);
		}

		/// <summary>
		/// Called when some input is no longer detected
		/// </summary>
		/// <param name="args">Information about the input</param>
		protected override void OnInputEnded (InputEventArgs args)
		{
			base.OnInputEnded(args);

			if(args.Input == AcceptButton)
				AcceptButtonReleased.NotifySubscribers(this, args);
			else if(args.Input == CancelButton)
				CancelButtonReleased.NotifySubscribers(this, args);
			else if(args.Input == PrimaryButton)
				PrimaryButtonReleased.NotifySubscribers(this, args);
			else if(args.Input == SecondaryButton)
				SecondaryButtonReleased.NotifySubscribers(this, args);
		}
	}
}
