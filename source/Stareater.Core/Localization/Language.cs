using System.Collections.Generic;

namespace Stareater.Localization
{
	public class Language
	{
		Dictionary<string, Context> contexts = new Dictionary<string, Context>();

		public string Code { get; private set; }

		public Language(string code)
		{

		}
	}
}
