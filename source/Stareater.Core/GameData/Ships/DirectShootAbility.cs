using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Ships
{
	class DirectShootAbility : AAbilityType
	{
		public Formula FirePower { get; private set; }
		public Formula Accuracy { get; private set; }
		public Formula Range { get; private set; }
		public Formula EnergyCost { get; private set; }
		
		public Formula ArmorEfficiency { get; private set; }
		public Formula ShieldEfficiency { get; private set; }
		public Formula PlanetEfficiency { get; private set; }
		
		public DirectShootAbility(string imagePath, 
		                          Formula firePower, Formula accuracy, Formula range, Formula energyCost,
		                          Formula armorEfficiency, Formula shieldEfficiency, Formula planetEfficiency)
			: base(imagePath)
		{
			this.FirePower = firePower;
			this.Accuracy = accuracy;
			this.Range = range;
			this.EnergyCost = energyCost;
			this.ArmorEfficiency = armorEfficiency;
			this.ShieldEfficiency = shieldEfficiency;
			this.PlanetEfficiency = planetEfficiency;
		}
		
		public override void Accept(IAbilityVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
