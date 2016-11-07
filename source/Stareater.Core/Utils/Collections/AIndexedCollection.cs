using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	class AIndexedCollection<T> : ICollection<T>, IDelayedCollection<T>
	{
		private HashSet<T> innerSet = new HashSet<T>();
		private readonly List<T> toAdd = new List<T>();
		private readonly List<T> toRemove = new List<T>();
		private readonly List<IIndex<T>> indices = new List<IIndex<T>>();

		protected void RegisterIndices(params IIndex<T>[] index)
		{
			this.indices.AddRange(index);
		}
		
		public void Add(IEnumerable<T> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		#region ICollection implementation
		public void Add(T item)
		{
			innerSet.Add(item); 
			
			foreach(var index in this.indices)
				index.Add(item);
		}
		public void Clear()
		{
			innerSet.Clear();
			
			foreach(var index in this.indices)
				index.Clear();
		}
		public bool Contains(T item)
		{
			return innerSet.Contains(item);
		}
		public void CopyTo(T[] array, int arrayIndex)
		{
			innerSet.CopyTo(array, arrayIndex);
		}
		public bool Remove(T item)
		{
			if (innerSet.Remove(item)) 
			{
				foreach(var index in this.indices)
					index.Remove(item);
			
				return true;
			}

			return false;
		}
		public int Count 
		{
			get { return innerSet.Count; }
		}
		public bool IsReadOnly 
		{
			get { return false; }
		}
		#endregion
		
		#region IEnumerable implementation
		public IEnumerator<T> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}
		#endregion
		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}
		#endregion
		#region IDelayedCollection implementation
		public void PendAdd(T element)
		{
			toAdd.Add(element);
		}

		public void PendRemove(T element)
		{
			toRemove.Add(element);
		}

		public void ApplyPending()
		{
			foreach (var element in toRemove)
				this.Remove(element);
			toRemove.Clear();
			
			foreach (var element in toAdd)
				this.Add(element);
			toAdd.Clear();
		}
		#endregion
	}
}
