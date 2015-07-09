using System;
using System.Collections.Generic;
using Ikadn;

namespace Stareater.Localization
{
	public class Context : IkadnBaseObject
	{
		private readonly string name;
		private readonly Dictionary<string, IText> entries;

		internal Context(string name, Dictionary<string, IText> entries)
		{
			this.name = name.ToLower();
			this.entries = entries;
		}

		protected override void DoCompose(IkadnWriter writer)
		{
			throw new InvalidOperationException("Localization context is not meant to be serialized.");
		}

		public override object Tag
		{
			get { return name; }
		}

		public override T To<T>()
		{
			Type target = typeof(T);

			if (target.IsInstanceOfType(this))
				return (T)(object)this;
			else
				throw new InvalidOperationException("Cast to " + target.Name + " is not supported for " + Tag);
		}

		public ISet<string> KeySet()
		{
			var keys = new HashSet<string>(entries.Keys);
			keys.UnionWith(LocalizationManifest.Get.DefaultLanguage[name].entries.Keys);
			
			return keys;
		}
		
		public bool HasEntry(string entryKey)
		{
			return entries.ContainsKey(entryKey);
		}

		public IText this[string entryKey]
		{
			get {
				if (entryKey == null)
					throw new KeyNotFoundException("Entry key is null");

				entryKey = entryKey.ToLower();
				if (entries.ContainsKey(entryKey))
					return entries[entryKey];
				else if (this != LocalizationManifest.Get.DefaultLanguage[name])
					return LocalizationManifest.Get.DefaultLanguage[name][entryKey];
				else
					throw new KeyNotFoundException("entryKey: " + entryKey);
			}
		}
	}
}
