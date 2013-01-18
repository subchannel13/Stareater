using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Stareater.AppData;

namespace Stareater.Maps
{
	public struct MapFactoryParameterInfo : IEnumerable<KeyValuePair<int, string>>
	{
		private IList<KeyValuePair<int, string>> values;
		private string contextKey;
		private string nameKey;

		public MapFactoryParameterInfo(string contextKey, string nameKey, IEnumerable<KeyValuePair<int, string>> values)
		{
			this.contextKey = contextKey;
			this.nameKey = nameKey;
			this.values = new ReadOnlyCollection<KeyValuePair<int, string>>(values.ToList());
		}

		public int Count
		{
			get { return values.Count; }
		}

		public string Name
		{
			get { return Settings.Get.Language[contextKey][nameKey]; }
		}

		public KeyValuePair<int, string> this[int optionIndex]
		{
			get { return values[optionIndex]; }
		}

		public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
		{
			foreach (var unlocalizedValue in values)
				yield return new KeyValuePair<int, string>(
					unlocalizedValue.Key,
					Settings.Get.Language[contextKey][unlocalizedValue.Value]);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotSupportedException("Use generic method instead");
		}
	}
}
