using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Ships;

namespace Stareater.GameData.Ships
{
	class MissionEquipmentType : AComponentType
	{
		public string ImagePath { get; private set; }
		
		public Formula Cost { get; private set; }
		public Formula Size { get; private set; }
		public AAbilityType[] Abilities { get; private set; }
		
		public MissionEquipmentType(string code, string nameCode, string descCode, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel, Formula cost, 
		                 Formula size, AAbilityType[] abilities)
			: base(code, nameCode, descCode, prerequisites, maxLevel)
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
