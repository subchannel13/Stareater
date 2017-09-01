using Ikadn;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.GameData.Construction
{
	static class ProjectFactory
	{
		public static IConstructionProject Load(IkadnBaseObject rawData, LoadSession session)
		{
			if (rawData.Tag.Equals(StaticProject.Tag))
				return session.Load<StaticProject>(rawData);
			else if (rawData.Tag.Equals(ShipProject.Tag))
				return session.Load<ShipProject>(rawData);
			else
				throw new KeyNotFoundException("Unknown construction project type: " + rawData.Tag);
		}
    }
}
