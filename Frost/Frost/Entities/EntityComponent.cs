using System;
using Frost.Utility;

namespace Frost.Entities
{
	/// <summary>
	/// Base class for all entity components.
	/// An entity component is a piece of functionality that an entity can have.
	/// </summary>
	public abstract class EntityComponent
	{
		/// <summary>
		/// Triggered when any of the properties of a component are changed
		/// </summary>
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

		/// <summary>
		/// This method should be called whenever a component's property has changed
		/// </summary>
		/// <param name="args">Event arguments</param>
		/// <remarks>This method calls the <see cref="PropertyChanged"/> event.</remarks>
		protected virtual void OnPropertyChanged (PropertyChangedEventArgs args)
		{
			PropertyChanged.NotifySubscribers(this, args);
		}
	}
}
