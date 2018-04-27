using Stareater.Galaxy;

namespace Stareater.GameData.Construction
{
	interface IConstructionEffect
	{
		void Apply(MainGame game, AConstructionSite site, long quantity);
    }
}
