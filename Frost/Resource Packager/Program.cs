using System;
using System.Collections.Generic;
using System.IO;
using Frost.IO.Resources;

namespace Frost.ResourcePackager
{
	class Program
	{
		private static bool _verbose, _stripExt;

		static int Main (string[] args)
		{
			var returnCode = ReturnCode.Ok;

			if(args.Length < 2)
				printUsage();
			else
			{
				var index = 0;
				bool matched;

				do
				{
					matched = false;

					// Parse options
					if(args[index] == "-v")
					{ // Turn on verbosity
						_verbose = true;
						++index;
						matched = true;
					}

					if(args[index] == "-s")
					{ // Strip file extensions
						_stripExt = true;
						++index;
						matched = true;
					}
				} while(matched); 

				// Parse actions
				var action    = args[index++].ToLower();
				var filepath  = args[index++];
				var list      = new List<KeyValuePair<string, string>>();
				var remaining = args.Length - index;
				if(remaining % 2 != 0)
					Console.WriteLine("Uneven number of arguments, list arguments as: resource, filename...");
				else
				{
					for(var i = index; i < args.Length; i += 2)
					{
						var name  = args[i];
						var file  = args[i + 1];
						var entry = new KeyValuePair<string, string>(name, file);
						list.Add(entry);
					}

					switch(action)
					{
					case "c":
					case "create":
						returnCode = createResourcePackageFile(filepath, list);
						break;
					case "x":
					case "extract":
						returnCode = extractResourcePackageFile(filepath, list);
						break;
					case "p":
					case "pack":
						returnCode = packDirectoryContents(filepath, list);
						break;
					case "u":
					case "unpack":
						returnCode = unpackDirectoryContents(filepath, list);
						break;
					case "l":
					case "list":
						returnCode = listResourcePackageContents(filepath);
						break;
					case "i":
					case "info":
						returnCode = displayPackageInfo(filepath);
						break;
					default:
						Console.Error.WriteLine("Unknown action '{0}'", action);
						printUsage();
						returnCode = ReturnCode.BadUsage;
						break;
					}
				}
			}

			return (int)returnCode;
		}

		/// <summary>
		/// Prints the usage for the program
		/// </summary>
		private static void printUsage ()
		{
			Console.WriteLine("frp is a program for creating and extracting Frost Resource Packages (.frp files) for the Frost game engine.");
			Console.WriteLine("Usage: frp [options] <action> <frp file> [resource, filename]...");
			Console.WriteLine();

			Console.WriteLine("Available options:");
			Console.WriteLine("   -v - Enable verbose output");
			Console.WriteLine("   -s - Strip file extensions");
			Console.WriteLine();

			Console.WriteLine("Available actions:");
			Console.WriteLine("   c or create  - Create a new resource package");
			Console.WriteLine("                  This will pack the file from [filename] and name it [resource] in the resource package.");
			Console.WriteLine("   e or extract - Extract resources from an existing package");
			Console.WriteLine("                  This will extract a resourced named [resource] from the package and store it in [filename].");
			Console.WriteLine("   p or pack    - Creates a new resource package from all of the files in a directory.");
			Console.WriteLine("                  [resource] now becomes a prefix for resource names in the corresponding directory [filename].");
			Console.WriteLine("   u or unpack  - Extracts resources from an existing package that match one or more prefixes.");
			Console.WriteLine("                  [resource] now becomes a prefix for resource names to extract into a corresponding directory [filename].");
			Console.WriteLine("   l or list    - Lists all of the resources contained in the package file.");
			Console.WriteLine("   i or info    - Displays meta-information about the resource package.");
			Console.WriteLine();

			Console.WriteLine("For [resource, filename]...");
			Console.WriteLine("provide a resource name (as it would appear in the resource file)");
			Console.WriteLine("and a path to a file to pack or extract to.");
			Console.WriteLine("[resource, filename] can be repeated multiple times for each resource to add or extract.");
		}

		/// <summary>
		/// Creates a new resource package file
		/// </summary>
		/// <param name="filepath">Path to the resource package file to create</param>
		/// <param name="contents">Collection of resources to put in the package.
		/// The layout is "resource name" => "filename"</param>
		/// <returns>A program return code</returns>
		private static ReturnCode createResourcePackageFile (string filepath, IEnumerable<KeyValuePair<string, string>> contents)
		{
			using(var writer = new ResourcePackageWriter(filepath))
			{
				foreach(var entry in contents)
				{
					var id   = Guid.NewGuid();
					var name = entry.Key;
					var file = entry.Value;

					if(_verbose)
						Console.WriteLine("{0} => {1}", file, name);

					var data = File.ReadAllBytes(file); // TODO: Make this better by using streams
					writer.Add(id, name, data); // TODO: Catch duplicate resource names
				}
				if(_verbose)
					Console.WriteLine("Writing contents to file...");
			}
			return ReturnCode.Ok;
		}

		/// <summary>
		/// Extracts selected resources from a resource package file
		/// </summary>
		/// <param name="filepath">Path to the resource package file to extract from</param>
		/// <param name="entries">Names of the resources to extract and their destination file path.
		/// The layout is "resource name" => "filename"</param>
		/// <returns>A program return code</returns>
		private static ReturnCode extractResourcePackageFile (string filepath, IEnumerable<KeyValuePair<string, string>> entries)
		{
			using(var reader = new ResourcePackageReader(filepath))
				foreach(var entry in entries)
				{
					var name = entry.Key;
					var file = entry.Value;

					if(_verbose)
						Console.WriteLine("{0} => {1}", name, file);

					var data = reader.GetResource(name);
					if(data != null)
						File.WriteAllBytes(file, data);
					else
						Console.Error.WriteLine("Resource '{0}' doesn't exist", name);
				}
			return ReturnCode.Ok;
		}

