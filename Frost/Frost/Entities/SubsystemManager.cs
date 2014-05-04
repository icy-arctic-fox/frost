using System;
using System.Collections.Generic;

namespace Frost.Entities
{
	/// <summary>
	/// Processes all entities through subsystems
	/// </summary>
	class SubsystemManager
	{
		private readonly List<SubsystemSet> _updateSystems = new List<SubsystemSet>(),
											_renderSystems = new List<SubsystemSet>();

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
			lock(_updateSystems)
				for(var i = 0; i < _updateSystems.Count; ++i)
				{
					var set = _updateSystems[i];
					var subsystem = set.Subsystem;
					var entities  = set.Entities;
					runUpdateSubsystem(subsystem, entities, stepArgs);
				}
		}

		/// <summary>
		/// Processes all entities for an update subsystem
		/// </summary>
		/// <param name="subsystem">Subsystem to execute</param>
		/// <param name="entities">Entities to run through the subsystem</param>
		/// <param name="args">Update information</param>
		private static void runUpdateSubsystem (ISubsystem subsystem, List<Entity> entities, FrameStepEventArgs args)
		{
			lock(entities)
				for(var i = 0; i < entities.Count; ++i)
				{
					var entity = entities[i];
					subsystem.Process(entity);
				}
		}

		/// <summary>
		/// Renders all entities by having the subsystems process them
		/// </summary>
		/// <param name="drawArgs">Render information</param>
		public void Render (FrameDrawEventArgs drawArgs)
		{
			lock(_renderSystems)
				for(var i = 0; i < _renderSystems.Count; ++i)
				{
					var set = _renderSystems[i];
					var subsystem = set.Subsystem;
					var entities  = set.Entities;
					runRenderSubsystem(subsystem, entities, drawArgs);
				}
		}

		/// <summary>
		/// Processes all entities for a render subsystem
		/// </summary>
		/// <param name="subsystem">Subsystem to execute</param>
		/// <param name="entities">Entities to run through the subsystem</param>
		/// <param name="args">Render information</param>
		private static void runRenderSubsystem (ISubsystem subsystem, List<Entity> entities, FrameDrawEventArgs args)
		{
			lock(entities)
				for(var i = 0; i < entities.Count; ++i)
				{
					var entity = entities[i];
					subsystem.Process(entity);
				}
		}

		/// <summary>
		/// Associates a collection of entities with a subsystem that can process them
		/// </summary>
		private struct SubsystemSet
		{
			/// <summary>
			/// Subsystem that will process the entities
			/// </summary>
			public readonly ISubsystem Subsystem;

			/// <summary>
			/// Entities that the subsystem can process
			/// </summary>
			public readonly List<Entity> Entities;

			/// <summary>
			/// Creates a subsystem set
			/// </summary>
			/// <param name="subsystem">Subsystem that will process the entities</param>
			public SubsystemSet (ISubsystem subsystem)
			{
				Subsystem = subsystem;
				Entities  = new List<Entity>();
			}
		}
	}
}
