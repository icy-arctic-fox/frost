using SFML.Graphics;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// A single unbreakable segment in word wrapping
	/// </summary>
	/// <typeparam name="T">Type of segment</typeparam>
	/// <seealso cref="WordWrap{T}"/>
	internal struct Word<T>
	{
		private readonly T _value;

		/// <summary>
		/// Value of the word
		/// </summary>
		public T Value
		{
			get { return _value; }
		}

		private readonly IntRect _bounds;

		/// <summary>
		/// Bounds of the word
		/// </summary>
		public IntRect Bounds
		{
			get { return _bounds; }
		}

		/// <summary>
		/// Creates a new word in a word wrap text block
		/// </summary>
		/// <param name="value">Value of the word</param>
		/// <param name="bounds">Bounds of the word</param>
		public Word (T value, IntRect bounds)
		{
			_value  = value;
			_bounds = bounds;
		}
	}
}
