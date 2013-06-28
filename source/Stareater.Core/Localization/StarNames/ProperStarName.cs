using System;

namespace Stareater.Localization.StarNames
{
	class ProperStarName : IStarName
	{
		internal const string ContextName = "ProperStarNames";
		private const string KeyPrefix = "proper";
		
		int properNameIndex;
			
		public ProperStarName(int properNameIndex)
		{
			this.properNameIndex = properNameIndex;
		}
		
		public string ToText(Language language)
		{
			return language[ContextName][KeyPrefix + properNameIndex.ToString()].Text();
		}
	}
}
