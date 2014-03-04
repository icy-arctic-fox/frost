namespace Frost.Entities
{
	/// <summary>
	/// Component that allows an entity to be represented in 2D space
	/// </summary>
	public class Position2DEntityComponent : EntityComponent
	{
		public class State : EntityComponentState
		{
			private float _x, _y;
		}
	}
}
