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
			return objectType == typeof(InputDescriptor); // TODO: Change to Mono compatible code
		}

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
			var text = (string)reader.Value;
			throw new NotImplementedException();
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
			var text = String.Empty; // TODO
			writer.WriteValue(text);
		}
	}
}
