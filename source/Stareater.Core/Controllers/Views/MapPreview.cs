using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;

namespace Stareater.Controllers.Views
{
	public class MapPreview
	{
		public IEnumerable<SystemPreview> Systems { get; private set; }
		public IEnumerable<WormholeInfo> Starlanes { get; private set; }

		internal MapPreview(IEnumerable<StarSystemBuilder> systems, HashSet<StarData> homewolrds, IEnumerable<Wormhole> starlanes)
		{
			//TODO(v0.8) calculate scores
			this.Systems = systems.Select(x => new SystemPreview(x.Star, homewolrds.Contains(x.Star), 0, 1)).ToList();
			this.Starlanes = starlanes.Select(x => new WormholeInfo(x)).ToList();
		}
	}
}
