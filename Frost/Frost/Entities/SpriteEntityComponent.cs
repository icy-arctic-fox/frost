using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frost.Graphics;
using Frost.Logic;

namespace Frost.Entities
{
	/// <summary>
	/// Functionality that allows an entity to be visibly represented by a 2D sprite
	/// </summary>
	public class SpriteEntityComponent : EntityComponent<SpriteEntityComponentState>
	{
		private readonly Texture _texture;

		public SpriteEntityComponent (Texture texture)
		{
			// TODO: Check for null
			_texture = texture;
		}

		public override StateSet<SpriteEntityComponentState> GetInitialStates ()
		{
			throw new NotImplementedException();
		}
	}

	public class SpriteEntityComponentState : EntityComponentState<SpriteEntityComponentState>
	{
		public override void Step (SpriteEntityComponentState prevState)
		{
			throw new NotImplementedException();
		}
	}
}
