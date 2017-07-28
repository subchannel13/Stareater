using Ikadn.Ikon.Types;
using Stareater.Utils.StateEngine;

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
		public IkonBaseObject Save(SaveSession session)
		{
			IkonComposite data = new IkonComposite(SaveTag);
			data.Add(IndexKey, new IkonInteger(this.properNameIndex));

			return data;
		}

		//TODO(v0.7) remove
		public IkonBaseObject Save()
		{
			IkonComposite data = new IkonComposite(SaveTag);
			data.Add(IndexKey, new IkonInteger(this.properNameIndex));
			
			return data;
		}

		public static IStarName Load(IkonComposite rawData)
		{
			return new ProperStarName(
				rawData[IndexKey].To<int>()
			);
		}
		
		public const string SaveTag = "Proper";
		private const string IndexKey = "index";
		#endregion
	}
}
