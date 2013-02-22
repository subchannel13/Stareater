using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Utils
{
	class TableSubset<TKey>
	{
		TKey[] keySet;

		public TableSubset(IEnumerable<TKey> keySet)
		{
			this.keySet = keySet.ToArray();
		}

		public IDictionary<TKey, TValue> Extract<TValue>(IDictionary<TKey, TValue> originalData)
		{
			var mappedData = new Dictionary<TKey, TValue>(keySet.Length);

			foreach (var key in keySet)
				mappedData.Add(key, originalData[key]);

			return mappedData;
		}
	}
}
