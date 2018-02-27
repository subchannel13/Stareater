using Stareater.GameData.Ships;
using Stareater.Galaxy.BodyTraits;

namespace Stareater.GameLogic.Combat
{
	class AbilityStats
	{
		public AAbilityType Type { get; private set; }
		public int Level { get; private set; }
		public int Quantity { get; private set; }
		
		public int Range { get; private set; }
		public bool IsInstantDamage { get; private set; }
		public bool IsProjectile { get; private set; }
		public bool TargetColony { get; private set; }
		public bool TargetShips { get; private set; }
		public bool TargetStar { get; private set; }
		
		public double FirePower { get; private set; }
		public double Accuracy { get; private set; }
		public double EnergyCost { get; private set; }
		
		public double AccuracyRangePenalty { get; private set; }
		public double Ammo { get; private set; }
		public double ArmorEfficiency { get; private set; }
		public double ShieldEfficiency { get; private set; }
		public double PlanetEfficiency { get; private set; }
		
		public TraitType AppliesTrait { get; private set; }

		public string ProjectileImage { get; private set; }
		public double Speed { get; private set; }
		public double SplashMaxTargets { get; private set; }
		public double SplashFirePower { get; private set; }
		public double SplashShieldEfficiency { get; private set; }
		public double SplashArmorEfficiency { get; private set; }

		public AbilityStats(AAbilityType type, int level, int quantity,
							int range, bool isInstantDamage, bool isProjectile,
							bool targetColony, bool targetShips, bool targetStar,
							double firePower, double accuracy, double energyCost, double ammo,
							double accuracyRangePenalty, double armorEfficiency, double shieldEfficiency, double planetEfficiency,
							TraitType appliesTrait, string projectileImage, double speed, double splashMaxTargets,
                            double splashFirePower, double splashShieldEfficiency, double splashArmorEfficiency)
		{
			this.Type = type;
			this.Level = level;
			this.Quantity = quantity;

			this.Range = range;
			this.IsInstantDamage = isInstantDamage;
			this.IsProjectile = isProjectile;
			this.TargetColony = targetColony;
			this.TargetShips = targetShips;
			this.TargetStar = targetStar;

			this.FirePower = firePower;
			this.Accuracy = accuracy;
			this.EnergyCost = energyCost;
			this.AccuracyRangePenalty = accuracyRangePenalty;
			this.Ammo = ammo;
			this.ArmorEfficiency = armorEfficiency;
			this.ShieldEfficiency = shieldEfficiency;
			this.PlanetEfficiency = planetEfficiency;

			this.AppliesTrait = appliesTrait;

			this.ProjectileImage = projectileImage;
			this.Speed = speed;
			this.SplashMaxTargets = splashMaxTargets;
            this.SplashFirePower = splashFirePower;
			this.SplashShieldEfficiency = splashShieldEfficiency;
			this.SplashArmorEfficiency = splashArmorEfficiency;
		}
	}
}
