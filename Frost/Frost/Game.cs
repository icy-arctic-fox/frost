﻿using System;
using System.Collections.Generic;
using System.IO;
using Frost.Display;
using Frost.Logic;
using Frost.Modules;
using Frost.Utility;

namespace Frost
{
	/// <summary>
	/// Base class for all games that use the Frost engine
	/// </summary>
	public abstract class Game : IFullDisposable
	{
		#region Game components and modules

		/// <summary>
		/// Window used to display graphics
		/// </summary>
		public Window Window { get; private set; }

		/// <summary>
		/// Manages the state and speed of the game
		/// </summary>
		public GameRunner Runner { get; private set; }

		/// <summary>
		/// Provides access to all of the resources available for the game
		/// </summary>
		public ResourceManager Resources { get; private set; }

		/// <summary>
		/// Configuration information for the game core
		/// </summary>
		public GameConfiguration Configuration { get; private set; }
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
		/// Creates the initial scene displayed in the game
		/// </summary>
		/// <returns>A scene</returns>
		protected abstract Scene CreateInitialScene ();

		private readonly List<IModule> _modules = new List<IModule>();

		/// <summary>
		/// Creates and initializes the modules used by the game
		/// </summary>
		protected virtual void InitializeModules ()
		{
			var input = new InputModule();
			input.Initialize();
			AddModule(input);
		}

		/// <summary>
		/// Adds a module to the game which is processed each logic update
		/// </summary>
		/// <param name="module">Module to add</param>
		protected void AddModule (IModule module)
		{
			Runner.AddModule(module);
			_modules.Add(module);
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
			Window = new Window(Configuration.WindowWidth, Configuration.WindowHeight, GameTitle);
			Window.Closed += Window_Closed;

			// Create the runner
			var initialScene = CreateInitialScene();
			Runner = new GameRunner(Window, initialScene) {ThreadSynchronization = Configuration.SyncRenderThread};

			InitializeModules();
			Initialized = true;
		}

		/// <summary>
		/// Starts the game
		/// </summary>
		/// <remarks>This method will not return until the game has finished.</remarks>
		/// <exception cref="InvalidOperationException">Thrown if the game hasn't been initialized</exception>
		/// <seealso cref="Initialize"/>
		public void Run ()
		{
			if(!Initialized)
				throw new InvalidOperationException("The game has not been initialized yet.");
			Runner.Run(UpdateRate, Configuration.FrameRate, Configuration.ThreadedRender);
		}

		/// <summary>
		/// Shuts down and cleans up the core game components
		/// </summary>
		public virtual void Shutdown ()
		{
			if(Initialized)
			{
				if(Configuration.Dirty)
				{// Save the modified configuration
					var gameDataDir = GameDataPath;
					if(!Directory.Exists(gameDataDir))
						Directory.CreateDirectory(gameDataDir);
					Configuration.Save(GameConfigurationPath);
				}

				Window.Dispose();
				Resources.Dispose();
				ShutdownModules();
			}
		}

		/// <summary>
		/// Shuts down and disposes of game modules
		/// </summary>
		protected virtual void ShutdownModules ()
		{
			foreach(var module in _modules)
				module.Dispose();
		}

		#region Subscribers

		/// <summary>
		/// Stop the runner when the window is closed
		/// </summary>
		/// <param name="sender">Window object</param>
		/// <param name="e">Event arguments</param>
		private void Window_Closed (object sender, EventArgs e)
		{
			Runner.Stop();
		}
		#endregion

		#region Game configuration

		private const string ConfigurationFileName = "game.config";

		/// <summary>
		/// Full path to the user's application data directory
		/// </summary>
		private static readonly string _applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		/// <summary>
		/// Full path to the directory that contains the user's data for the game
		/// </summary>
		public string GameDataPath 
		{
			get { return Path.Combine(_applicationDataPath, ShortGameName); }
		}

		/// <summary>
		/// Full path to the file that contains the game's configuration
		/// </summary>
		public string GameConfigurationPath
		{
			get { return Path.Combine(_applicationDataPath, ShortGameName, ConfigurationFileName); }
		}

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
			return new GameConfiguration();
		}
		#endregion

		#region Disposable

		/// <summary>
		/// Flag that indicates whether the game has been disposed
		/// </summary>
		public bool Disposed { get; private set; }

		/// <summary>
		/// Triggered when the game is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Frees the resources held by the game
		/// </summary>
		/// <remarks>Disposing of the game will stop and shut it down if it is still running.</remarks>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Tears down the game
		/// </summary>
		~Game ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Underlying method that releases the resources held by the game
		/// </summary>
		/// <param name="disposing">True if <see cref="Dispose"/> was called and inner resources should be disposed as well</param>
		protected void Dispose (bool disposing)
		{
			if(!Disposed)
			{// Don't do anything if the game is already disposed
				Disposed = true;
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);
				if(disposing)
				{// Dispose of the resources this object holds
					Shutdown();
					Runner.Dispose();
					Window.Dispose();
					Resources.Dispose();
				}
			}
		}
		#endregion
	}
}
