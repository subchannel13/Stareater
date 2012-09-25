using System.Collections.Generic;
using IKON;

namespace Stareater.Localization
{
	public class Context : Value
	{
		private string name;
		private Dictionary<string, Value> entries = new Dictionary<string, Value>();

		internal Context(string name)
		{
			this.name = name;
		}

		public Value this[string memberName]
		{
			get { return entries[memberName]; }
			internal set
			{
				if (entries.ContainsKey(memberName))
					entries[memberName] = value;
				else
					entries.Add(memberName, value);
			}
		}

		public override void Compose(Composer composer)
		{
			//NoOP
		}

		public override string TypeName
		{
			get { return "Context"; }
		}
	}
}
