using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.GameData.Ships
{
	class ProjectileAbility : AAbilityType
	{
		public ProjectileAbility(string imagePath) : base(imagePath)
		{
			//TODO
		}

		public override void Accept(IAbilityVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
