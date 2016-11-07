using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Players;
using Stareater.Players.Reports;

namespace Stareater.GameData.Databases.Tables
{
	class ReportCollection : AIndexedCollection<IReport>
	{
		public CollectionIndex<IReport, Player> Of { get; private set; }

		public ReportCollection()
		{
			this.Of = new CollectionIndex<IReport, Player>(x => x.Owner);
			this.RegisterIndices(this.Of);
		}
	}
}
