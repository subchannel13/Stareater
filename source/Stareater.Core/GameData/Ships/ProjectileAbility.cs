namespace Stareater.GameData.Ships
{
	class ProjectileAbility : AAbilityType
	{
		public string ProjectileImage { get; private set; }

		public ProjectileAbility(string imagePath, string projectileImage)
			: base(imagePath)
		{
			this.ProjectileImage = projectileImage;
		}

		public override void Accept(IAbilityVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
