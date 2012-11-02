using System.Collections.Generic;
using Ikon;

namespace Stareater.Localization
{
	public class Context : Value
	{
		private string name;
		private Dictionary<string, IText> entries;

		internal Context(string name, Dictionary<string, IText> entries)
		{
			this.name = name.ToLower();
			this.entries = entries;
		}

		protected override void DoCompose(IkonWriter writer)
		{
			//NoOP
		}

		public override string TypeName
		{
			get { return name; }
		}

		public bool HasEntry(string entryKey)
		{
			return entries.ContainsKey(entryKey);
		}

		public string this[string entryKey, params double[] variables]
		{
			get {
				entryKey = entryKey.ToLower();
				if (entries.ContainsKey(entryKey))
					return entries[entryKey].Get(variables);
				else if (this != LocalizationManifest.DefaultLanguage[name])
					return LocalizationManifest.DefaultLanguage[name][entryKey, variables];
				else
					throw new KeyNotFoundException("entryKey: " + entryKey);
			}
		}
	}
}
