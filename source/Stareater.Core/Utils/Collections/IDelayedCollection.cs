using System;

namespace Stareater.Utils.Collections
{
	public interface IDelayedCollection<T>
	{
		/// <summary>
		/// Mark element for adding.
		/// </summary>
		/// <param name="element">Key of element</param>
		void PendAdd(T element);
		
		/// <summary>
		/// Mark element for removal.
		/// </summary>
		/// <param name="element">Key of element</param>
		void PendRemove(T element);
		
		/// <summary>
		/// Remove all elements marked with PendRemove method and add all supplied
		/// through PendAdd.
		/// </summary>
		void ApplyPending();
	}
}
