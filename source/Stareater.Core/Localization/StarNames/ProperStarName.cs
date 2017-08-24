using Stareater.Utils.StateEngine;

namespace Stareater.Localization.StarNames
{
	[StateType(saveTag: SaveTag)]
	class ProperStarName : IStarName
	{
		internal const string ContextName = "ProperStarNames";
		private const string KeyPrefix = "proper";

		[StateProperty(saveKey: "index")]
		private int nameIndex { get; set; }

		public ProperStarName(int properNameIndex)
		{
			this.nameIndex = properNameIndex;
		}

		private ProperStarName()
		{ }

		public string ToText(Language language)
		{
			return language[ContextName][KeyPrefix + nameIndex.ToString()].Text();
		}

		public const string SaveTag = "Proper";
	}
}