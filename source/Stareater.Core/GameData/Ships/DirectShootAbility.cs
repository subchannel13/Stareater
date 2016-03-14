using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Ships
{
	class DirectShootAbility : AAbilityType
	{
		public Formula FirePower { get; private set; }
		public Formula Accuracy { get; private set; }
		public Formula ShieldEfficiency { get; private set; } //TODO(v0.5) make it optional in data file
		public Formula ArmorEfficiency { get; private set; } //TODO(v0.5) make it optional in data file
		public Formula EnergyCost { get; private set; }
		
		public DirectShootAbility(string imagePath, Formula firePower, Formula accuracy,
		                          Formula shieldEfficiency, Formula armorEfficiency, Formula energyCost)
			: base(imagePath)
		{
			this.FirePower = firePower;
			this.Accuracy = accuracy;
			this.ShieldEfficiency = shieldEfficiency;
			this.ArmorEfficiency = armorEfficiency;
			this.EnergyCost = energyCost;
		}
	}
}
