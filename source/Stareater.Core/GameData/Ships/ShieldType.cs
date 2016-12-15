using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Ships;

namespace Stareater.GameData.Ships
{
	class ShieldType : AComponentType
	{
		public string ImagePath { get; private set; }
		public Formula Cost { get; private set; }
		
		public Formula HpFactor { get; private set; }
		public Formula RegenerationFactor { get; private set; }
		public Formula Thickness { get; private set; }
		public Formula Reduction { get; private set; }
		
		public Formula Cloaking { get; private set; }
		public Formula Jamming { get; private set; }
		
		public Formula PowerUsage { get; private set; }
		
		public ShieldType(string code, string nameCode, string descCode, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel, 
		                 bool canPick, Formula cost, Formula hpFactor, Formula regenerationFactor, 
		                 Formula thickness, Formula reduction,
		                 Formula cloaking, Formula jamming, Formula powerUsage)
			: base(code, nameCode, descCode, prerequisites, maxLevel, canPick)
		{
			this.ImagePath = imagePath;
			this.Cost = cost;
			this.HpFactor = hpFactor;
			this.RegenerationFactor = regenerationFactor;
			this.Thickness = thickness;
			this.Reduction = reduction;
			this.Cloaking = cloaking;
			this.Jamming = jamming;
			this.PowerUsage = powerUsage;
		}
		
		public Component<ShieldType> MakeBest(IDictionary<string, double> techLevels)
		{
			return new Component<ShieldType>(this, HighestLevel(techLevels));
		}
	}
}
