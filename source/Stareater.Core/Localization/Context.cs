using System.Collections.Generic;
using IKON;

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

		public override void Compose(Composer composer)
		{
			//NoOP
		}

		public override string TypeName
		{
			get { return name; }
		}

		public string this[string entryKey, params double[] variables]
		{
			get { return entries[entryKey.ToLower()].Get(variables); }
		}
	}
}
