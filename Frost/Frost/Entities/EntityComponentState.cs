using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frost.Utility;

namespace Frost.Entities
{
	public abstract class EntityComponentState
	{
		/// <summary>
		/// Triggered when any of the properties of a component's state are changed
		/// </summary>
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

		/// <summary>
		/// This method should be called whenever a component's state's property has changed
		/// </summary>
		/// <param name="args">Event arguments</param>
		/// <remarks>This method calls the <see cref="PropertyChanged"/> event.</remarks>
		protected virtual void OnPropertyChanged (PropertyChangedEventArgs args)
		{
			PropertyChanged.NotifySubscribers(this, args);
		}

		public abstract void Step (EntityComponentState prevState);
	}
}
