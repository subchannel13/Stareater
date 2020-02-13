using Stareater.Utils;
using Stareater.Utils.StateEngine;

namespace Stareater.Localization.StarNames
{
	[StateTypeAttribute(saveTag: SaveTag)]
	class ProperStarName : IStarName
	{
		internal const string ContextName = "ProperStarNames";
		private const string KeyPrefix = "proper";

		[StatePropertyAttribute(saveKey: "index")]
		private int nameIndex { get; set; }

		public ProperStarName(int properNameIndex)
		{
			this.nameIndex = properNameIndex;
		}

		private ProperStarName()
		{ }

		public string ToText(Language language)
		{
			return language[ContextName][KeyPrefix + nameIndex.ToStringInvariant()].Text();
		}

		public const string SaveTag = "Proper";
	}
}