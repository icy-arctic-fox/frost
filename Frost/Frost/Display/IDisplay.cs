using Frost.Graphics;
using SFML.Graphics;

namespace Frost.Display
{
	/// <summary>
	/// Describes an object that displays frames on the screen
	/// </summary>
	public interface IDisplay : IRenderTarget
	{
		/// <summary>
		/// Indicates whether vertical synchronization is enabled
		/// </summary>
		/// <remarks>Vertical synchronization forces frames to be drawn to the screen in time with screen refreshes.
		/// Enabling VSync can reduce the rendering rate (fps).</remarks>
		bool VSync { get; set; } // TODO: Implement 'Adaptive' VSync

		/// <summary>
		/// Performs any updates to the display's state (not part of the rendered frames)
		/// </summary>
		void Update ();

		/// <summary>
		/// Sets a display object as being actively rendered in on a thread.
		/// A display object can be rendered from only one thread at a time.
		/// Call this method with false to so that other threads can render to it.
		/// A thread must call this method with a true flag to enable rendering for that thread, but this will not work if rendering is active on another thread.
		/// </summary>
		/// <param name="flag">True if the display object is active for rendering on the current thread, or false to disable rendering on the current thread</param>
		/// <returns>True if the current thread is now active for rendering, false if another thread is already rendering.</returns>
		bool SetActive (bool flag = true);

		/// <summary>
		/// Starts a new frame
		/// </summary>
		void EnterFrame ();

		/// <summary>
		/// Ends drawing actions for the current frame and displays it
		/// </summary>
		void ExitFrame ();
	}
}
