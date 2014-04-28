using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

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
			lock(_appearanceModifiers)
			{
				AddAppearanceModifierRule("b", applyBoldTextAppearance);
				AddAppearanceModifierRule("i", applyItalicTextAppearance);
				AddAppearanceModifierRule("u", applyUnderlinedTextAppearance);
				AddAppearanceModifierRule("+", applyIncreaseSizeTextAppearance);
				AddAppearanceModifierRule("-", applyDecreaseSizeTextAppearance);
				AddAppearanceModifierRule("c", applyColorTextAppearance);
			}
		}

		#region Formatting rules

		/// <summary>
		/// Applies bold text appearance
		/// </summary>
		/// <param name="before">Appearance of the text before being modified</param>
		/// <param name="extra">Additional information for formatting (unused)</param>
		/// <returns>Appearance of the text after being modified</returns>
		private static TextAppearance applyBoldTextAppearance (TextAppearance before, string extra)
		{
			var after = before.CloneTextAppearance();
			after.Bold = true;
			return after;
		}

		/// <summary>
		/// Applies italic text appearance
		/// </summary>
		/// <param name="before">Appearance of the text before being modified</param>
		/// <param name="extra">Additional information for formatting (unused)</param>
		/// <returns>Appearance of the text after being modified</returns>
		private static TextAppearance applyItalicTextAppearance (TextAppearance before, string extra)
		{
			var after = before.CloneTextAppearance();
			after.Italic = true;
			return after;
		}

		/// <summary>
		/// Applies underlined text appearance
		/// </summary>
		/// <param name="before">Appearance of the text before being modified</param>
		/// <param name="extra">Additional information for formatting (unused)</param>
		/// <returns>Appearance of the text after being modified</returns>
		private static TextAppearance applyUnderlinedTextAppearance (TextAppearance before, string extra)
		{
			var after = before.CloneTextAppearance();
			after.Underlined = true;
			return after;
		}

		private const int TextSizeChangeAmount = 2;

		/// <summary>
		/// Applies size increase to the text appearance
		/// </summary>
		/// <param name="before">Appearance of the text before being modified</param>
		/// <param name="extra">Amount to increase the size by</param>
		/// <returns>Appearance of the text after being modified</returns>
		private static TextAppearance applyIncreaseSizeTextAppearance (TextAppearance before, string extra)
		{
			var after = before.CloneTextAppearance();
			uint amount;
			if(!UInt32.TryParse(extra, out amount))
				amount = TextSizeChangeAmount;
			after.Size += amount;
			return after;
		}

		/// <summary>
		/// Applies size decrease to the text appearance
		/// </summary>
		/// <param name="before">Appearance of the text before being modified</param>
		/// <param name="extra">Amount to decrease the size by</param>
		/// <returns>Appearance of the text after being modified</returns>
		private static TextAppearance applyDecreaseSizeTextAppearance (TextAppearance before, string extra)
		{
			var after = before.CloneTextAppearance();
			uint amount;
			if(!UInt32.TryParse(extra, out amount))
				amount = TextSizeChangeAmount;
			after.Size -= amount;
			return after;
		}

		/// <summary>
		/// Applies a color change to the text appearance
		/// </summary>
		/// <param name="before">Appearance of the text before being modified</param>
		/// <param name="extra">Color information (hex, triplet of color channels, or color name)</param>
		/// <returns>Appearance of the text after being modified</returns>
		private static TextAppearance applyColorTextAppearance (TextAppearance before, string extra)
		{
			var after = before.CloneTextAppearance();
			after.Color = parseColorString(extra);
			return after;
		}

		private static readonly Regex TripletRegex = new Regex(@"\(?(\d+)(,|\s+|,\s+)(\d+)(,|\s+|,\s+)(\d+)((,|\s+|,\s+)(\d+))?\)?",
																RegexOptions.Compiled);

		private static Color parseColorString (string value)
		{
			if(value != null)
			{
				// Try matching something like:
				// (255, 255, 255)
				var match = TripletRegex.Match(value);
				if(match.Success)
				{
					var redString   = match.Groups[1].Value;
					var greenString = match.Groups[3].Value;
					var blueString  = match.Groups[5].Value;
					var alphaString = match.Groups[8].Value;
					byte red, green, blue, alpha;
					if(!Byte.TryParse(redString, out red))
						red = Byte.MaxValue;
					if(!Byte.TryParse(greenString, out green))
						green = Byte.MaxValue;
					if(!Byte.TryParse(blueString, out blue))
						blue = Byte.MaxValue;
					if(!Byte.TryParse(alphaString, out alpha))
						alpha = Byte.MaxValue;
					return new Color(red, green, blue, alpha);
				}

				// Try matching something like:
				// 0xff00ff
				int hexValue;
				var hexString = value.StartsWith("0x") ? value.Substring(2) : value;
				if(Int32.TryParse(hexString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out hexValue))
					return new Color(hexValue);

				// Last resort is a color name
				return new Color(value);
			}

			return new Color(0x000000); // Black by default
		}
		#endregion
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
			if(formatName == null)
				throw new ArgumentNullException("formatName");
			if(modifier == null)
				throw new ArgumentNullException("modifier");

			var key = formatName.ToLowerInvariant();
			lock(_appearanceModifiers)
				_appearanceModifiers[key] = modifier;
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
			var key = formatName.ToLowerInvariant();
			TextAppearanceModifier modifier;
			lock(_appearanceModifiers)
				if(!_appearanceModifiers.TryGetValue(key, out modifier))
					return null;
			return modifier(before, extra);
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
			if(formatName == null)
				throw new ArgumentNullException("formatName");
			if(translator == null)
				throw new ArgumentNullException("translator");

			var key = formatName.ToLowerInvariant();
			lock(_segmentTranslators)
				_segmentTranslators[key] = translator;
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
			var key = name.ToLowerInvariant();
			SegmentCodeTranslator translator;
			lock(_segmentTranslators)
				if(!_segmentTranslators.TryGetValue(key, out translator))
					return null;
			return translator(appearance, info);
		}
		#endregion
	}
}
