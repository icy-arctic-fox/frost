using Frost.Display;
using Frost.Entities;
using Frost.Graphics;

namespace Frost
{
	/// <summary>
	/// Logically separate section of the game
	/// </summary>
	public abstract class Scene : IStepable, IRenderable
	{
		private SceneManager _sceneManager;

		/// <summary>
		/// Manager that owns the scene
		/// </summary>
		protected SceneManager ParentSceneManager
		{
			get { return _sceneManager; }
		}

		/// <summary>
		/// Sets the scene manager that is the scene's parent
		/// </summary>
		/// <param name="manager">Parent scene manager</param>
		internal void SetParentManager (SceneManager manager)
		{
			_sceneManager = manager;
		}

		private readonly EntityManager _entityManager = new EntityManager();

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

		/// <summary>
		/// Visible name of the scene
		/// </summary>
		/// <remarks>This property is used instead of reflection (<see cref="System.Type.FullName"/>) because it's faster.</remarks>
		public abstract string Name { get; }

		/// <summary>
		/// Indicates whether scenes below this one can be processed during the same frame
		/// </summary>
		public abstract bool AllowFallthrough { get; }

		/// <summary>
		/// Updates the state of the scene by a single step
		/// </summary>
		/// <param name="args">Update information</param>
		/// <remarks>The only game state that should be modified during this process is the state indicated by <see cref="FrameStepEventArgs.NextStateIndex"/>.
		/// The state indicated by <see cref="FrameStepEventArgs.PreviousStateIndex"/> can be used for reference (if needed), but should not be modified.
		/// Modifying any other game state info during this process could corrupt the game state.</remarks>
		public virtual void Step (FrameStepEventArgs args)
		{
			// TODO: Update all updatable entities
		}

		/// <summary>
		/// Draws the state of the scene
		/// </summary>
		/// <param name="display">Display to draw the object's state onto</param>
		/// <param name="args">Render information</param>
		/// <remarks>None of the game states should be modified by this process - including the state indicated by <see cref="FrameDrawEventArgs.StateIndex"/>.
		/// Modifying the game state info during this process would corrupt the game state.</remarks>
		public virtual void Draw (IDisplay display, FrameDrawEventArgs args)
		{
			// TODO: Render all drawable entities
		}
	}
}
