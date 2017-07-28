using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	public class ObjectIndexer
	{
		private Dictionary<Type, Dictionary<object, int>> indices = new Dictionary<Type, Dictionary<object, int>>();
		
		public void Add<T>(T item)
		{
			Type type = typeof(T);
			
			if (!this.indices.ContainsKey(type))
				this.indices.Add(type, new Dictionary<object, int>());
			
			this.indices[type].Add(item, indices[type].Count);
		}
		
		public void AddAll<T>(IEnumerable<T> items)
		{
			foreach(var item in items)
				this.Add(item);
		}
		
		public int IndexOf<T>(T item)
		{
			return indices[typeof(T)][item];
		}

		public int IndexOf(object item)
		{
			return indices[item.GetType()][item];
		}

		public bool HasType(Type type)
		{
			return this.indices.ContainsKey(type);
		}
	}
}
