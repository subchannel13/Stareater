using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.NewGameHelpers;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;

namespace Stareater.Controllers.Views
{
	public class MapPreview
	{
		public IEnumerable<SystemPreview> Systems { get; private set; }
		public IEnumerable<WormholeInfo> Starlanes { get; private set; }

		internal MapPreview(IEnumerable<StarSystemBuilder> systems, HashSet<StarData> homewolrds, IEnumerable<Wormhole> starlanes, SystemEvaluator evaluator)
		{
			this.Systems = systems.Select(x => new SystemPreview(
				x.Star, 
				homewolrds.Contains(x.Star), 
				evaluator.StartingScore(x.Star, x.Planets), 
				evaluator.PotentialScore(x.Star, x.Planets)
			)).ToList();
			this.Starlanes = starlanes.Select(x => new WormholeInfo(x)).ToList();
		}
	}
}
