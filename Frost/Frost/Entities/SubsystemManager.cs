﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Frost.Entities
{
	/// <summary>
	/// Processes all entities through subsystems
	/// </summary>
	class SubsystemManager
	{
		private readonly List<SubsystemSet> _updateSystems = new List<SubsystemSet>(),
											_renderSystems = new List<SubsystemSet>();

		private readonly List<Entity> _allEntities = new List<Entity>();

		/// <summary>
		/// Adds a subsystem that will process entities during logic updates
		/// </summary>
		/// <param name="subsystem">Subsystem to add</param>
		/// <exception cref="ArgumentNullException">The <paramref name="subsystem"/> to add can't be null.</exception>
		public void AddUpdateSubsystem (ISubsystem subsystem)
		{
			if(subsystem == null)
				throw new ArgumentNullException("subsystem");

			// Get the entities that the subsystem can process
			List<Entity> entities;
			lock(_allEntities)
				entities = _allEntities.Where(subsystem.CanProcess).ToList();

			// Add the subsystem to the collection
			var set = new SubsystemSet(subsystem, entities);
			lock(_updateSystems)
				_updateSystems.Add(set);
		}

		/// <summary>
		/// Adds a subsystem that will process entities during frame renders
		/// </summary>
		/// <param name="subsystem">Subsystem to add</param>
		/// <exception cref="ArgumentNullException">The <paramref name="subsystem"/> to add can't be null.</exception>
		public void AddRenderSubsystem (ISubsystem subsystem)
		{
			if(subsystem == null)
				throw new ArgumentNullException("subsystem");

			// Get the entities that the subsystem can process
			List<Entity> entities;
			lock(_allEntities)
				entities = _allEntities.Where(subsystem.CanProcess).ToList();

			// Add the subsystem to the collection
			var set = new SubsystemSet(subsystem, entities);
			lock(_renderSystems)
				_renderSystems.Add(set);
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
			/// <param name="entities">Entities that the subsystem can process</param>
			public SubsystemSet (ISubsystem subsystem, List<Entity> entities)
			{
				Subsystem = subsystem;
				Entities  = entities;
			}
		}
	}
}