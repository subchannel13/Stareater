using System;
using Stareater.Utils.Collections;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class ResearchProgressCollection : AIndexedCollection<ResearchProgress>
	{
		public Scalar2Index<ResearchProgress, Player, string> Of { get; private set; }

		public ResearchProgressCollection()
		{
			this.Of = new Scalar2Index<ResearchProgress, Player, string>(x => new Tuple<Player, string>(x.Owner, x.Topic.IdCode));
			this.registerIndices(this.Of);
		}
	}
}
