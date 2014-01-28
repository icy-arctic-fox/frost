using System;
using Newtonsoft.Json;

namespace Frost.Modules.Input
{
	/// <summary>
	/// Converts input descriptors to and from JSON
	/// </summary>
	[JsonConverter(typeof(InputDescriptor))]
	public class JsonInputDescriptorConverter : JsonConverter
	{
		/// <summary>
		/// Checks if the type can be converted
		/// </summary>
		/// <param name="objectType">Type to check</param>
		/// <returns>True if <paramref name="objectType"/> is the type <see cref="InputDescriptor"/></returns>
		public override bool CanConvert (Type objectType)
		{
#if MONO
			return objectType.GUID == typeof(InputDescriptor).GUID;
#else
			return objectType == typeof(InputDescriptor);
#endif
		}

		private const string KeyboardType = "Keyboard";
		private const string MouseType    = "Mouse";
		private const string JoystickType = "Joystick"; // TODO: Add joystick serialization

		/// <summary>
		/// Reads the string form of the input descriptor
		/// </summary>
		/// <param name="reader">Json reader</param>
		/// <param name="objectType">Type of object</param>
		/// <param name="existingValue">Existing value of the object being read</param>
		/// <param name="serializer">Json serializer</param>
		/// <returns>Parsed input descriptor</returns>
		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var text  = (string)reader.Value;
			var parts = text.Split(':');
			if(parts.Length != 2)
				throw new FormatException("The format of the input descriptor is expected to be: TYPE:ID");
			
			// Parse the type
			InputType type;
			switch(parts[0])
			{
			case KeyboardType:
				type = InputType.Keyboard;
				break;
			case MouseType:
				type = InputType.Mouse;
				break;
			default:
				throw new FormatException("Unrecognized input type");
			}

			// Parse the value
			var value = Int32.Parse(parts[1]); // Will throw if invalid

			return new InputDescriptor(type, value);
		}

		/// <summary>
		/// Writes the input descriptor to Json format
		/// </summary>
		/// <param name="writer">Json writer</param>
		/// <param name="value">Input descriptor</param>
		/// <param name="serializer">Json serializer</param>
		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			var input = (InputDescriptor)value;
			
			// Stringify the type
			string type;
			switch(input.Type)
			{
			case InputType.Keyboard:
				type = KeyboardType;
				break;
			case InputType.Mouse:
				type = MouseType;
				break;
			default:
				throw new FormatException("Unrecognized input type");
			}

			// Stringify the value
			var v = input.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);

			var text = String.Format("{0}:{1}", type, v);
			writer.WriteValue(text);
		}
	}
}
