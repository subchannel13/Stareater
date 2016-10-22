using System;
using Stareater.Controllers.Views;

namespace Stareater.GUI.Reports
{
	class OpenReportVisitor : IReportInfoVisitor
	{
		private Action openDevelopment;
		private Action openResearch;

		public OpenReportVisitor(Action openDevelopment, Action openResearch)
		{
			this.openDevelopment = openDevelopment;
			this.openResearch = openResearch;
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
