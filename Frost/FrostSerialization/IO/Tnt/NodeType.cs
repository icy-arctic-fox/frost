namespace Frost.IO.Tnt
{
	/// <summary>
	/// Values for the different types of nodes that exist
	/// </summary>
	public enum NodeType : byte
	{
		/// <summary>
		/// Not really a node type, but an indicator.
		/// Symbolizes the end of a collection of nodes.
		/// </summary>
		End = 0,

		/// <summary>
		/// True or false value
		/// </summary>
		Boolean = 1,

		/// <summary>
		/// 8-bit unsigned integer
		/// </summary>
		Byte = 2,

		/// <summary>
		/// 8-bit signed integer
		/// </summary>
		SByte = 3,

		/// <summary>
		/// 16-bit signed integer
		/// </summary>
		Short = 4,

		/// <summary>
		/// 16-bit unsigned integer
		/// </summary>
		UShort = 5,

		/// <summary>
		/// 32-bit signed integer
		/// </summary>
		Int = 6,

		/// <summary>
		/// 32-bit unsigned integer
		/// </summary>
		UInt = 7,

		/// <summary>
		/// 64-bit signed integer
		/// </summary>
		Long = 8,

		/// <summary>
		/// 64-bit unsigned integer
		/// </summary>
		ULong = 9,

		/// <summary>
		/// 32-bit single-precision floating-point number
		/// </summary>
		Float = 10,

		/// <summary>
		/// 64-bit double-precision floating-point number
		/// </summary>
		Double = 11,

		/// <summary>
		/// Collection of characters that make up text
		/// </summary>
		String = 12,

		/// <summary>
		/// 128-bit globally unique identifier
		/// </summary>
		Guid = 13,

		/// <summary>
		/// Date and time
		/// </summary>
		DateTime = 14,

		/// <summary>
		/// Length of time
		/// </summary>
		TimeSpan = 15,

		/// <summary>
		/// Raw binary data (array of bytes)
		/// </summary>
		Blob = 16,

		/// <summary>
		/// X and Y size or location as an integer
		/// </summary>
		Xy = 17,

		/// <summary>
		/// X, Y, and Z size of location as an integer
		/// </summary>
		Xyz = 18,

		/// <summary>
		/// X and Y-coordinates represented as floating-point numbers
		/// </summary>
		Point2f = 19,

		/// <summary>
		/// X, Y, and Z-coordinates represented as floating-point numbers
		/// </summary>
		Coordinate3D = 20,

		/// <summary>
		/// ARGB color value
		/// </summary>
		Color = 21,

		/// <summary>
		/// List of nodes of the same type
		/// </summary>
		List = 22,

		/// <summary>
		/// Collection of nodes with varying types
		/// </summary>
		Complex = 23

		// TODO: Add 'Reference' type to allow pointing to existing nodes
	}
}
