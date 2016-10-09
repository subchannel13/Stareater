using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Localization
{
	public class LanguageInfo
	{
		public string Id { get; private set; }
		public string Name { get; private set; }

		public LanguageInfo(string id, string name)
		{
			this.Id = id;
			this.Name = name;
		}
	}
}
