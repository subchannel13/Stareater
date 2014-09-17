using Ikadn;
using Stareater.Utils.Collections;

namespace Stareater.Players.Reports
{
	interface IReport
	{
		Player Owner { get; }
		
		void Accept(IReportVisitor visitor);
		IkadnBaseObject Save(ObjectIndexer indexer);
	}
}
