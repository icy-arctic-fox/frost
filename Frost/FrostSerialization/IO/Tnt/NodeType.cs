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
		/// 8-bit unsigned integer
		/// </summary>
		Byte = 1,

		/// <summary>
		/// 8-bit signed integer
		/// </summary>
		SByte = 2,

		/// <summary>
		/// 16-bit signed integer
		/// </summary>
		Short = 3,

		/// <summary>
		/// 16-bit unsigned integer
		/// </summary>
		UShort = 4,

		/// <summary>
		/// 32-bit signed integer
		/// </summary>
		Int = 5,

		/// <summary>
		/// 32-bit unsigned integer
		/// </summary>
		UInt = 6,

		/// <summary>
		/// 64-bit signed integer
		/// </summary>
		Long = 7,

		/// <summary>
		/// 64-bit unsigned integer
		/// </summary>
		ULong = 8,

		/// <summary>
		/// 32-bit single-precision floating-point number
		/// </summary>
		Float = 9,

		/// <summary>
		/// 64-bit double-precision floating-point number
		/// </summary>
		Double = 10,

		/// <summary>
		/// Collection of characters that make up text
		/// </summary>
		String = 11,

		/// <summary>
		/// 128-bit globally unique identifier
		/// </summary>
		Guid = 12,

		/// <summary>
		/// Date and time
		/// </summary>
		DateTime = 13,

		/// <summary>
		/// Length of time
		/// </summary>
		TimeSpan = 14,

		/// <summary>
		/// Raw binary data (array of bytes)
		/// </summary>
		Blob = 15,

		/// <summary>
		/// X and Y size or location as an integer
		/// </summary>
		XY = 16,

		/// <summary>
		/// X, Y, and Z size of location as an integer
		/// </summary>
		XYZ = 17,

		/// <summary>
		/// X and Y-coordinates represented as floating-point numbers
		/// </summary>
		Coordinate2D = 18,

		/// <summary>
		/// X, Y, and Z-coordinates represented as floating-point numbers
		/// </summary>
		Coordinate3D = 19,

		/// <summary>
		/// ARGB color value
		/// </summary>
		Color = 20,

		/// <summary>
		/// List of nodes of the same type
		/// </summary>
		List = 21,

		/// <summary>
		/// Collection of nodes with varying types
		/// </summary>
		Complex = 22

		// TODO: Add 'Reference' type to allow pointing to existing nodes
	}
}
