using System;
using System.Collections.Generic;
using System.Linq;

namespace Frost.Entities
{
	/// <summary>
	/// Processes all entities through subsystems
	/// </summary>
	public class SubsystemManager
	{
		private readonly List<SubsystemSet<IUpdateSubsystem>> _updateSystems = new List<SubsystemSet<IUpdateSubsystem>>();
		private readonly List<SubsystemSet<IRenderSubsystem>> _renderSystems = new List<SubsystemSet<IRenderSubsystem>>();
		private readonly EntityGroup _allEntities = new EntityGroup();

		/// <summary>
		/// Adds a subsystem that will process entities during logic updates
		/// </summary>
		/// <param name="subsystem">Subsystem to add</param>
		/// <exception cref="ArgumentNullException">The <paramref name="subsystem"/> to add can't be null.</exception>
		public void AddUpdateSubsystem (IUpdateSubsystem subsystem)
		{
			if(subsystem == null)
				throw new ArgumentNullException("subsystem");

			lock(_allEntities)
			{
				// Get the entities that the subsystem can process
				var entities = _allEntities.Where(subsystem.CanProcess);

				// Add the subsystem to the collection
				var set = new SubsystemSet<IUpdateSubsystem>(subsystem, entities);
				lock(_updateSystems)
					_updateSystems.Add(set);
			}
		}

		/// <summary>
		/// Adds a subsystem that will process entities during frame renders
		/// </summary>
		/// <param name="subsystem">Subsystem to add</param>
		/// <exception cref="ArgumentNullException">The <paramref name="subsystem"/> to add can't be null.</exception>
		public void AddRenderSubsystem (IRenderSubsystem subsystem)
		{
			if(subsystem == null)
				throw new ArgumentNullException("subsystem");
			
			lock(_allEntities)
			{
				// Get the entities that the subsystem can process
				var entities = _allEntities.Where(subsystem.CanProcess);

				// Add the subsystem to the collection
				var set = new SubsystemSet<IRenderSubsystem>(subsystem, entities);
				lock(_renderSystems)
					_renderSystems.Add(set);
			}
		}

		/// <summary>
		/// Adds an entity to be processed by each update and rendering
		/// </summary>
		/// <param name="entity">Entity to add</param>
		/// <exception cref="ArgumentNullException">The <paramref name="entity"/> to add can't be null.</exception>
		public void AddEntity (Entity entity)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");

			lock(_allEntities)
				_allEntities.Add(entity);

			lock(_updateSystems)
				for(var i = 0; i < _updateSystems.Count; ++i)
				{
					var set = _updateSystems[i];
					addToSubsystem(set.Subsystem, entity, set.Entities);
				}

			lock(_renderSystems)
				for(var i = 0; i < _renderSystems.Count; ++i)
				{
					var set = _renderSystems[i];
					addToSubsystem(set.Subsystem, entity, set.Entities);
				}
		}

		/// <summary>
		/// Removes an entity from being processed by each update and rendering
		/// </summary>
		/// <param name="entity">Entity to remove</param>
		/// <exception cref="ArgumentNullException">The <paramref name="entity"/> to remove can't be null.</exception>
		public void RemoveEntity (Entity entity)
		{
			if(entity == null)
				throw new ArgumentNullException("entity");

			bool found;
			lock(_allEntities)
				found = _allEntities.Remove(entity);

			if(!found)
				return;  // Entity isn't tracked, don't waste time processing

			lock(_updateSystems)
				for(var i = 0; i < _updateSystems.Count; ++i)
				{
					var set = _updateSystems[i];
					set.Entities.Remove(entity);
				}

			lock(_renderSystems)
				for(var i = 0; i < _renderSystems.Count; ++i)
				{
					var set = _renderSystems[i];
					set.Entities.Remove(entity);
				}
		}

		/// <summary>
		/// Adds an entity to a subsystem set if the subsystem can process it
		/// </summary>
		/// <param name="subsystem">Subsystem to check</param>
		/// <param name="entity">Entity to add</param>
		/// <param name="entityList">List of existing entities</param>
		private static void addToSubsystem (ISubsystem subsystem, Entity entity, EntityGroup entityList)
		{
			if(subsystem.CanProcess(entity))
				lock(entityList)
					entityList.Add(entity);
		}

		/// <summary>
		/// Updates all entities by having the subsystems process them
		/// </summary>
		/// <param name="stepArgs">Update information</param>
		public void Step (FrameStepEventArgs stepArgs)
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
		private static void runUpdateSubsystem (IUpdateSubsystem subsystem, EntityGroup entities, FrameStepEventArgs args)
		{
			lock(entities) // TODO: Use read-write lock
				for(var i = 0; i < entities.Count; ++i)
					subsystem.Process(entities[i], args);
		}

		/// <summary>
		/// Renders all entities by having the subsystems process them
		/// </summary>
		/// <param name="drawArgs">Render information</param>
		public void Draw (FrameDrawEventArgs drawArgs)
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
		private static void runRenderSubsystem (IRenderSubsystem subsystem, EntityGroup entities, FrameDrawEventArgs args)
		{
			lock(entities)
				for(var i = 0; i < entities.Count; ++i)
					subsystem.Process(entities[i], args);
		}

		/// <summary>
		/// Associates a collection of entities with a subsystem that can process them
		/// </summary>
		private struct SubsystemSet<T> where T : ISubsystem
		{
			/// <summary>
			/// Subsystem that will process the entities
			/// </summary>
			public readonly T Subsystem;

			/// <summary>
			/// Entities that the subsystem can process
			/// </summary>
			public readonly EntityGroup Entities;

			/// <summary>
			/// Creates a subsystem set
			/// </summary>
			/// <param name="subsystem">Subsystem that will process the entities</param>
			/// <param name="entities">Entities that the subsystem can process</param>
			public SubsystemSet (T subsystem, IEnumerable<Entity> entities)
			{
				Subsystem = subsystem;
				Entities  = new EntityGroup(entities);
			}
		}
	}
}
