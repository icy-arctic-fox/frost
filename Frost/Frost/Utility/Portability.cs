using System;

namespace Frost.Utility
{
	/// <summary>
	/// Methods that enable portability across platforms
	/// </summary>
	public static class Portability
	{
		/// <summary>
		/// Compares two types for equality.
		/// Mono does not support directly comparing two types,
		/// so a workaround is used for Mono compilations.
		/// </summary>
		/// <param name="left">First type to compare</param>
		/// <param name="right">Second type to compare</param>
		/// <returns>True if <paramref name="left"/> and <paramref name="right"/> are the same type</returns>
		public static bool CompareTypes (Type left, Type right)
		{
#if MONO
			return left.GUID == right.GUID;
#else
			return left == right;
#endif
		}
	}
}
