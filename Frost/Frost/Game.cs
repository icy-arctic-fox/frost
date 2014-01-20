using System;
using System.IO;
using Frost.Display;
using Frost.Modules;

namespace Frost
{
	/// <summary>
	/// Base class for all games that use the Frost engine
	/// </summary>
	public abstract class Game
	{
		#region Game components and modules

		/// <summary>
		/// Window used to display graphics
		/// </summary>
		protected readonly Window Window;

		/// <summary>
		/// Manages the state and speed of the game
		/// </summary>
		protected readonly StateManager Manager;

		/// <summary>
		/// Provides access to all of the resources available for the game
		/// </summary>
		protected readonly ResourceManager Resources;

		/// <summary>
		/// Configuration information for the game core
		/// </summary>
		protected readonly GameConfiguration Configuration;
		#endregion

		/// <summary>
		/// Visible name of the game
		/// </summary>
		/// <remarks>This text is displayed on the window title</remarks>
		public abstract string GameTitle { get; }

		/// <summary>
		/// Short name of the game.
		/// This is used internally and for file paths.
		/// </summary>
		/// <remarks>This string should contain no whitespace or symbols.
		/// Lowercase is also preferred.</remarks>
		public abstract string ShortGameName { get; }

		/// <summary>
		/// Creates the underlying game
		/// </summary>
		protected Game ()
		{
			// Load the core configuration
			Configuration = findConfiguration();

			// Set up resource management
			Resources = new ResourceManager();
			if(Directory.Exists(Configuration.ResourcePath))
				Resources.AddResourceDirectory(Configuration.ResourcePath);
			if(Directory.Exists(Configuration.ModsPath))
				Resources.AddResourceDirectory(Configuration.ModsPath);

			// Create the window
			Window = new Window(Configuration.WindowWidth, Configuration.WindowHeight, GameTitle); // TODO: Pass title to constructor

			// Create the state manager
			Manager = new StateManager(Window, null, null); // TODO: Somehow get the nodes from type params?
		}

		/// <summary>
		/// Starts the game
		/// </summary>
		public void Run ()
		{
			Manager.Run(); // TODO: Add state manager options to configuration
		}

		#region Game configuration

		private const string ConfigurationFileName = "game.config";

		private static bool tryLoadConfiguration (string path, out GameConfiguration config)
		{
			if(File.Exists(path))
			{
				try
				{
					config = GameConfiguration.Load(path);
					if(config != null)
						return true;
				}
				catch {} // TODO: Properly catch errors
			}
			config = null;
			return false;
		}

		private GameConfiguration findConfiguration ()
		{
			GameConfiguration config;

			// First, check the user's home directory
			var appDataPath   = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var appConfigPath = Path.Combine(appDataPath, ShortGameName, ConfigurationFileName);
			if(tryLoadConfiguration(appConfigPath, out config))
				return config;

			// Next, check the working directory (game/program files)
			var curDirPath = Environment.CurrentDirectory;
			var configPath = Path.Combine(curDirPath, ConfigurationFileName);
			if(tryLoadConfiguration(configPath, out config))
				return config;

			// Lastly, check the resource packages
			// TODO

			// Give up, there's no configuration. Create a new one.
			config = new GameConfiguration();

			// Save the new configuration
			configPath = Path.Combine(appDataPath, ShortGameName);
			if(!Directory.Exists(configPath))
				Directory.CreateDirectory(configPath);
			config.Save(appConfigPath);
			return config;
		}
		#endregion
	}
}
