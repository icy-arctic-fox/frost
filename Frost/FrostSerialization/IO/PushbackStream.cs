using System;
using System.IO;

namespace Frost.IO
{
	/// <summary>
	/// Stream decorator class used to "push" data back into the stream by writing to it.
	/// The pushed data is read first before advancing in the underlying stream.
	/// This stream decorator can be used to simulate seeking on a non-seekable stream by pushing previously read data back onto the stream.
	/// </summary>
	public sealed class PushbackStream : Stream
	{
		private readonly Stream _s;

		/// <summary>
		/// Creates a new pushback stream decorator
		/// </summary>
		/// <param name="s">Underlying stream</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is null</exception>
		public PushbackStream (Stream s)
		{
			if(s == null)
				throw new ArgumentNullException("s", "The underlying stream can't be null.");
			_s = s;
		}

		/// <summary>
		/// Flushes the contents of the stream.
		/// This method is unsupported for pushback streams.
		/// </summary>
		/// <exception cref="NotSupportedException">Pushback streams don't support flushing</exception>
		public override void Flush ()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Sets the position within the current stream
		/// </summary>
		/// <returns>The new position within the current stream</returns>
		/// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter</param>
		/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed</exception>
		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Sets the length of the current stream
		/// </summary>
		/// <param name="value">The desired length of the current stream in bytes</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed</exception>
		public override void SetLength (long value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
		/// Any pushed back data (via <see cref="Write"/>) will be read first.
		/// </summary>
		/// <returns>The total number of bytes read into the buffer.
		/// This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
		/// <param name="buffer">An array of bytes.
		/// When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream</param>
		/// <param name="count">The maximum number of bytes to be read from the current stream</param>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed</exception>
		public override int Read (byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Pushes data back into the stream to be read at a later time by <see cref="Read"/>
		/// </summary>
		/// <param name="buffer">An array of bytes.
		/// This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the pushback buffer.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the pushback buffer</param>
		/// <param name="count">The number of bytes to be written to the current stream</param>
		public override void Write (byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Indicates whether the current stream supports reading
		/// </summary>
		/// <returns>True if the stream supports reading; otherwise, false</returns>
		/// <remarks>This property is always true.</remarks>
		public override bool CanRead
		{
			get { return true; }
		}

		/// <summary>
		/// Indicates whether the current stream supports seeking
		/// </summary>
		/// <returns>True if the stream supports seeking; otherwise, false</returns>
		public override bool CanSeek
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Indicates whether the current stream supports writing (pushback)
		/// </summary>
		/// <returns>True if the stream supports writing; otherwise, false</returns>
		/// <remarks>This property is always true.</remarks>
		public override bool CanWrite
		{
			get { return true; }
		}

		/// <summary>
		/// Length in bytes of the stream
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">A class derived from <see cref="Stream"/> does not support seeking</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed</exception>
		public override long Length
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Byte offset (position) within the current stream
		/// </summary>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed</exception>
		public override long Position
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}
	}
}
