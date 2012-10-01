namespace Stareater.Collections
{
	public interface IDelayedRemoval<T>
	{
		/// <summary>
		/// Mark element for removal.
		/// </summary>
		/// <param name="key">Key of element</param>
		void PendRemove(T element);


		/// <summary>
		/// Remove all elements marked with PendRemove method.
		/// </summary>
		void ApplyRemove();
	}
}
