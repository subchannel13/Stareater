using System;
using Stareater.Controllers.Views;

namespace Stareater.GUI.Reports
{
	class OpenReportVisitor : IReportInfoVisitor
	{
		private readonly Action openDevelopment;
		private readonly Action openDiplomacy;
		private readonly Action openResearch;

		public OpenReportVisitor(Action openDevelopment, Action openDiplomacy, Action openResearch)
		{
			this.openDevelopment = openDevelopment;
			this.openDiplomacy = openDiplomacy;
			this.openResearch = openResearch;
		}

		public void Visit(ContactReportInfo reportInfo)
		{
			this.openDiplomacy();
		}

		public void Visit(DevelopmentReportInfo reportInfo)
		{
			this.openDevelopment();
		}

		public void Visit(ResearchReportInfo reportInfo)
		{
			this.openResearch();
		}
	}
}
