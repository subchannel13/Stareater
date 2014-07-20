using System;
using Ikadn.Ikon.Types;

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

		#region Saving
		public Ikadn.IkadnBaseObject Save()
		{
			IkonComposite data = new IkonComposite(SaveTag);
			data.Add(IndexKey, new IkonInteger(this.properNameIndex));
			
			return data;
		}

		public const string SaveTag = "Proper";
		private const string IndexKey = "index";
		#endregion
	}
}
