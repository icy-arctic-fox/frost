using System;

namespace Frost.Entities
{
	/// <summary>
	/// Draws 2D graphical entities
	/// </summary>
	public class Graphics2DEntityProcessor : IEntityRenderer
	{
		private readonly EntityComponentMap<Position2DEntityComponent> _position2DMap;
		private readonly EntityComponentMap<TexturedEntityComponent> _texturedMap;

		/// <summary>
		/// Creates a new 2D entity graphics processor
		/// </summary>
		/// <param name="manager">Manager containing the entities to process</param>
		/// <exception cref="ArgumentNullException">The entity <paramref name="manager"/> can't be null.</exception>
		public Graphics2DEntityProcessor (Entity.Manager manager)
		{
			if(manager == null)
				throw new ArgumentNullException("manager");

			_position2DMap = manager.GetComponentMap<Position2DEntityComponent>();
			_texturedMap   = manager.GetComponentMap<TexturedEntityComponent>();
		}

		/// <summary>
		/// Draws a 2D entity
		/// </summary>
		/// <param name="e">Entity to process</param>
		/// <param name="stateIndex">Index of the entity state to draw</param>
		/// <param name="t">Amount of interpolation</param>
		public void DrawEntity (Entity e, int stateIndex, double t)
		{
			var pos = _position2DMap.GetComponent(e);
			var tex = _texturedMap.GetComponent(e);

			if(pos != null && tex != null)
			{// Process the entity if it has the components
				throw new NotImplementedException();
			}
		}
	}
}
