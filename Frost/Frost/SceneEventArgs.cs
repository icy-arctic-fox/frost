using System;

namespace Frost
{
	/// <summary>
	/// Describes an event that involves a scene
	/// </summary>
	public class SceneEventArgs : EventArgs
	{
		private readonly Scene _scene;

		/// <summary>
		/// Scene involved in the event
		/// </summary>
		public Scene Scene
		{
			get { return _scene; }
		}

		/// <summary>
		/// Creates a new scene event
		/// </summary>
		/// <param name="scene">Scene involved in the event</param>
		/// <exception cref="ArgumentNullException">The <paramref name="scene"/> can't be null.</exception>
		public SceneEventArgs (Scene scene)
		{
			if(scene == null)
				throw new ArgumentNullException("scene");

			_scene = scene;
		}
	}
}
