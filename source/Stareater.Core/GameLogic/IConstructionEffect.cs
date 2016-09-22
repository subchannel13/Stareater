using System;
using Stareater.Galaxy;
using Stareater.GameData.Databases;

namespace Stareater.GameLogic
{
	interface IConstructionEffect
	{
		void Apply(StatesDB states, TemporaryDB derivates, AConstructionSite site, long quantity);
		
		void Accept(IConstructionVisitor visitor);
	}
}
