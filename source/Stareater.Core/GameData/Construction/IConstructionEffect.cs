using Stareater.Galaxy;
using Stareater.GameData.Databases;

namespace Stareater.GameData.Construction
{
	interface IConstructionEffect
	{
		void Apply(StatesDB states, TemporaryDB derivates, AConstructionSite site, long quantity);
    }
}
