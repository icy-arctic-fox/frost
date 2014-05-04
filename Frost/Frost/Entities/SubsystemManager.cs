using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frost.Entities
{
	/// <summary>
	/// Processes all entities through subsystems
	/// </summary>
	class SubsystemManager
	{
		/// <summary>
		/// Adds a subsystem that will process entities during logic updates
		/// </summary>
		/// <param name="subsystem">Subsystem to add</param>
		public void AddUpdateSubsystem (ISubsystem subsystem)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds a subsystem that will process entities during frame renders
		/// </summary>
		/// <param name="subsystem">Subsystem to add</param>
		public void AddRenderSubsystem (ISubsystem subsystem)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds an entity to be processed by each update and rendering
		/// </summary>
		/// <param name="entity">Entity to add</param>
		public void AddEntity (Entity entity)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates all entities by having the subsystems process them
		/// </summary>
		/// <param name="stepArgs">Update information</param>
		public void Update (FrameStepEventArgs stepArgs)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Renders all entities by having the subsystems process them
		/// </summary>
		/// <param name="drawArgs">Render information</param>
		public void Render (FrameDrawEventArgs drawArgs)
		{
			throw new NotImplementedException();
		}
	}
}
