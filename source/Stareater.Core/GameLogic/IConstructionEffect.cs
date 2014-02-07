using System;
using Stareater.Galaxy;

namespace Stareater.GameLogic
{
	interface IConstructionEffect
	{
		void Apply(AConstructionSite site, double quantity);
	}
}
