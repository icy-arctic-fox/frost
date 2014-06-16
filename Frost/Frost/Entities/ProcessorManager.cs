using System;
using System.Collections.Generic;
using System.Linq;

namespace Frost.Entities
{
	/// <summary>
	/// Runs entities in batches through processors
	/// </summary>
	public class ProcessorManager
	{
		private readonly List<ProcessorSet<IUpdateProcessor>> _updateProcessors = new List<ProcessorSet<IUpdateProcessor>>();
		private readonly List<ProcessorSet<IRenderProcessor>> _renderProcessors = new List<ProcessorSet<IRenderProcessor>>();
		private readonly EntityGroup _allEntities = new EntityGroup();

		/// <summary>
		/// Adds a processor that will process entities during logic updates
		/// </summary>
		/// <param name="processor">Processor to add</param>
		/// <exception cref="ArgumentNullException">The <paramref name="processor"/> to add can't be null.</exception>
		public void AddUpdateProcessor (IUpdateProcessor processor)
		{
			if(processor == null)
				throw new ArgumentNullException("processor");

			lock(_allEntities)
			{
				// Get the entities that the processor can handle
				var entities = _allEntities.Where(processor.CanProcess);

				// Add the processor to the collection
				var set = new ProcessorSet<IUpdateProcessor>(processor, entities);
				lock(_updateProcessors)
					_updateProcessors.Add(set);
			}
		}

		/// <summary>
		/// Adds a processor that will process entities during frame renders
		/// </summary>
		/// <param name="processor">processor to add</param>
		/// <exception cref="ArgumentNullException">The <paramref name="processor"/> to add can't be null.</exception>
		public void AddRenderProcessor (IRenderProcessor processor)
		{
			if(processor == null)
				throw new ArgumentNullException("processor");
			
			lock(_allEntities)
			{
				// Get the entities that the processor can handle
				var entities = _allEntities.Where(processor.CanProcess);

				// Add the processor to the collection
				var set = new ProcessorSet<IRenderProcessor>(processor, entities);
				lock(_renderProcessors)
					_renderProcessors.Add(set);
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

			lock(_updateProcessors)
				for(var i = 0; i < _updateProcessors.Count; ++i)
				{
					var set = _updateProcessors[i];
					addToProcessor(set.Processor, entity, set.Entities);
				}

			lock(_renderProcessors)
				for(var i = 0; i < _renderProcessors.Count; ++i)
				{
					var set = _renderProcessors[i];
					addToProcessor(set.Processor, entity, set.Entities);
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

			lock(_updateProcessors)
				for(var i = 0; i < _updateProcessors.Count; ++i)
				{
					var set = _updateProcessors[i];
					set.Entities.Remove(entity);
				}

			lock(_renderProcessors)
				for(var i = 0; i < _renderProcessors.Count; ++i)
				{
					var set = _renderProcessors[i];
					set.Entities.Remove(entity);
				}
		}

		/// <summary>
		/// Adds an entity to a processor set if the processor supports it
		/// </summary>
		/// <param name="processor">Processor to check</param>
		/// <param name="entity">Entity to add</param>
		/// <param name="entityList">List of existing entities</param>
		private static void addToProcessor (IProcessor processor, Entity entity, EntityGroup entityList)
		{
			if(processor.CanProcess(entity))
				lock(entityList)
					entityList.Add(entity);
		}

		/// <summary>
		/// Updates all entities by having the processors process them
		/// </summary>
		/// <param name="stepArgs">Update information</param>
		public void Step (FrameStepEventArgs stepArgs)
		{
			lock(_updateProcessors)
				for(var i = 0; i < _updateProcessors.Count; ++i)
				{
					var set = _updateProcessors[i];
					var processor = set.Processor;
					var entities  = set.Entities;
					runUpdateProcessor(processor, entities, stepArgs);
				}
		}

		/// <summary>
		/// Processes all entities for an update processor
		/// </summary>
		/// <param name="processor">Processor to execute</param>
		/// <param name="entities">Entities to run through the processor</param>
		/// <param name="args">Update information</param>
		private static void runUpdateProcessor (IUpdateProcessor processor, EntityGroup entities, FrameStepEventArgs args)
		{
			lock(entities) // TODO: Use read-write lock
				for(var i = 0; i < entities.Count; ++i)
					processor.Process(entities[i], args);
		}

		/// <summary>
		/// Renders all entities by having the processors process them
		/// </summary>
		/// <param name="drawArgs">Render information</param>
		public void Draw (FrameDrawEventArgs drawArgs)
		{
			lock(_renderProcessors)
				for(var i = 0; i < _renderProcessors.Count; ++i)
				{
					var set = _renderProcessors[i];
					var processor = set.Processor;
					var entities  = set.Entities;
					runRenderProcessor(processor, entities, drawArgs);
				}
		}

		/// <summary>
		/// Processes all entities for a render processor
		/// </summary>
		/// <param name="processor">Processor to execute</param>
		/// <param name="entities">Entities to run through the processor</param>
		/// <param name="args">Render information</param>
		private static void runRenderProcessor (IRenderProcessor processor, EntityGroup entities, FrameDrawEventArgs args)
		{
			lock(entities)
				for(var i = 0; i < entities.Count; ++i)
					processor.Process(entities[i], args);
		}

		/// <summary>
		/// Associates a collection of entities with a processor that can handle them
		/// </summary>
		private struct ProcessorSet<T> where T : IProcessor
		{
			/// <summary>
			/// Processor that will handle the entities
			/// </summary>
			public readonly T Processor;

			/// <summary>
			/// Entities that the processor can process
			/// </summary>
			public readonly EntityGroup Entities;

			/// <summary>
			/// Creates a processor set
			/// </summary>
			/// <param name="processor">Processor that will handle the entities</param>
			/// <param name="entities">Entities that the processor can process</param>
			public ProcessorSet (T processor, IEnumerable<Entity> entities)
			{
				Processor = processor;
				Entities  = new EntityGroup(entities);
			}
		}
	}
}
