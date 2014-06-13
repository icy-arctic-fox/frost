using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frost.Entities
{
	public class StaticImageSubsystem : IRenderSubsystem
	{
		/// <summary>
		/// Determines whether the subsystem can process the entity
		/// </summary>
		/// <param name="entity">Entity to check</param>
		/// <returns>True if the entity can be processed by the subsystem</returns>
		public bool CanProcess (Entity entity)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Processes a single entity
		/// </summary>
		/// <param name="entity">Entity to process</param>
		/// <param name="args">Render information</param>
		public void Process (Entity entity, FrameDrawEventArgs args)
		{
			throw new NotImplementedException();
		}
	}
}
