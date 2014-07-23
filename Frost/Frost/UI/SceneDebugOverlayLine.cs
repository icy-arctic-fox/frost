using System;
using Frost.Utility;

namespace Frost.UI
{
	/// <summary>
	/// Debug overlay line that displays scene information from a <see cref="SceneManager"/>
	/// </summary>
	public class SceneDebugOverlayLine : IDebugOverlayLine
	{
		private readonly GameRunner _runner;

		/// <summary>
		/// Creates a new scene debug overlay line
		/// </summary>
		/// <param name="runner">Game runner to get the <see cref="SceneManager"/> from</param>
		/// <exception cref="ArgumentNullException">The game <paramref name="runner"/> can't be null.</exception>
		public SceneDebugOverlayLine (GameRunner runner)
		{
			if(runner == null)
				throw new ArgumentNullException("runner");

			_runner = runner;
			runner.Disposing += _runner_Disposing;
		}

		/// <summary>
		/// Called when the game runner is disposing and the overlay line should be removed
		/// </summary>
		/// <param name="sender">Game runner</param>
		/// <param name="e">Event arguments</param>
		private void _runner_Disposing (object sender, EventArgs e)
		{
			Disposing.NotifyThreadedSubscribers(this, e);
		}

		/// <summary>
		/// Triggered when the game runner is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Retrieves the contents of the debug overlay line
		/// </summary>
		/// <param name="contents">String to store the debug information in</param>
		public void GetDebugInfo (MutableString contents)
		{
			var scenes    = _runner.Scenes;
			var curScene  = scenes.CurrentScene;
			var sceneName = curScene.Name;
			var entities  = curScene.EntityManager.Count;
			var states    = _runner.StateManager;
			contents.Append("Scene: ");
			contents.Append(sceneName);
			contents.Append(' ');
			states.GetDebugInfo(contents);
			contents.Append(" - ");
			contents.Append(entities);
			contents.Append(" entities");
		}
	}
}
