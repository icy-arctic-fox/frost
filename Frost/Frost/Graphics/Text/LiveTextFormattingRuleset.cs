using System;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Rules for applying text appearance changes and standalone segments
	/// </summary>
	public class LiveTextFormattingRuleset
	{
		/// <summary>
		/// Retrieves the default live text formatting ruleset
		/// </summary>
		/// <returns>Default ruleset</returns>
		/// <remarks>The default ruleset will have the built-in rules.</remarks>
		/// <seealso cref="ApplyDefaultRules"/>
		public static LiveTextFormattingRuleset GetDefaultRuleset ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds the default built-in rules to the ruleset
		/// </summary>
		/// <remarks>Any conflicting rules (existing rules with the same name) will be overwritten.</remarks>
		/// <seealso cref="GetDefaultRuleset"/>
		public void ApplyDefaultRules ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Applies changes to text appearance given formatting code information
		/// </summary>
		/// <param name="formatName">Name of the formatter</param>
		/// <param name="extra">Additional information for formatting</param>
		/// <param name="before">Appearance of the text prior to the change</param>
		/// <returns>Appearance of the text after the change</returns>
		internal TextAppearance TranslateFormattingCode (string formatName, string extra, TextAppearance before)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a segment from a formatting code
		/// </summary>
		/// <param name="name">Name of the formatter</param>
		/// <param name="info">Additional information</param>
		/// <param name="appearance">Current text appearance</param>
		/// <returns>Newly created segment</returns>
		internal ILiveTextSegment TranslateSegmentCode (string name, string info, TextAppearance appearance)
		{
			throw new NotImplementedException();
		}
	}
}
