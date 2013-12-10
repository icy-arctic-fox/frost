namespace Frost
{
	/// <summary>
	/// Some item that can be canceled
	/// </summary>
	/// <remarks>Once an item has been canceled,
	/// it should not be be resumed or be un-cancellable.</remarks>
	public interface ICancellable
	{
		/// <summary>
		/// Indicate that the item has been canceled
		/// </summary>
		bool IsCanceled { get; }

		/// <summary>
		/// Cancels the item (or event) from happening
		/// </summary>
		void Cancel ();
	}
}
