namespace Frost.Scripting.Compiler
{
	/// <summary>
	/// Classes that tokens can be categorized under
	/// </summary>
	public enum TokenCategory
	{
		/// <summary>
		/// Literal numerical value
		/// </summary>
		Numerical,

		/// <summary>
		/// Built-in script variable type
		/// </summary>
		Type,

		/// <summary>
		/// Static or evaluable string
		/// </summary>
		String,

		/// <summary>
		/// Built-in script keyword that has special meaning
		/// </summary>
		Keyword,

		/// <summary>
		/// Symbols
		/// </summary>
		Punctuation,

		/// <summary>
		/// Mathematical and logical operators
		/// </summary>
		Operator,

		/// <summary>
		/// Custom identifier
		/// </summary>
		Identifier,

		/// <summary>
		/// User comments ignored by the compiler
		/// </summary>
		Comment
	}
}
