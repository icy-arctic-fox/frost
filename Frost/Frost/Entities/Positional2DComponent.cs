namespace Frost.Entities
{
	/// <summary>
	/// Information about an entity's location in 2D space
	/// </summary>
	public class Positional2DComponent : IEntityComponent
	{
		/// <summary>
		/// Offset of the entity from the origin along the x-axis
		/// </summary>
		public float X { get; set; }

		/// <summary>
		/// Offset of the entity from the origin along the y-axis
		/// </summary>
		public float Y { get; set; }
	}
}
