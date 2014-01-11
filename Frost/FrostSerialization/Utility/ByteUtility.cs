using System;

namespace Frost.Utility
{
	/// <summary>
	/// Useful methods for manipulating byte arrays
	/// </summary>
	public static class ByteUtility
	{
		/// <summary>
		/// Copies bytes from one array to another
		/// </summary>
		/// <remarks>A negative count will copy bytes until the end of either array is reached.</remarks>
		/// <param name="src">Array to copy bytes from</param>
		/// <param name="dest">Array to put bytes into</param>
		/// <param name="srcStart">Starting position in the source array (default is 0)</param>
		/// <param name="destStart">Starting position in the destination array (default is 0)</param>
		/// <param name="count">Number of bytes to copy (default is -1)</param>
		public static void Copy (this byte[] src, byte[] dest, int srcStart = 0, int destStart = 0, int count = -1)
		{
			if(0 > count) // Calculate where to stop
				count = Math.Min(src.Length - srcStart, dest.Length - destStart);

#if UNSAFE
			unsafe
			{
				fixed(byte* pSrc = src, pDest = dest)
				{
					var ps = pSrc  + srcStart;
					var pd = pDest + destStart;
#if X64
					var stop = count / sizeof(long);
#else
					var stop = count / sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = *((long*)ps);
						pd += sizeof(long);
						ps += sizeof(long);
					}
					stop = count % sizeof(long);
#else
						*((int*)pd) = *((int*)ps);
						pd += sizeof(int);
						ps += sizeof(int);
					}
					stop = count % sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
						*pd = *ps;
						++pd;
						++ps;
					}
				}
			}
#else
			Buffer.BlockCopy(src, srcStart, dest, destStart, count);
#endif
		}

		/// <summary>
		/// Creates a copy of an array of bytes
		/// </summary>
		/// <param name="src">Original byte array</param>
		/// <returns>A new byte array containing the same contents as <paramref name="src"/></returns>
		public static byte[] Duplicate (this byte[] src)
		{
			var dest = new byte[src.Length];
			src.Copy(dest);
			return dest;
		}
	}
}
