using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace Stareater.Maps
{
	public struct MapFactoryParameterInfo : IEnumerable<KeyValuePair<int, string>>
	{
		private IList<KeyValuePair<int, string>> values;
		private string nameKey;

		public MapFactoryParameterInfo(string nameKey, IEnumerable<KeyValuePair<int, string>> values)
		{
			this.nameKey = nameKey;
			this.values = new ReadOnlyCollection<KeyValuePair<int, string>>(values.ToList());
		}

		public int Count
		{
			get { return values.Count; }
		}

		public KeyValuePair<int, string> this[int optionIndex]
		{
			get { return values[optionIndex]; }
		}

		public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
		{
			return values.GetEnumerator();
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return values.GetEnumerator();
		}
	}
}
