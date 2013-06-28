using System;

namespace Stareater.Localization.StarNames
{
	class ProperStarName : IStarName
	{
		const string ContextName = "ProperStarNames";
		const string KeyPrefix = "proper";
		
		int properNameIndex;
			
		public ProperStarName(int properNameIndex)
		{
			this.properNameIndex = properNameIndex;
		}
		
		public string Name(Language language)
		{
			return language[ContextName][KeyPrefix + properNameIndex.ToString()].Text();
		}
	}
}
