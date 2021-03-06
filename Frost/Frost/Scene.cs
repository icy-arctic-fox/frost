﻿using Frost.Entities;

namespace Frost
{
	/// <summary>
	/// Logically separate section of the game
	/// </summary>
	public abstract class Scene : IFrameUpdate, IFrameRender
	{
		/// <summary>
		/// Manager that owns the scene
		/// </summary>
		protected SceneManager Owner { get; private set; }

		/// <summary>
		/// Sets the scene manager that is the scene's owner
		/// </summary>
		/// <param name="manager">Parent scene manager</param>
		internal void SetOwner (SceneManager manager)
		{
			Owner = manager;
		}

		/// <summary>
		/// Visible name of the scene
		/// </summary>
		/// <remarks>This property is used instead of reflection (<see cref="System.Type.FullName"/>) because it's faster.</remarks>
		public abstract string Name { get; }

		/// <summary>
		/// Indicates whether scenes below this one can be processed during the same frame
		/// </summary>
		public virtual bool AllowFallthrough
		{
			get { return false; }
		}

		/// <summary>
		/// Creates the base of the scene
		/// </summary>
		protected Scene ()
		{
			// Listen for entities being added and removed
			_entityManager.EntityRegistered   += entityRegistered;
			_entityManager.EntityDeregistered += entityDeregistered;
		}

		/// <summary>
		/// Called when the state of the scene should be updated by a single step
		/// </summary>
		/// <param name="args">Update information</param>
		/// <remarks>The only game state that should be modified during this process is the state indicated by <see cref="FrameStepEventArgs.NextStateIndex"/>.
		/// The state indicated by <see cref="FrameStepEventArgs.PreviousStateIndex"/> can be used for reference (if needed), but should not be modified.
		/// Modifying any other game state info during this process could corrupt the game state.</remarks>
		protected virtual void OnStep (FrameStepEventArgs args)
		{
			_processorManager.Step(args);
		}

		/// <summary>
		/// Updates the state of the scene by a single step and moves to the next frame
		/// </summary>
		/// <param name="args">Update information</param>
		public void Update (FrameStepEventArgs args)
		{
			OnStep(args);
		}

		/// <summary>
		/// Called when the scene should be drawn to the display
		/// </summary>
		/// <param name="args">Render information</param>
		/// <remarks>None of the game states should be modified by this process - including the state indicated by <see cref="FrameDrawEventArgs.StateIndex"/>.
		/// Modifying the game state info during this process would corrupt the game state.</remarks>
		protected virtual void OnDraw (FrameDrawEventArgs args)
		{
			_processorManager.Draw(args);
		}

		/// <summary>
		/// Renders the state of the scene
		/// </summary>
		/// <param name="args">Render information</param>
		public void Render (FrameDrawEventArgs args)
		{
			OnDraw(args);
		}

		#region Processors and entities

		private readonly ProcessorManager _processorManager = new ProcessorManager();
		private readonly EntityManager _entityManager = new EntityManager();

		/// <summary>
		/// Processors that handle entities in the scene
		/// </summary>
		protected ProcessorManager Processors
		{
			get { return _processorManager; }
		}

		/// <summary>
		/// Called when an entity is registered in a scene
		/// </summary>
		/// <param name="sender">Entity manager</param>
		/// <param name="e">Event arguments</param>
		private void entityRegistered (object sender, EntityEventArgs e)
		{
			_processorManager.AddEntity(e.Entity);
		}

		/// <summary>
		/// Called when an entity is deregistered in a scene
		/// </summary>
		/// <param name="sender">Entity manager</param>
		/// <param name="e">Event arguments</param>
		private void entityDeregistered (object sender, EntityEventArgs e)
		{
			_processorManager.RemoveEntity(e.Entity);
		}

		/// <summary>
		/// Tracks entities used in the current scene
		/// </summary>
		protected EntityManager Entities
		{
			get { return _entityManager; }
		}

		/// <summary>
		/// Tracks entities used in the current scene
		/// </summary>
		internal EntityManager EntityManager
		{
			get { return _entityManager; }
		}
		#endregion
	}
}
