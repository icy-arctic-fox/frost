namespace Frost.IO
{
	/// <summary>
	/// An object that can serialize and deserialize itself using an array of bytes.
	/// Serializable classes *should* have a constructor that accepts a byte array to extract content from.
	/// </summary>
	public interface ISerializable
	{
		/// <summary>
		/// Packs up the contents of the object into an array of bytes
		/// </summary>
		/// <returns>Byte array containing marshaled data from the object</returns>
		byte[] Serialize ();
	}
}
