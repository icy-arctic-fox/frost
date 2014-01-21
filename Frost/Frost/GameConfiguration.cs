using System;
using System.IO;
using Newtonsoft.Json;

namespace Frost
{
	/// <summary>
	/// Configuration information about the core game
	/// </summary>
	public class GameConfiguration
	{
		#region Defaults

		/// <summary>
		/// Default number of frames to render per second
		/// </summary>
		public const double DefaultRenderRate = 60d;

		/// <summary>
		/// Indicates whether threaded rendering is enabled by default
		/// </summary>
		public const bool DefaultThreadedRender = false;

		/// <summary>
		/// Indicates whether the game logic and render threads should be synchronized by default
		/// </summary>
		public const bool DefaultSyncRenderThread = true;

		/// <summary>
		/// Default path to the resources directory
		/// </summary>
		public const string DefaultResourcePath = "resources";

		/// <summary>
		/// Default path to the mod resource directory
		/// </summary>
		public const string DefaultModsPath = "mods";
		#endregion

		/// <summary>
		/// Creates a default game configuration
		/// </summary>
		public GameConfiguration ()
		{
			WindowWidth  = Display.Window.DefaultWidth;
			WindowHeight = Display.Window.DefaultHeight;

			FrameRate        = DefaultRenderRate;
			ThreadedRender   = DefaultThreadedRender;
			SyncRenderThread = DefaultSyncRenderThread;
		}

		/// <summary>
		/// Creates a new game configuration
		/// </summary>
		/// <param name="width">Width of the window in pixels</param>
		/// <param name="height">Height of the window in pixels</param>
		public GameConfiguration (uint width, uint height)
		{
			WindowWidth  = width;
			WindowHeight = height;

			FrameRate        = DefaultRenderRate;
			ThreadedRender   = DefaultThreadedRender;
			SyncRenderThread = DefaultSyncRenderThread;
		}

		#region Json
		private static readonly JsonSerializer Json;

		/// <summary>
		/// Initializes the Json serializer
		/// </summary>
		static GameConfiguration ()
		{
			Json = new JsonSerializer {Formatting = Formatting.Indented};
		}
		#endregion

		/// <summary>
		/// Indicates whether the configuration has been modified since last save or load
		/// </summary>
		[JsonIgnore]
		public bool Dirty { get; private set; }

		#region Properties

		/// <summary>
		/// Width of the window in pixels
		/// </summary>
		[JsonProperty("windowWidth")]
		public uint WindowWidth { get; set; }

		/// <summary>
		/// Height of the window in pixels
		/// </summary>
		[JsonProperty("windowHeight")]
		public uint WindowHeight { get; set; }

		/// <summary>
		/// Number of frames rendered per second (0 for unbounded)
		/// </summary>
		[JsonProperty("frameRate")]
		public double FrameRate { get; set; }

		/// <summary>
		/// Indicates whether a separate thread from the game logic thread is used for rendering
		/// </summary>
		[JsonProperty("threadedRender")]
		public bool ThreadedRender { get; set; }

		/// <summary>
		/// Indicates whether the render and game logic thread are synchronized.
		/// This option is only beneficial when <see cref="ThreadedRender"/> is enabled when both threads should be running at the same frequency.
		/// </summary>
		[JsonProperty("syncRenderThread")]
		public bool SyncRenderThread { get; set; }

		private string _resourcePath = DefaultResourcePath;

		/// <summary>
		/// Path to the directory that contains game resources
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown when attempting to set the path to null or whitespace</exception>
		[JsonProperty("resourcePath")]
		public string ResourcePath
		{
			get { return _resourcePath; }
			set
			{
				if(String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value", "Can't set the resource path to null or a blank value.");
				_resourcePath = value;
			}
		}

		private string _modsPath = DefaultModsPath;

		/// <summary>
		/// Path to the directory that contains the mod resources
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown when attempting to set the path to null or whitespace</exception>
		[JsonProperty("modResourcePath")]
		public string ModsPath
		{
			get { return _modsPath; }
			set
			{
				if(String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value", "Can't set the mod resource path to null or a blank value.");
				_modsPath = value;
			}
		}
		#endregion

		#region Save and load

		/// <summary>
		/// Saves the configuration to a file
		/// </summary>
		/// <param name="path">Path of the file to save to</param>
		/// <remarks><see cref="Dirty"/> will be set to false after saving.</remarks>
		public void Save (string path)
		{
			using(var writer = File.CreateText(path))
				Json.Serialize(writer, this);
			Dirty = false;
		}

		/// <summary>
		/// Loads the configuration from a file
		/// </summary>
		/// <param name="path">Path of the file to load from</param>
		/// <returns>The configuration or null if there was an error loading the configuration</returns>
		public static GameConfiguration Load (string path)
		{
			using(var reader = File.OpenText(path))
				return Json.Deserialize(reader, typeof(GameConfiguration)) as GameConfiguration;
		}
		#endregion
	}
}
