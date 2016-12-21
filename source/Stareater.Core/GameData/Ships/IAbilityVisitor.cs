using System;

namespace Stareater.GameData.Ships
{
	interface IAbilityVisitor
	{
		void Visit(DirectShootAbility ability);
		void Visit(StarShootAbility ability);
	}
}
