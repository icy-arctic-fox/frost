﻿using System;
using Frost.Logic;
using Frost.Utility;

namespace Frost.UI
{
	/// <summary>
	/// Debug overlay line that displays scene information from a <see cref="Scene.Manager"/>
	/// </summary>
	public class SceneDebugOverlayLine : IDebugOverlayLine
	{
		private readonly Scene.Manager _scenes;

		/// <summary>
		/// Creates a new scene debug overlay line
		/// </summary>
		/// <param name="runner">Game runner to get the <see cref="Scene.Manager"/> from</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="runner"/> is null</exception>
		public SceneDebugOverlayLine (GameRunner runner)
		{
			if(runner == null)
				throw new ArgumentNullException("runner", "The game runner can't be null.");

			_scenes = runner.Scenes;
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
		/// Generates the text displayed in the debug overlay
		/// </summary>
		/// <returns>Scene information</returns>
		public override string ToString ()
		{
			return String.Format("Scene: {0} - {1}", _scenes.CurrentScene.Name, _scenes.StateManager);
		}
	}
}
