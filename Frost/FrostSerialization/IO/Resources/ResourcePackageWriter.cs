using System.IO;

namespace Frost.IO.Resources
{
	/// <summary>
	/// Creates resource package files
	/// </summary>
	public class ResourcePackageWriter : ResourcePackage
	{
		/// <summary>
		/// Current resource package version
		/// </summary>
		private const int CurrentVersion = 1;

		/// <summary>
		/// Writer used to put data into the file
		/// </summary>
		private readonly BinaryWriter _bw;

		/// <summary>
		/// Opens a resource package file to start storing resources into it
		/// </summary>
		/// <param name="filepath">Path to the resource file</param>
		/// <param name="blockSize">Size (in kilobytes) of each block minus 1 (0 means 1 KB)</param>
		/// <param name="opts">Options for the resource package file</param>
		/// <remarks>The file will remain open until <see cref="Close"/> or <see cref="Dispose"/> is called.</remarks>
		public ResourcePackageWriter (string filepath, byte blockSize = 3, ResourcePackageOptions opts = ResourcePackageOptions.None)
		{
			// Create the file stream
			FileStream = new FileStream(filepath, FileMode.Create);
			_bw        = new EndianBinaryWriter(FileStream, Endian.Big);

			// Write the file header info
			var info  = new HeaderInfo(CurrentVersion, opts, blockSize);
			BlockSize = (blockSize + 1) * Kilobyte; // +1 to make 0 mean 1 KB block size
			Options   = opts;
			writeFileInfo(_bw, info);

			// The header is created when Write() is called
		}

		#region IO
		#region Save

		/// <summary>
		/// Writes the resource package header to the file
		/// </summary>
		/// <param name="bw">Writer used to put data into the file</param>
		/// <param name="info">Raw header information</param>
		private static void writeFileInfo (BinaryWriter bw, HeaderInfo info)
		{
			bw.Write(info.Version);
			bw.Write((ushort)info.Options);
			bw.Write(info.KbCount);
		}
		#endregion

		#region Access
		#endregion

		/// <summary>
		/// Closes the resource package file.
		/// No more operations to the contents of the package will be allowed for this instance.
		/// </summary>
		public override void Close ()
		{
			_bw.Close();
		}
		#endregion

		/// <summary>
		/// Disposes of the resource package
		/// </summary>
		/// <param name="disposing">True if inner-resources should be disposed of (<see cref="Dispose"/> was called)</param>
		protected override void Dispose (bool disposing)
		{
			if(!Disposed)
			{
				Disposed = true;
				_bw.Dispose();
			}
		}
	}
}
