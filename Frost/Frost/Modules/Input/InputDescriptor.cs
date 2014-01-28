namespace Frost.Modules.Input
{
	/// <summary>
	/// Information about an input source and input ID (button/key)
	/// </summary>
	public struct InputDescriptor
	{
		private readonly InputType _type;
		private readonly int _id;

		/// <summary>
		/// Type of input
		/// </summary>
		public InputType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// ID associated with the input
		/// </summary>
		public int Id
		{
			get { return _id; }
		}

		/// <summary>
		/// Creates a new input ID
		/// </summary>
		/// <param name="type">Type of input</param>
		/// <param name="id">ID (button/key) associated with the input</param>
		public InputDescriptor (InputType type, int id)
		{
			_type = type;
			_id   = id;
		}
	}
}
