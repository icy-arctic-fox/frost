using System;
using System.IO;

namespace Frost.IO
{
	/// <summary>
	/// An object that can read and write its contents over a stream.
	/// Streamable classes *should* have a constructor that accepts a <see cref="BinaryReader"/> to read data with.
	/// </summary>
	public interface IStreamable
	{
		/// <summary>
		/// Writes the contents of the object to a stream using a stream writer
		/// </summary>
		/// <param name="bw">Binary writer to use for writing data to the stream</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bw"/> is null.
		/// The writer for putting data on the stream can't be null.</exception>
		void WriteToStream (BinaryWriter bw);
	}
}
