using System;

namespace Stareater.GameLogic
{
	interface IConstructionVisitor
	{
		void Visit(ConstructionAddColonizer constructionEffect);
		void Visit(ConstructionAddShip constructionEffect);
		void Visit(ConstructionAddBuilding constructionEffect);
	}
}
