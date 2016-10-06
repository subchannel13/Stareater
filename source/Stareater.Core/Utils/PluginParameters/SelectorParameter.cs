using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections;
using Stareater.Localization;

namespace Stareater.Utils.PluginParameters
{
	public class SelectorParameter : ParameterBase, IEnumerable<KeyValuePair<int, string>>
	{
		public int Value { get; set; }
		private IList<KeyValuePair<int, string>> values;

		public SelectorParameter(string contextKey, string nameKey, IEnumerable<KeyValuePair<int, string>> values, int current) :
			base(contextKey, nameKey)
		{
			this.values = new ReadOnlyCollection<KeyValuePair<int, string>>(values.ToList());
			this.Value = current;
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
			foreach (var unlocalizedValue in values)
				yield return new KeyValuePair<int, string>(
					unlocalizedValue.Key,
					LocalizationManifest.Get.CurrentLanguage[contextKey][unlocalizedValue.Value].Text());
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotSupportedException("Use generic method instead");
		}
	}
}
