using System;
using System.Collections.Generic;
using Ikadn;

namespace Stareater.Localization
{
	public class Context : IkadnBaseValue
	{
		private string name;
		private Dictionary<string, IText> entries;

		internal Context(string name, Dictionary<string, IText> entries)
		{
			this.name = name.ToLower();
			this.entries = entries;
		}

		protected override void DoCompose(IkadnWriter writer)
		{
			throw new InvalidOperationException("Localization context is not meant to be serialized.");
		}

		public override string TypeName
		{
			get { return name; }
		}

		public override T To<T>()
		{
			Type target = typeof(T);

			if (target.IsAssignableFrom(this.GetType()))
				return (T)(object)this;
			else
				throw new InvalidOperationException("Cast to " + target.Name + " is not supported for " + TypeName);
		}

		public bool HasEntry(string entryKey)
		{
			return entries.ContainsKey(entryKey);
		}

		public IText this[string entryKey]
		{
			get {
				if (entryKey == null)
					throw new NullReferenceException("Entry key is null");

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
