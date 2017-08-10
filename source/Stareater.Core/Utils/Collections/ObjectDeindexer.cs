using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	public class ObjectDeindexer
	{
		private Dictionary<Type, List<object>> indices = new Dictionary<Type, List<object>>();
		private Dictionary<Type, Dictionary<string, object>> ids = new Dictionary<Type, Dictionary<string, object>>();
		
		public void Add<T>(T item)
		{
			Type type = typeof(T);
			
			if (!this.indices.ContainsKey(type))
				this.indices.Add(type, new List<object>());
			
			this.indices[type].Add(item);
		}
		
		public void Add<T>(T item, string id)
		{
			Type type = typeof(T);
			
			if (!this.ids.ContainsKey(type))
				this.ids.Add(type, new Dictionary<string, object>());
			
			this.ids[type].Add(id, item);
		}

		public void AddAll<T>(IEnumerable<T> items)
		{
			foreach(var item in items)
				this.Add(item);
		}
		
		public void AddAll<T>(IEnumerable<T> items, Func<T, string> idMethod)
		{
			foreach(var item in items)
				this.Add(item, idMethod(item));
		}

		public bool HasType(Type type)
		{
			return this.indices.ContainsKey(type);
		}

		public T Get<T>(int index)
		{
			return (T)this.indices[typeof(T)][index];
		}
		
		public T Get<T>(string id)
		{
			return (T)this.ids[typeof(T)][id];
		}
	}
}
