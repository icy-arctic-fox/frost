namespace Frost.Entities
{
	/// <summary>
	/// Base game object that consists of a unique ID and component information
	/// </summary>
	public class Entity
	{
		private ulong _id;

		/// <summary>
		/// Unique identification number of the entity.
		/// This value can be used to reference an entity from anywhere.
		/// An entity that has an ID of 0 has not been registered and should not be openly used.
		/// </summary>
		public ulong Id
		{
			get { return _id; }
		}

		/// <summary>
		/// Indicates whether the entity has been registered.
		/// An unregistered entity will have an <see cref="Id"/> of 0.
		/// </summary>
		public bool Registered
		{
			get { return _id != 0L; }
		}
	}
}
