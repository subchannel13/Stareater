using Stareater.AppData.Expressions;

namespace Stareater.GameData.Ships
{
	class ProjectileAbility : AAbilityType
	{
		//TODO(later) consider converting to child ability
		public Formula FirePower { get; private set; }
		public Formula Accuracy { get; private set; }

		public Formula Ammo { get; private set; }
		public Formula Speed { get; private set; } //TODO(later) consider making virtual ship design

		public Formula ArmorEfficiency { get; private set; }
		public Formula ShieldEfficiency { get; private set; }
		public Formula PlanetEfficiency { get; private set; }

		public Formula SplashMaxTargets { get; private set; }
		public Formula SplashFirePower { get; private set; }
		public Formula SplashArmorEfficiency { get; private set; }
		public Formula SplashShieldEfficiency { get; private set; }
		public Formula SplashPlanetEfficiency { get; private set; }

		public string ProjectileImage { get; private set; }

		public ProjectileAbility(string imagePath,
								 Formula firePower, Formula accuracy, Formula ammo, Formula speed,
								 Formula armorEfficiency, Formula shieldEfficiency, Formula planetEfficiency,
								 Formula splashMaxTargets, Formula splashFirePower,
								 Formula splashArmorEfficiency, Formula splashShieldEfficiency,
								 string projectileImage)
			: base(imagePath)
		{
			this.FirePower = firePower;
			this.Accuracy = accuracy;
			this.Ammo = ammo;
			this.Speed = speed;
			this.ArmorEfficiency = armorEfficiency;
			this.ShieldEfficiency = shieldEfficiency;
			this.PlanetEfficiency = planetEfficiency;
			this.SplashMaxTargets = splashMaxTargets;
			this.SplashFirePower = splashFirePower;
			this.SplashArmorEfficiency = splashArmorEfficiency;
			this.SplashShieldEfficiency = splashShieldEfficiency;
			this.ProjectileImage = projectileImage;
		}

		public override void Accept(IAbilityVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
