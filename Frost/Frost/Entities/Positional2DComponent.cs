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

		/// <summary>
		/// Compares another object to the current one to check if they're identical
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if <paramref name="obj"/> has identical values</returns>
		/// <remarks>Null objects and instances that aren't of type <see cref="Positional2DComponent"/> are considered not equal.</remarks>
		public override bool Equals (object obj)
		{
			if(ReferenceEquals(null, obj))
				return false;
			if(ReferenceEquals(this, obj))
				return true;
			var other = obj as Positional2DComponent;
			return other != null && Equals(other);
		}

		/// <summary>
		/// Compares another instance to the current one to check if they contain the same values
		/// </summary>
		/// <param name="other">Instance to compare against</param>
		/// <returns>True if <paramref name="other"/> has identical values</returns>
		protected bool Equals (Positional2DComponent other)
		{
			return X.Equals(other.X) && Y.Equals(other.Y);
		}

		/// <summary>
		/// Generates a hash code from the values in the component
		/// </summary>
		/// <returns>A hash code</returns>
		public override int GetHashCode ()
		{
			return unchecked((X.GetHashCode() * 397) ^ Y.GetHashCode());
		}
	}
}
