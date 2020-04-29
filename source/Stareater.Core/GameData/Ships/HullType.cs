using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils.StateEngine;

namespace Stareater.GameData.Ships
{
	[StateTypeAttribute(true)]
	class HullType : AComponentType
	{
		public const string IsDriveSizeKey = "hullIsSize"; //base hull's IS drive size

		public ReadOnlyCollection<string> ImagePaths { get; private set; }
		public Formula Cost { get; private set; }
	
		public Formula Size { get; private set; }
		public Formula SpaceFree { get; private set; }
		
		public Formula SizeIS { get; private set; }
		public Formula SizeShield { get; private set; }
		
		public Formula ArmorBase { get; private set; }
		public Formula ArmorAbsorption { get; private set; }
		public Formula ShieldBase { get; private set; }
		
		public Formula InertiaBase { get; private set; }
		public Formula JammingBase { get; private set; }
		public Formula CloakingBase { get; private set; }
		public Formula SensorsBase { get; private set; }
		
		public HullType(string code, string languageCode, string[] imagePaths,
		                IEnumerable<Prerequisite> prerequisites, int maxLevel, bool canPick, Formula cost, 
		                Formula size, Formula spaceFree, 
		                Formula sizeIS, Formula sizeShield,
		                Formula armorBase, Formula armorAbsorption, Formula shieldBase, 
		                Formula inertiaBase, Formula jammingBase, Formula cloakingBase, Formula sensorsBase)
			: base(code, languageCode, prerequisites, maxLevel, canPick)
		{
			this.ImagePaths = Array.AsReadOnly(imagePaths);
			this.Cost = cost;
			this.Size = size;
			this.SpaceFree = spaceFree;
			this.SizeIS = sizeIS;
			this.SizeShield = sizeShield;
			this.ArmorBase = armorBase;
			this.ArmorAbsorption = armorAbsorption;
			this.ShieldBase = shieldBase;
			this.InertiaBase = inertiaBase;
			this.JammingBase = jammingBase;
			this.CloakingBase = cloakingBase;
			this.SensorsBase = sensorsBase;
		}
		
		public Component<HullType> MakeHull(IDictionary<string, double> techLevels)
		{
			return new Component<HullType>(this, HighestLevel(techLevels));
		}
	}
}
