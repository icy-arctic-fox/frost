﻿using System;
using Frost.Utility;
using Newtonsoft.Json;

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
		[JsonProperty("accept")]
		public InputDescriptor AcceptButton { get; set; }
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
		[JsonProperty("cancel")]
		public InputDescriptor CancelButton { get; set; }
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
		[JsonProperty("primary")]
		public InputDescriptor PrimaryButton { get; set; }
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
		[JsonProperty("secondary")]
		public InputDescriptor SecondaryButton { get; set; }
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

		/// <summary>
		/// Up arrow button
		/// </summary>
		[JsonProperty("up")]
		public InputDescriptor UpArrow { get; set; }
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

		/// <summary>
		/// Down arrow button
		/// </summary>
		[JsonProperty("down")]
		public InputDescriptor DownArrow { get; set; }
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

		/// <summary>
		/// Left arrow button
		/// </summary>
		[JsonProperty("left")]
		public InputDescriptor LeftArrow { get; set; }
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

		/// <summary>
		/// Right arrow button
		/// </summary>
		[JsonProperty("right")]
		public InputDescriptor RightArrow { get; set; }
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
			else if(args.Input == UpArrow)
				UpArrowPressed.NotifySubscribers(this, args);
			else if(args.Input == DownArrow)
				DownArrowPressed.NotifySubscribers(this, args);
			else if(args.Input == LeftArrow)
				LeftArrowPressed.NotifySubscribers(this, args);
			else if(args.Input == RightArrow)
				RightArrowPressed.NotifySubscribers(this, args);
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
			else if(args.Input == UpArrow)
				UpArrowReleased.NotifySubscribers(this, args);
			else if(args.Input == DownArrow)
				DownArrowReleased.NotifySubscribers(this, args);
			else if(args.Input == LeftArrow)
				LeftArrowReleased.NotifySubscribers(this, args);
			else if(args.Input == RightArrow)
				RightArrowReleased.NotifySubscribers(this, args);
		}
	}
}