using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Ships;

namespace Stareater.GameData
{
	class HullType : AComponentType
	{
		public string[] ImagePaths { get; private set; }
		public Formula Cost { get; private set; }
	
		public Formula Size { get; private set; }
		public Formula SpaceFree { get; private set; }
		
		public Formula SizeIS { get; private set; }
		public Formula SizeReactor { get; private set; }
		public Formula SizeShield { get; private set; }
		
		public Formula ArmorBase { get; private set; }
		public Formula ArmorAbsorption { get; private set; }
		public Formula ShieldBase { get; private set; }
		
		public Formula InertiaBase { get; private set; }
		public Formula JammingBase { get; private set; }
		public Formula CloakingBase { get; private set; }
		public Formula SensorsBase { get; private set; }
		
		public HullType(string nameCode, string descCode, string[] imagePaths,
		                IEnumerable<Prerequisite> prerequisites, int maxLevel, Formula cost, 
		                Formula size, Formula spaceFree, 
		                Formula sizeIS, Formula sizeReactor, Formula sizeShield,
		                Formula armorBase, Formula armorAbsorption, Formula shieldBase, 
		                Formula inertiaBase, Formula jammingBase, Formula cloakingBase, Formula sensorsBase)
			: base(nameCode, descCode, prerequisites, maxLevel)
		{
			this.ImagePaths = imagePaths;
			this.Cost = cost;
			this.Size = size;
			this.SpaceFree = spaceFree;
			this.SizeIS = sizeIS;
			this.SizeReactor = sizeReactor;
			this.SizeShield = sizeShield;
			this.ArmorBase = armorBase;
			this.ArmorAbsorption = armorAbsorption;
			this.ShieldBase = shieldBase;
			this.InertiaBase = inertiaBase;
			this.JammingBase = jammingBase;
			this.CloakingBase = cloakingBase;
			this.SensorsBase = sensorsBase;
		}
		
		public Hull MakeHull(IDictionary<string, int> techLevels, int imageIndex)
		{
			return new Hull(this, HighestLevel(techLevels), imageIndex);
		}
	}
}
