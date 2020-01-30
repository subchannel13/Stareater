using Stareater.Utils.StateEngine;
using System;

namespace Stareater.Players.Reports
{
	static class ReportFactory
	{
		public static IReport Load(Ikadn.IkadnBaseObject reportData, LoadSession session)
		{
			if (reportData.Tag.Equals(ContactReport.SaveTag))
				return session.Load<ContactReport>(reportData);
			else if (reportData.Tag.Equals(DevelopmentReport.SaveTag))
				return session.Load<DevelopmentReport>(reportData);
			else if (reportData.Tag.Equals(ResearchReport.SaveTag))
				return session.Load<ResearchReport>(reportData);

			//TODO(later) add error handling
			throw new NotImplementedException();
		}
	}
}
