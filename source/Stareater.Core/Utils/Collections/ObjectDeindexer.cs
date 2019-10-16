using Stareater.Utils.StateEngine;
using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	public class ObjectDeindexer
	{
		private readonly Dictionary<Type, Dictionary<string, object>> ids = new Dictionary<Type, Dictionary<string, object>>();
		
		public void Add<T>(T item, string id)
		{
			Type type = typeof(T);
			
			if (!this.ids.ContainsKey(type))
				this.ids.Add(type, new Dictionary<string, object>());
			
			this.ids[type].Add(id, item);
		}

		public void AddAll<T>(IEnumerable<T> items) where T: IIdentifiable
		{
			foreach(var item in items)
				this.Add(item, item.IdCode);
		}

		public bool HasType(Type type)
		{
			return this.ids.ContainsKey(type);
		}
		
		public T Get<T>(string id)
		{
			return (T)this.ids[typeof(T)][id];
		}

		public object Get(Type type, string id)
		{
			return this.ids[type][id];
		}
	}
}
