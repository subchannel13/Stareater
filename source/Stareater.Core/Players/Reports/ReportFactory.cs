using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;

namespace Stareater.Players.Reports
{
	static class ReportFactory
	{
		public static IReport Load(IkonComposite reportData, ObjectDeindexer deindexer)
		{
			if (reportData.Tag.Equals(TechnologyReport.SaveTag))
				return TechnologyReport.Load(reportData, deindexer);

			//TODO(later): add error handling
			throw new NotImplementedException();
		}
	}
}
