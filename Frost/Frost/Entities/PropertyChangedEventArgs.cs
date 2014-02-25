using System;

namespace Frost.Entities
{
	/// <summary>
	/// Describes an event when a component's property has changed
	/// </summary>
	public class PropertyChangedEventArgs : EventArgs
	{
		private readonly string _prop;

		/// <summary>
		/// Name of the property that changed
		/// </summary>
		public string Property
		{
			get { return _prop; }
		}

		/// <summary>
		/// Creates new property changed event arguments
		/// </summary>
		/// <param name="prop">Name of the property that changed</param>
		public PropertyChangedEventArgs (string prop)
		{
			if(String.IsNullOrWhiteSpace(prop))
				throw new ArgumentNullException("prop", "The name of the property that changed can't be null.");
			_prop = prop;
		}
	}
}
