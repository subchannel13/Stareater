using System;
using Stareater.Controllers.Data;

namespace Stareater.GUI.Reports
{
	class OpenReportVisitor : IReportInfoVisitor
	{
		private Action openDevelopment;
		
		public OpenReportVisitor(Action openDevelopment)
		{
			this.openDevelopment = openDevelopment;
		}
		
		public void Visit(TechnologyReportInfo reportInfo)
		{
			this.openDevelopment(); //TODO(v0.5) check if development or research
		}
	}
}
