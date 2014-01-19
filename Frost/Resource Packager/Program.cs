using System;
using System.Collections.Generic;
using System.IO;
using Frost.IO.Resources;

namespace Frost.ResourcePackager
{
	class Program
	{
		static int Main (string[] args)
		{
			var returnCode = ReturnCode.Ok;

			if(args.Length < 2)
				printUsage();
			else
			{
				var action   = args[0].ToLower();
				var filepath = args[1];
				var list     = new List<KeyValuePair<string, string>>();
				if(args.Length % 2 != 0)
					Console.WriteLine("Uneven number of arguments, list arguments as: resource, filename...");
				else
				{
					for(var i = 2; i < args.Length; i += 2)
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
			throw new NotImplementedException();
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
				foreach(var entry in contents)
				{
					var id   = Guid.NewGuid();
					var name = entry.Key;
					var file = entry.Value;
					var data = File.ReadAllBytes(file);
					writer.Add(id, name, data);
				}
			return 0;
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
					var data = reader.GetResource(name);
					if(data != null)
						File.WriteAllBytes(file, data);
					else
						Console.Error.WriteLine("Resource '{0}' doesn't exist", name);
				}
			return 0;
		}
	}
}
