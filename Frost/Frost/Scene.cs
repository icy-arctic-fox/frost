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
		/// Visible name of the scene
		/// </summary>
		/// <remarks>This property is used instead of reflection (<see cref="System.Type.FullName"/>) because it's faster.</remarks>
		public abstract string Name { get; }

		/// <summary>
		/// Updates the state of the scene by a single step
		/// </summary>
		/// <param name="prev">Index of the previous state that was updated</param>
		/// <param name="next">Index of the next state that should be updated</param>
		/// <remarks>The only game state that should be modified during this process is the state indicated by <paramref name="next"/>.
		/// The state indicated by <paramref name="prev"/> can be used for reference (if needed), but should not be modified.
		/// Modifying any other game state info during this process would corrupt the game state.</remarks>
		public virtual void Step (int prev, int next)
		{
			// TODO: Update all updatable entities
		}

		/// <summary>
		/// Draws the state of the scene
		/// </summary>
		/// <param name="display">Display to draw the scene onto</param>
		/// <param name="state">Index of the state to draw</param>
		/// <param name="t">Interpolation value</param>
		/// <remarks>None of the game states should be modified by this process - including the state indicated by <paramref name="state"/>.
		/// Modifying the game state info during this process would corrupt the game state.</remarks>
		public virtual void Draw (IDisplay display, int state, double t)
		{
			// TODO: Render all drawable entities
		}
	}
}
