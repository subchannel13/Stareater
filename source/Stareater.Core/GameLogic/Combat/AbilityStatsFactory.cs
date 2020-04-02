using Stareater.GameData.Databases;
using Stareater.GameData.Ships;
using Stareater.Utils.Collections;
using Stareater.Galaxy.BodyTraits;

namespace Stareater.GameLogic.Combat
{
	class AbilityStatsFactory : IAbilityVisitor
	{
		private readonly int level;
		private readonly StaticsDB statics;
		
		private int range = 0;
		private bool isInstantDamage = false;
		private bool isProjectile = false;
		private bool targetColony = false;
		private bool targetShips = false;
		private bool targetStar = false;
		
		private double firePower = 0;
		private double accuracy = 0;
		private double energyCost = 0;
		private double accuracyRangePenalty = 0;
		private double ammo = 0;
		private double armorEfficiency = 0;
		private double shieldEfficiency = 0;
		private double planetEfficiency = 0;
		
		private StarTraitType appliesTrait = null;

		private string projectileImage = null;
		private double speed = 0;
		private double splashMaxTargets = 0;
		private double splashFirePower = 0;
		private double splashShieldEfficiency = 0;
		private double splashArmorEfficiency = 0;

		private AbilityStatsFactory(int level, StaticsDB statics)
		{
			this.level = level;
			this.statics = statics;
		}
		
		public static AbilityStats Create(AAbilityType type, AComponentType provider, int level, int quantity, StaticsDB statics)
		{
			var factory = new AbilityStatsFactory(level, statics);
			type.Accept(factory);
			
			return new AbilityStats(type, provider, level, quantity, factory.range, factory.isInstantDamage, factory.isProjectile,
								   factory.targetColony, factory.targetShips, factory.targetStar,
			                       factory.firePower, factory.accuracy, factory.energyCost, factory.ammo,
			                       factory.accuracyRangePenalty, factory.armorEfficiency, factory.shieldEfficiency, factory.planetEfficiency,
			                       factory.appliesTrait, factory.projectileImage, factory.speed, factory.splashMaxTargets,
								   factory.splashFirePower, factory.splashShieldEfficiency, factory.splashArmorEfficiency);
		}

		#region IAbilityVisitor implementation

		public void Visit(DirectShootAbility ability)
		{
			var vars = new Var(AComponentType.LevelKey, this.level).Get;
			
			this.range = (int)ability.Range.Evaluate(vars);
			this.isInstantDamage = true;
			this.targetColony = true;
			this.targetShips = true;
			
			this.firePower = ability.FirePower.Evaluate(vars);
			this.accuracy = ability.Accuracy.Evaluate(vars);
			this.energyCost = ability.EnergyCost.Evaluate(vars);
			this.ammo = ability.Ammo.Evaluate(vars);
			this.accuracyRangePenalty = ability.AccuracyRangePenalty.Evaluate(vars);
			this.armorEfficiency = ability.ArmorEfficiency.Evaluate(vars);
			this.shieldEfficiency = ability.ShieldEfficiency.Evaluate(vars);
			this.planetEfficiency = ability.PlanetEfficiency.Evaluate(vars);
		}

		public void Visit(ProjectileAbility ability)
		{
			var vars = new Var(AComponentType.LevelKey, this.level).Get;

			this.range = int.MaxValue;
			this.isProjectile = true;
			this.targetColony = true;
			this.targetShips = true;

			this.firePower = ability.FirePower.Evaluate(vars);
			this.accuracy = ability.Accuracy.Evaluate(vars);
			this.armorEfficiency = ability.ArmorEfficiency.Evaluate(vars);
			this.shieldEfficiency = ability.ShieldEfficiency.Evaluate(vars);
			this.planetEfficiency = ability.PlanetEfficiency.Evaluate(vars);

			this.ammo = ability.Ammo.Evaluate(vars);
			this.speed = ability.Speed.Evaluate(vars); //TODO(v0.8) include player techs
			this.projectileImage = ability.ProjectileImage;

			this.splashMaxTargets = ability.SplashMaxTargets.Evaluate(vars);
			this.splashFirePower = ability.SplashFirePower.Evaluate(vars);
			this.splashArmorEfficiency = ability.SplashArmorEfficiency.Evaluate(vars);
			this.splashShieldEfficiency = ability.SplashShieldEfficiency.Evaluate(vars);
		}

		public void Visit(StarShootAbility ability)
		{
			var vars = new Var(AComponentType.LevelKey, this.level).Get;
			
			this.range = (int)ability.Range.Evaluate(vars);
			this.isInstantDamage = true;
			this.targetStar = true;
			
			this.energyCost = ability.EnergyCost.Evaluate(vars);
			this.appliesTrait = statics.StarTraits[ability.AppliesTraitId];
			this.ammo = ability.Ammo.Evaluate(vars);
		}
		#endregion
	}
}
