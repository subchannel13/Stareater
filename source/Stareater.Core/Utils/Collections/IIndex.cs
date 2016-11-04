using System;

namespace Stareater.Utils.Collections
{
	public interface IIndex<T>
	{
		void Add(T item);
		void Remove(T item);
		void Clear();
	}
}
