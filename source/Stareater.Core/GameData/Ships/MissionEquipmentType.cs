using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils.StateEngine;

namespace Stareater.GameData.Ships
{
	[StateTypeAttribute(true)]
	class MissionEquipmentType : AComponentType
	{
		public string ImagePath { get; private set; }
		
		public Formula Cost { get; private set; }
		public Formula Size { get; private set; }
		public AAbilityType[] Abilities { get; private set; }
		
		public MissionEquipmentType(string code, string languageCode, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel, Formula cost, 
		                 bool canPick, Formula size, AAbilityType[] abilities)
			: base(code, languageCode, prerequisites, maxLevel, canPick)
		{
			this.ImagePath = imagePath;
			
			this.Cost = cost;
			this.Size = size;
			this.Abilities = abilities;
		}
		
		public override bool CanHaveMultiple 
		{
			get { return true; }
		}
		
		public Component<MissionEquipmentType> MakeBest(IDictionary<string, double> techLevels, int quantity)
		{
			return new Component<MissionEquipmentType>(this, HighestLevel(techLevels), quantity);
		}
	}
}
