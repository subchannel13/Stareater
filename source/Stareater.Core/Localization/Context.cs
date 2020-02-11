using Ikadn;
using System;
using System.Collections.Generic;

namespace Stareater.Localization
{
	public class Context : IkadnBaseObject
	{
		private readonly string name;
		private readonly Dictionary<string, IText> entries;

		internal Context(string name, Dictionary<string, IText> entries)
		{
			this.name = name.ToUpperInvariant();
			this.entries = entries;
		}

		protected override void DoCompose(IkadnWriter writer)
		{
#pragma warning disable CA1303 // Do not pass literals as localized parameters
			throw new InvalidOperationException("Localization context is not meant to be serialized.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
		}

		public override object Tag
		{
			get { return this.name; }
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
			var keys = new HashSet<string>(this.entries.Keys);
			keys.UnionWith(LocalizationManifest.Get.DefaultLanguage[this.name].entries.Keys);
			
			return keys;
		}
		
		public bool HasEntry(string entryKey)
		{
			return this.entries.ContainsKey(entryKey);
		}

		public IText Description(string entryKey)
		{
			return this[entryKey + "_DESC"];
		}

		public IText Name(string entryKey)
		{
			return this[entryKey + "_NAME"];
		}

		public IText this[string entryKey]
		{
			get {
				if (entryKey == null)
#pragma warning disable CA1303 // Do not pass literals as localized parameters
					throw new KeyNotFoundException("Entry key is null");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

				entryKey = entryKey.ToUpperInvariant();
				if (this.entries.ContainsKey(entryKey))
					return this.entries[entryKey];
				else if (this != LocalizationManifest.Get.DefaultLanguage[this.name])
					return LocalizationManifest.Get.DefaultLanguage[this.name][entryKey];
				else
					throw new KeyNotFoundException("entryKey: " + entryKey + " context: " + this.name);
			}
		}
	}
}
