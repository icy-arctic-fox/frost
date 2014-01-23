using System;

namespace Frost.Modules
{
	/// <summary>
	/// Collection of functionality with logic that runs every update
	/// </summary>
	public interface IModule : IDisposable
	{
		/// <summary>
		/// Starts up the module and prepares it for usage
		/// </summary>
		void Initialize ();

		/// <summary>
		/// Performs a logic update
		/// </summary>
		void Update ();
	}
}
