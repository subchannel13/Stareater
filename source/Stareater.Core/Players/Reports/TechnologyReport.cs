using System;
using Ikadn;
using Stareater.AppData;
using Stareater.Controllers.Data;
using Stareater.GameData;
using Stareater.GameLogic;
using Stareater.Utils.Collections;

namespace Stareater.Players.Reports
{
	class TechnologyReport : IReport
	{
		public ActivityResult<TechnologyProgress> TechProgress { get; private set; }
		
		internal TechnologyReport(ActivityResult<TechnologyProgress> techProgress)
		{
			this.TechProgress = techProgress;
		}
		
		public void Accept(IReportVisitor visitor)
		{
			visitor.Visit(this);
		}
		
		public IkadnBaseObject Save(ObjectIndexer indexer)
		{
			//UNDONE(v0.5)
			throw new NotImplementedException();
		}
	}
}
