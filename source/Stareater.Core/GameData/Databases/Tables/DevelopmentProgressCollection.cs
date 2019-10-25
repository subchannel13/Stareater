using System;
using Stareater.Utils.Collections;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class DevelopmentProgressCollection : AIndexedCollection<DevelopmentProgress>
	{
		public Scalar2Index<DevelopmentProgress, Player, string> Of { get; private set; }

		public DevelopmentProgressCollection()
		{
			this.Of = new Scalar2Index<DevelopmentProgress, Player, string>(x => new Tuple<Player, string>(x.Owner, x.Topic.IdCode));
			this.registerIndices(this.Of);
		}
	}
}
