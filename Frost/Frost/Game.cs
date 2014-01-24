﻿using System;
using System.IO;
using Frost.Display;

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
		protected Window Window;

		/// <summary>
		/// Manages the state and speed of the game
		/// </summary>
		protected GameRunner Runner;

		/// <summary>
		/// Provides access to all of the resources available for the game
		/// </summary>
		protected readonly ResourceManager Resources;

		/// <summary>
		/// Configuration information for the game core
		/// </summary>
		protected GameConfiguration Configuration;
		#endregion

		/// <summary>
		/// Indicates whether the game has been initialized
		/// </summary>
		public bool Initialized { get; private set; }

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
		/// Number of game updates processed per second
		/// </summary>
		protected abstract double UpdateRate { get; }

		/// <summary>
		/// Creates the underlying game
		/// </summary>
		protected Game ()
		{
			Resources = new ResourceManager();
		}

		/// <summary>
		/// Initializes the core game components
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if the game has already been initialized</exception>
		/// <seealso cref="Initialized"/>
		public virtual void Initialize ()
		{
			if(Initialized)
				throw new InvalidOperationException("The game has already been initialized.");

			// Load the core configuration
			Configuration = findConfiguration();

			// Set up resource management
			if(Directory.Exists(Configuration.ResourcePath))
				Resources.AddResourceDirectory(Configuration.ResourcePath);
			if(Directory.Exists(Configuration.ModsPath))
				Resources.AddResourceDirectory(Configuration.ModsPath);

			// Create the window
			Window = new Window(Configuration.WindowWidth, Configuration.WindowHeight, GameTitle); // TODO: Pass title to constructor

			// TODO: Create scene manager

			Initialized = true;
		}

		/// <summary>
		/// Starts the game
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if the game hasn't been initialized</exception>
		/// <seealso cref="Initialize"/>
		public void Run ()
		{
			if(!Initialized)
				throw new InvalidOperationException("The game has not been initialized yet.");
			// TODO
		}

		/// <summary>
		/// Shuts down and cleans up the core game components
		/// </summary>
		public virtual void Shutdown ()
		{
			if(Initialized)
			{
				// TODO
			}
		}

		#region Game configuration

		private const string ConfigurationFileName = "game.config";

		/// <summary>
		/// Tries to load a configuration at a location
		/// </summary>
		/// <param name="path">Path to the game configuration file</param>
		/// <param name="config">Loaded game configuration</param>
		/// <returns>True if the configuration was found and loaded</returns>
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

		/// <summary>
		/// Finds or creates the configuration for the core game
		/// </summary>
		/// <returns></returns>
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
