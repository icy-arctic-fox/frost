namespace Frost
{
	/// <summary>
	/// Base class for all entities.
	/// An entity is a fundamental object for a game.
	/// Entities can have any number of components (<see cref="EntityComponent"/>)
	/// which define characteristics of that entity.
	/// </summary>
	/// <seealso cref="EntityManager"/>
	public abstract class Entity
	{
		private readonly ulong _id;

		/// <summary>
		/// Unique entity identifier
		/// </summary>
		public ulong Id
		{
			get { return _id; }
		}

		/// <summary>
		/// Creates a new entity
		/// </summary>
		/// <param name="id">Unique identifier returned from <see cref="EntityManager.NextAvailableId"/></param>
		protected Entity (ulong id)
		{
			_id = id;
		}
	}
}
