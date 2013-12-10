﻿using System;
using System.ComponentModel;

namespace Frost.Utility
{
	/// <summary>
	/// Useful methods for delegates and events
	/// </summary>
	public static class DelegateUtility
	{
		/// <summary>
		/// Notifies subscribers that an event has occurred
		/// </summary>
		/// <typeparam name="TEventArgs">Type of event arguments</typeparam>
		/// <param name="ev">Event that occurred</param>
		/// <param name="sender">Object that the event occurred in (this)</param>
		/// <param name="args">Arguments related to the event</param>
		/// <remarks>If <paramref name="args"/> is <see cref="ICancellable"/> (or <see cref="CancellableEventArgs"/>),
		/// subscribers will stop being called as soon as the event is cancelled.</remarks>
		/// <seealso cref="ICancellable"/>
		/// <seealso cref="CancellableEventArgs"/>
		public static void NotifySubscribers<TEventArgs> (this EventHandler<TEventArgs> ev, object sender, TEventArgs args) where TEventArgs : EventArgs
		{
#if DEBUG
			if(null == args)
				throw new ArgumentNullException("args", "The arguments passed to an event can't be null.");
#endif

			if(null != ev)
			{// There are subscribers
				var list = ev.GetInvocationList();
				var cancelArgs = args as ICancellable;

				if(cancelArgs != null)
				{// The event could be cancelled
					if(!cancelArgs.IsCanceled)
					{// Only run this code if the event isn't already cancelled (for some reason...)
						foreach(var d in list)
						{
							CallDelegate(d, sender, args);
							if(cancelArgs.IsCanceled)
								break; // Event has been cancelled, stop calling subscribers
						}
					}
				}

				else // The event won't be cancelled
					foreach(var d in ev.GetInvocationList())
						CallDelegate(d, sender, args);
			}
		}

		/// <summary>
		/// Calls a delegate that might be on another thread
		/// </summary>
		/// <param name="d">Delegate to call</param>
		/// <param name="args">Arguments to pass to the delegate</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="d"/> is null - cannot call a null delegate</exception>
		/// <remarks>This method handles safely calling a delegate that might want to be invoked on another thread, such as a GUI.</remarks>
		public static void CallDelegate (this Delegate d, params object[] args)
		{
#if DEBUG
			if(null == d)
				throw new ArgumentNullException("d", "Cannot call a null delegate.");
#endif

			var sync = d.Target as ISynchronizeInvoke;
			if(sync == null) // Invoke on the current thread
				d.DynamicInvoke(args);
			else // Cross-thread invocation
				sync.BeginInvoke(d, args);
		}
	}
}
