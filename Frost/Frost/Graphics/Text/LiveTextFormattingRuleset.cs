using System;
using System.Collections.Generic;

namespace Frost.Graphics.Text
{
	/// <summary>
	/// Rules for applying text appearance changes and standalone segments
	/// </summary>
	public class LiveTextFormattingRuleset
	{
		#region Default ruleset
		private static readonly object _defaultLocker = new object();
		private static LiveTextFormattingRuleset _defaultRuleset;

		/// <summary>
		/// Retrieves the default live text formatting ruleset
		/// </summary>
		/// <returns>Default ruleset</returns>
		/// <remarks>The default ruleset will have the built-in rules.</remarks>
		/// <seealso cref="ApplyDefaultRules"/>
		public static LiveTextFormattingRuleset GetDefaultRuleset ()
		{
			lock(_defaultLocker)
			{
				if(_defaultRuleset == null)
				{// Default ruleset doesn't exist, create it
					_defaultRuleset = new LiveTextFormattingRuleset();
					_defaultRuleset.ApplyDefaultRules();
				}
				return _defaultRuleset;
			}
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
		#endregion

		#region Appearance modifiers
		private readonly Dictionary<string, TextAppearanceModifier> _appearanceModifiers =
			new Dictionary<string, TextAppearanceModifier>();

		/// <summary>
		/// Describes a method that modifies the appearance of live text
		/// </summary>
		/// <param name="before">Appearance of the text before being modified</param>
		/// <param name="extra">Additional information for formatting (may be null)</param>
		/// <returns>Appearance of the text after being modified</returns>
		/// <remarks><paramref name="before"/> must be cloned and the clone must be edited.
		/// Do not modify the contents of <paramref name="before"/>.</remarks>
		public delegate TextAppearance TextAppearanceModifier (TextAppearance before, string extra);

		/// <summary>
		/// Adds a rule that will change the appearance of text
		/// </summary>
		/// <param name="formatName">Name of the formatting code</param>
		/// <param name="modifier">Method executed to apply the appearance change</param>
		/// <exception cref="ArgumentNullException">The <paramref name="formatName"/> and <paramref name="modifier"/> can't be null.</exception>
		public void AddAppearanceModifierRule (string formatName, TextAppearanceModifier modifier)
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
		#endregion

		#region Segment codes
		private readonly Dictionary<string, SegmentCodeTranslator> _segmentTranslators =
			new Dictionary<string, SegmentCodeTranslator>();

		/// <summary>
		/// Describes a method that creates a live text segment from a formatting code
		/// </summary>
		/// <param name="appearance">Current appearance of the text when the formatting code was encountered</param>
		/// <param name="info">Additional information for formatting</param>
		/// <returns>Newly created segment</returns>
		/// <remarks>Do not modify the contents of <paramref name="appearance"/>.</remarks>
		public delegate ILiveTextSegment SegmentCodeTranslator (TextAppearance appearance, string info);

		/// <summary>
		/// Adds a rule that will create a segment from a formatting code
		/// </summary>
		/// <param name="formatName">Name of the formatting code</param>
		/// <param name="translator">Method that will construct the segment from the formatting code</param>
		/// <exception cref="ArgumentNullException">The <paramref name="formatName"/> and <paramref name="translator"/> can't be null.</exception>
		public void AddSegmentCodeRule (string formatName, SegmentCodeTranslator translator)
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
		#endregion
	}
}
