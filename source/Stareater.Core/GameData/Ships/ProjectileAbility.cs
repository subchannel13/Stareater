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

		public string ProjectileImage { get; private set; }

		public ProjectileAbility(string imagePath, 
			                     Formula firePower, Formula accuracy, Formula ammo, Formula speed,
			                     string projectileImage)
			: base(imagePath)
		{
			this.FirePower = firePower;
			this.Accuracy = accuracy;
			this.Ammo = ammo;
			this.Speed = speed;
			this.ProjectileImage = projectileImage;
		}

		public override void Accept(IAbilityVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
