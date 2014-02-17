using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Frost.Utility;

namespace Frost.Scripting.Compiler
{
	/// <summary>
	/// Pulls tokens from a stream of characters
	/// </summary>
	public class Lexer : IFullDisposable
	{
		private readonly BinaryReader _br;

		/// <summary>
		/// Creates a new token lexer
		/// </summary>
		/// <param name="s">Stream to read characters from</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is null</exception>
		public Lexer (Stream s)
		{
			if(s == null)
				throw new ArgumentNullException("s", "The stream to read characters from can't be null.");

			_br = new BinaryReader(s);
		}

		#region Disposable

		private volatile bool _disposed;

		/// <summary>
		/// Indicates whether the lexer has been disposed
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Triggered when the lexer is being disposed
		/// </summary>
		public event EventHandler<EventArgs> Disposing;

		/// <summary>
		/// Frees the resources held by the lexer
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Deconstructs the lexer
		/// </summary>
		~Lexer ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the lexer
		/// </summary>
		/// <param name="disposing">Flag indicating whether internal resources should be freed</param>
		protected virtual void Dispose (bool disposing)
		{
			if(!_disposed)
			{
				_disposed = true;
				Disposing.NotifyThreadedSubscribers(this, EventArgs.Empty);

				if(disposing) // Dispose of the stream
					_br.Dispose();
			}
		}
		#endregion
	}
}
