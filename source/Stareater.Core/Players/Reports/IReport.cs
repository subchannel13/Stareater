using Ikadn;
using Stareater.Utils.Collections;

namespace Stareater.Players.Reports
{
	interface IReport
	{
		void Accept(IReportVisitor visitor);
		IkadnBaseObject Save(ObjectIndexer indexer);
	}
}
