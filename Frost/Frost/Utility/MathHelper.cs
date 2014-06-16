namespace Frost.Utility
{
	/// <summary>
	/// Utility methods for fast math calculations and common math operations
	/// </summary>
	public static class MathHelper
	{
		/// <summary>
		/// Corrects an angle in degrees so that it is between 0 and 360
		/// </summary>
		/// <param name="angle">Angle to correct</param>
		/// <returns>Corrected angle</returns>
		public static float CorrectAngle (float angle)
		{
			var corrected = angle % 360;
			if(corrected < 0)
				corrected += 360;
			return corrected;
		}
	}
}