		/// <summary>
		/// Looks through a directory and its sub-directories for files
		/// </summary>
		/// <param name="entries">List to append found files to</param>
		/// <param name="dirPath">Directory path to look in</param>
		/// <param name="prefix">Prefix to give the resource names</param>
		private static void discoverFiles (ICollection<KeyValuePair<string, string>> entries, string dirPath, string prefix)
		{
			foreach(var file in Directory.EnumerateFiles(dirPath))
			{// Iterate through the files
				var name  = prefix + (_stripExt ? Path.GetFileNameWithoutExtension(file) : Path.GetFileName(file));
				var entry = new KeyValuePair<string, string>(name, file);
				entries.Add(entry);
			}

			foreach(var dir in Directory.EnumerateDirectories(dirPath))
			{// Recurse into sub-directories
				var dirName   = Path.GetFileName(dir);
				var subPrefix = String.Format("{0}{1}/", prefix, dirName);
				discoverFiles(entries, dir, subPrefix);
			}
		}

		/// <summary>
		/// Creates a resource package file from all files contained in a directory
		/// </summary>
		/// <param name="filepath">Path to the resource package to create</param>
		/// <param name="dirs">Directories and prefixes to package.
		/// The layout is: "name prefix" => "directory path"</param>
		/// <returns>A program return code</returns>
		private static ReturnCode packDirectoryContents (string filepath, IEnumerable<KeyValuePair<string, string>> dirs)
		{
			foreach(var entry in dirs)
			{
				var prefix = String.IsNullOrWhiteSpace(entry.Key) ? String.Empty : entry.Key + "/";
				var dir    = entry.Value;
				if(Directory.Exists(dir))
				{
					var entries = new List<KeyValuePair<string, string>>();
					discoverFiles(entries, dir, prefix);
					return createResourcePackageFile(filepath, entries);
				}
				Console.Error.WriteLine("Source directory '{0}' doesn't exist", dir);
				return ReturnCode.MissingSource;
			}
			return ReturnCode.Ok;
		}

		/// <summary>
		/// Unpacks selected resources prefixed with a value from the package and stores them into directories
		/// </summary>
		/// <param name="filepath">Path to the resource package to extract from</param>
		/// <param name="dirs">Directories and prefixes to unpack.
		/// The layout is: "name prefix" => "directory path"</param>
		/// <returns>A program return code</returns>
		private static ReturnCode unpackDirectoryContents (string filepath, IEnumerable<KeyValuePair<string, string>> dirs)
		{
			using(var reader = new ResourcePackageReader(filepath))
				foreach(var entry in dirs)
				{
					var prefix = String.IsNullOrWhiteSpace(entry.Key) ? String.Empty : entry.Key + "/";
					var dir    = entry.Value;
					foreach(var resource in reader.Resources)
						if(resource.Name.StartsWith(prefix))
						{// Resource matches
							var resPath = resource.Name.Replace('/', Path.DirectorySeparatorChar);
							var file    = Path.Combine(dir, resPath);
							var subDir  = Path.GetDirectoryName(file) ?? ".";
							if(_stripExt)
								file = Path.Combine(dir, subDir, Path.GetFileNameWithoutExtension(file) ?? file);
							if(_verbose)
								Console.WriteLine("{0} => {1}", resource.Name, file);

							var data = reader.GetResource(resource.Name);
							if(!Directory.Exists(subDir))
							{
								if(_verbose)
									Console.WriteLine("Creating directory {0}", subDir);
								Directory.CreateDirectory(subDir);
							}
							File.WriteAllBytes(file, data);
						}
				}
			return ReturnCode.Ok;
		}

		/// <summary>
		/// Lists all of the resources contained in the package
		/// </summary>
		/// <param name="filepath">Path to the resource package to read from</param>
		/// <returns>A program return code</returns>
		private static ReturnCode listResourcePackageContents (string filepath)
		{
			using(var reader = new ResourcePackageReader(filepath))
				foreach(var resource in reader.Resources)
					Console.WriteLine("{0} {1,9} {2}", resource.Id, formatSize(resource.Size), resource.Name);
			return ReturnCode.Ok;
		}

		private static readonly string[] _units = new[] {"b ", "KB", "MB", "GB"};

		/// <summary>
		/// Formats a number of bytes as a human readable string
		/// </summary>
		/// <param name="bytes">Size in bytes</param>
		/// <returns>Formatted size as a string</returns>
		private static string formatSize (long bytes)
		{
			var index = 0;
			var size  = (double)bytes;
			while(size > 1000d)
			{
				size /= 1024d;
				++index;
			}
			var unit = _units[index];
			return String.Format("{0:F} {1}", size, unit);
		}

		/// <summary>
		/// Displays information about the package
		/// </summary>
		/// <param name="filepath">Path to the resource package to read from</param>
		/// <returns>A program return code</returns>
		private static ReturnCode displayPackageInfo (string filepath)
		{
			using(var reader = new ResourcePackageReader(filepath))
			{
				Console.WriteLine("Name:        {0}", reader.Name);
				Console.WriteLine("Creator:     {0}", reader.Creator);
				Console.WriteLine("Description: {0}", reader.Description);
			}
			return ReturnCode.Ok;
		}
	}
}
