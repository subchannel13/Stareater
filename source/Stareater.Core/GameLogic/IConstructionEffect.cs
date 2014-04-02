using System;
using Stareater.Galaxy;
using Stareater.GameData.Databases;

namespace Stareater.GameLogic
{
	interface IConstructionEffect
	{
		void Apply(StatesDB states, AConstructionSite site, double quantity);
	}
}
