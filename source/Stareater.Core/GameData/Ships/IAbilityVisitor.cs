using System;

namespace Stareater.GameData.Ships
{
	interface IAbilityVisitor
	{
		void Visit(DirectShootAbility ability);
	}
}
