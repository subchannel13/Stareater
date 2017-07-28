using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils.StateEngine;

namespace Stareater.GameData.Ships
{
	[StateType(true)]
	class SpecialEquipmentType : AComponentType
	{
		public string ImagePath { get; private set; }
		
		public Formula Cost { get; private set; }
		public Formula Size { get; private set; }
		public Formula MaxCount { get; private set; }
		
		public SpecialEquipmentType(string code, string languageCode, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel, 
		                 bool canPick, Formula cost, Formula size, Formula maxCount)
			: base(code, languageCode, prerequisites, maxLevel, canPick)
		{
			this.ImagePath = imagePath;
			this.Cost = cost;
			this.Size = size;
			this.MaxCount = maxCount;
		}
		
		public override bool CanHaveMultiple 
		{
			get { return true; }
		}
		
		public Component<SpecialEquipmentType> MakeBest(IDictionary<string, double> techLevels, int quantity)
		{
			return new Component<SpecialEquipmentType>(this, HighestLevel(techLevels), quantity);
		}
	}
}
